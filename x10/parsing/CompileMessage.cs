namespace x10.parsing {
  public enum CompileMessageSeverity {
    Info,
    Warning,
    Error,
  }

  public class CompileMessage {
    public string Message { get; set; }
    public CompileMessageSeverity Severity { get; set; }
    public TreeElement TreeElement { get; set; }

    public override string ToString() {
      return string.Format("{0}:{1}:{2} - {3}: {4}", TreeElement.FileInfo.FilePath, TreeElement.Start.LineNumber, TreeElement.Start.CharacterPosition, Severity, Message);
    }
  }
}