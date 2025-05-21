WaveformSampleの例外処理設計について検討してみます。コードを分析した結果、以下のような階層構造があることがわかりました：

1. **Form (UI層)**
2. **ProjectService (サービス層)**
3. **AppContext (アプリケーションコンテキスト層)**
4. **ProjectContext (プロジェクトコンテキスト層)**
5. **Project (モデル層)**

それぞれのレイヤーでの例外処理と伝播の改善案を提案します。

## 1. 例外階層の整備

まず、アプリケーション固有の例外階層を作成します：

```csharp
// 基本となるアプリケーション例外クラス
public class WaveformApplicationException : Exception
{
    public ErrorSeverity Severity { get; private set; }

    public WaveformApplicationException(string message, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message)
    {
        Severity = severity;
    }

    public WaveformApplicationException(string message, Exception innerException, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message, innerException)
    {
        Severity = severity;
    }
}

// 機能別の例外クラス
public class ProjectOperationException : WaveformApplicationException
{
    public ProjectOperationException(string message, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message, severity) { }
    
    public ProjectOperationException(string message, Exception innerException, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message, innerException, severity) { }
}

public class SequenceOperationException : WaveformApplicationException
{
    public SequenceOperationException(string message, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message, severity) { }
    
    public SequenceOperationException(string message, Exception innerException, ErrorSeverity severity = ErrorSeverity.Error) 
        : base(message, innerException, severity) { }
}

// エラーの重大度を表す列挙型
public enum ErrorSeverity
{
    Info,
    Warning,
    Error,
    Critical
}
```

## 2. 各レイヤーごとの例外処理方針

### Project層 (モデル層)

モデル層では低レベルの例外をスローし、詳細なエラー情報を提供します。

```csharp
public class Project
{
    // 既存のコード...

    public void MonitorWaveformStepChanges()
    {
        try
        {
            // 実装...
        }
        catch (Exception ex)
        {
            throw new WaveformApplicationException("波形ステップ変更の監視中にエラーが発生しました。", ex, ErrorSeverity.Warning);
        }
    }

    // その他のメソッド...
}
```

### ProjectContext層

この層では、Project層の例外をキャッチし、より具体的なコンテキスト情報を付加します。

```csharp
public class ProjectContext
{
    // 既存のコード...

    public void SaveProject(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
        }

        if (CurrentProject == null)
        {
            throw new ProjectOperationException("保存するプロジェクトが存在しません。", ErrorSeverity.Error);
        }

        try
        {
            // プロジェクト名とディレクトリパスを取得
            string projectFileName = Path.GetFileNameWithoutExtension(filePath);
            // 残りの実装...
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new ProjectOperationException("ファイルの保存先へのアクセス権限がありません。", ex, ErrorSeverity.Error);
        }
        catch (IOException ex)
        {
            throw new ProjectOperationException($"プロジェクトファイルの保存中にエラーが発生しました: {filePath}", ex, ErrorSeverity.Error);
        }
        catch (Exception ex)
        {
            throw new ProjectOperationException($"プロジェクトの保存中に予期しないエラーが発生しました: {filePath}", ex, ErrorSeverity.Critical);
        }
    }

    // 他のメソッドも同様に修正...
}
```

### AppContext層

AppContext層では、特定のコンテキストに関連する例外のみを処理し、それ以外はそのまま伝播させます。

```csharp
public class AppContext
{
    // 既存のコード...

    public void SaveProject(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
        }

        try
        {
            ProjectContext.SaveProject(filePath);
        }
        catch (ProjectOperationException)
        {
            // 既に適切な例外がスローされている場合はそのまま伝播
            throw;
        }
        catch (Exception ex)
        {
            throw new ProjectOperationException($"プロジェクトの保存中にエラーが発生しました: {filePath}", ex, ErrorSeverity.Error);
        }
    }

    // 他のメソッドも同様に修正...
}
```

### ProjectService層

ProjectService層は、ビジネスロジックを実行し、業務ルールに関連する検証や例外処理を行います。

