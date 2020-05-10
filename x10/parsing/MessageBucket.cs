using System.Collections.Generic;
using System.Linq;

namespace x10.parsing {
  public class MessageBucket {
    public HashSet<CompileMessage> Messages { get; private set; }

    // Derived
    public bool HasErrors { get { return Messages.Any(x => x.Severity == CompileMessageSeverity.Error); } }
    public bool IsEmpty { get { return Messages.Count == 0; } }
    public int Count { get { return Messages.Count; } }

    public MessageBucket() {
      Messages = new HashSet<CompileMessage>();
    }

    public IEnumerable<CompileMessage> FilteredMessages(CompileMessageSeverity? severities) {
      if (severities == null)
        return Messages;

      return Messages.Where(x => (x.Severity & severities.Value) > 0);
    }

    public void Clear() {
      Messages.Clear();
    }

    public void AddError(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Error, element, message, substitutions);
    }

    public void AddWarning(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Warning, element, message, substitutions);
    }

    public void AddInfo(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Info, element, message, substitutions);
    }

    public void AddMessage(CompileMessageSeverity severity, IParseElement element, string message, params object[] substitutions) {

      if (substitutions != null && substitutions.Length > 0)
        message = string.Format(message, substitutions);

      Add(new CompileMessage() {
        ParseElement = element,
        Message = message,
        Severity = severity,
      });
    }

    public void Add(CompileMessage error) {
      Messages.Add(error);
    }
  }
}