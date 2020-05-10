namespace x10.parsing {
  public enum CompileMessageSeverity {
    Info,
    Warning,
    Error,
  }

  public class CompileMessage {
    public string Message { get; set; }
    public CompileMessageSeverity Severity { get; set; }
    public IParseElement ParseElement { get; set; }

    public override string ToString() {
      return string.Format("{0}:{1}:{2} - {3}: {4}", ParseElement?.FileInfo?.FilePath, ParseElement?.Start?.LineNumber, ParseElement?.Start?.CharacterPosition, Severity, Message);
    }

    public override int GetHashCode() {
      return ToString().GetHashCode();
    }

    public override bool Equals(object obj) {
      return ToString() == obj.ToString();
    }

  }
}