```csharp
public class ProjectService
{
    private AppContext _appContext;

    // 既存のコード...

    public void SaveProjectAs(string filePath)
    {
        try
        {
            ValidateProjectPath(filePath);
            _appContext.SaveProject(filePath);
        }
        catch (ArgumentNullException ex)
        {
            throw new ProjectOperationException("プロジェクト保存に必要な情報が不足しています。", ex, ErrorSeverity.Warning);
        }
        catch (ProjectOperationException)
        {
            // 既に適切な例外がスローされている場合はそのまま伝播
            throw;
        }
        catch (Exception ex)
        {
            throw new ProjectOperationException($"プロジェクトの保存処理中にエラーが発生しました。", ex, ErrorSeverity.Error);
        }
    }

    private void ValidateProjectPath(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
        }

        // その他の検証ロジック
        string extension = Path.GetExtension(filePath);
        if (extension != ".wproj")
        {
            throw new ProjectOperationException($"プロジェクトファイルの拡張子は .wproj である必要があります。指定された拡張子: {extension}", ErrorSeverity.Warning);
        }
    }

    // 他のメソッドも同様に修正...
}
```

### Form層 (UI層)

Form層では、ユーザーフレンドリーなエラーメッセージを表示し、適切なUIフィードバックを提供します。

```csharp
public partial class Form1 : Form
{
    // プロジェクトサービスのインスタンス
    private ProjectService _projectService;
    private string _baseTitle = "波形サンプルアプリケーション";
    
    // エラー処理用インスタンス
    private ErrorHandler _errorHandler;

    public Form1()
    {
        InitializeComponent();
        _projectService = new ProjectService();
        _errorHandler = new ErrorHandler(this); // エラーハンドラの初期化
    }

    private void btnSaveProject_Click(object sender, EventArgs e)
    {
        try
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Waveform Project|*.wproj";
                saveDialog.Title = "プロジェクトを保存";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _projectService.SaveProjectAs(saveDialog.FileName);
                    UpdateFormTitle(); // 保存成功後にフォームタイトルを更新
                    MessageBox.Show("プロジェクトを保存しました。", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            _errorHandler.HandleException(ex);
        }
    }
    
    private void UpdateFormTitle()
    {
        var project = _projectService.GetCurrentProject();
        if (project != null)
        {
            string projectName = string.IsNullOrEmpty(project.Name) ? "無題のプロジェクト" : project.Name;
            string dirtyMark = project.IsDirty ? "*" : "";
            this.Text = $"{_baseTitle} - {projectName}{dirtyMark}";
        }
        else
        {
            this.Text = _baseTitle;
        }
    }

    // 他のイベントハンドラも同様に修正...
}
```

## 3. エラーハンドラの実装

UI層での例外処理を統一するためのエラーハンドラクラス：

```csharp
public class ErrorHandler
{
    private Form _parentForm;
    private ILogger _logger;

    public ErrorHandler(Form parentForm)
    {
        _parentForm = parentForm;
        _logger = new FileLogger("error.log"); // ロガーの初期化
    }

    public void HandleException(Exception ex)
    {
        // 例外のログ記録
        _logger.LogError(ex);

        // 例外の種類によって異なる処理
        if (ex is WaveformApplicationException appEx)
        {
            DisplayApplicationError(appEx);
        }
        else if (ex is ArgumentException)
        {
            DisplayArgumentError(ex as ArgumentException);
        }
        else if (ex is IOException)
        {
            DisplayIOError(ex as IOException);
        }
        else
        {
            // その他の一般的な例外
            DisplayGeneralError(ex);
        }
    }

    private void DisplayApplicationError(WaveformApplicationException ex)
    {
        MessageBoxIcon icon = MessageBoxIcon.Error;
        
        // 重大度に応じてアイコンを変更
        switch (ex.Severity)
        {
            case ErrorSeverity.Info:
                icon = MessageBoxIcon.Information;
                break;
            case ErrorSeverity.Warning:
                icon = MessageBoxIcon.Warning;
                break;
            case ErrorSeverity.Error:
                icon = MessageBoxIcon.Error;
                break;
            case ErrorSeverity.Critical:
                icon = MessageBoxIcon.Stop;
                break;
        }

        string title = ex.Severity.ToString();
        MessageBox.Show(_parentForm, ex.Message, title, MessageBoxButtons.OK, icon);
    }

    private void DisplayArgumentError(ArgumentException ex)
    {
        MessageBox.Show(_parentForm, $"入力エラー: {ex.Message}", "パラメータエラー", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void DisplayIOError(IOException ex)
    {
        MessageBox.Show(_parentForm, $"ファイル操作エラー: {ex.Message}", "IOエラー", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void DisplayGeneralError(Exception ex)
    {
        MessageBox.Show(_parentForm, $"エラーが発生しました: {ex.Message}", "システムエラー", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        
        // 深刻なエラーの場合は詳細を表示するオプションを提供
        if (MessageBox.Show(_parentForm, "エラーの詳細を表示しますか？", "詳細情報", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            ShowDetailedErrorDialog(ex);
        }
    }

    private void ShowDetailedErrorDialog(Exception ex)
    {
        // 詳細なエラー情報を表示するためのカスタムダイアログを表示
        ErrorDetailDialog dialog = new ErrorDetailDialog(ex);
        dialog.ShowDialog(_parentForm);
    }
}
```

