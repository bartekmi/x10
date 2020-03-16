namespace x10.parsing {
  public enum CompileMessageSeverity {
    Info,
    Warning,
    Error,
  }

  public class CompileMessage {
    public string Message { get; set; }
    public CompileMessageSeverity Severity { get; set; }
    public TreeNode TreeNode { get; set; }
  }
}