## 4. エラーログの実装

```csharp
public interface ILogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
    void LogError(Exception ex);
}

public class FileLogger : ILogger
{
    private string _logFilePath;

    public FileLogger(string logFileName)
    {
        string appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WaveformSample",
            "Logs");
            
        // ログディレクトリの作成
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        _logFilePath = Path.Combine(appDataPath, logFileName);
    }

    public void LogInfo(string message)
    {
        WriteLogEntry("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteLogEntry("WARNING", message);
    }

    public void LogError(string message)
    {
        WriteLogEntry("ERROR", message);
    }

    public void LogError(Exception ex)
    {
        string message = $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
        
        // 内部例外がある場合はそれも記録
        if (ex.InnerException != null)
        {
            message += $"\nInner Exception: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}\n{ex.InnerException.StackTrace}";
        }
        
        LogError(message);
    }

    private void WriteLogEntry(string level, string message)
    {
        try
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // ログの書き込み自体が失敗した場合は黙って失敗（デバッグモードでのみ例外をスロー可能）
#if DEBUG
            throw;
#endif
        }
    }
}
```

## 5. エラー詳細表示ダイアログ

```csharp
public partial class ErrorDetailDialog : Form
{
    public ErrorDetailDialog(Exception ex)
    {
        InitializeComponent();
        
        this.Text = $"エラー詳細: {ex.GetType().Name}";
        textBoxErrorDetails.Text = FormatException(ex);
        textBoxErrorDetails.Select(0, 0); // カーソルを先頭に
    }

    private string FormatException(Exception ex, int level = 0)
    {
        StringBuilder sb = new StringBuilder();
        string indent = new string(' ', level * 4);
        
        sb.AppendLine($"{indent}例外の種類: {ex.GetType().FullName}");
        sb.AppendLine($"{indent}メッセージ: {ex.Message}");
        sb.AppendLine($"{indent}発生場所: {ex.Source}");
        sb.AppendLine($"{indent}スタックトレース:");
        sb.AppendLine($"{indent}{ex.StackTrace}");

        if (ex.InnerException != null)
        {
            sb.AppendLine();
            sb.AppendLine($"{indent}内部例外:");
            sb.AppendLine(FormatException(ex.InnerException, level + 1));
        }

        return sb.ToString();
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(textBoxErrorDetails.Text))
        {
            Clipboard.SetText(textBoxErrorDetails.Text);
            MessageBox.Show("エラー詳細がクリップボードにコピーされました。", "コピー完了", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
```

## 6. 実装の方針

1. **下位層（Project, ProjectContext）** - 具体的かつ詳細なエラー情報を提供
2. **中間層（AppContext, ProjectService）** - ビジネスルールに関連するエラーを追加・変換
3. **上位層（Form）** - ユーザーフレンドリーな表示とロギング

## 7. エラーメッセージの決め方

1. **エラーの原因を明確に** - 何が問題なのかを明確に伝える
2. **ユーザーの操作ミスの場合** - 正しい操作方法を示す
3. **システムエラーの場合** - ユーザーが取るべき行動を示す
4. **重大度に応じた表現** - 軽微なエラーと深刻なエラーで表現を変える
5. **技術的詳細はログに** - UIには簡潔なメッセージ、詳細はログに記録

## 改善後の流れ

以下は、フォームからプロジェクト保存を行う際の例外処理の流れです：

1. **Form**: ユーザーが「保存」ボタンをクリック
2. **Form**: `_projectService.SaveProjectAs(path)` を呼び出し、例外をキャッチ
3. **ProjectService**: パスの検証後、`_appContext.SaveProject(path)` を呼び出し
4. **AppContext**: `ProjectContext.SaveProject(path)` を呼び出し
5. **ProjectContext**: 実際の保存処理を実行、問題があれば具体的な例外をスロー
6. **例外の伝播**: 下位層→上位層へ伝播し、適宜変換・補足情報の追加
7. **Form**: キャッチした例外を `_errorHandler.HandleException(ex)` に渡す
8. **ErrorHandler**: 例外の種類と重大度に応じたメッセージ表示とログ記録

この設計により、各レイヤーでの責任が明確になり、ユーザーに適切なエラー情報が提供されます。また、詳細なログ記録により、問題の特定と解決が容易になります。