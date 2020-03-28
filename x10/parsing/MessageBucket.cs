using System.Collections.Generic;
using System.Linq;

namespace x10.parsing {
  public class MessageBucket {
    public List<CompileMessage> Messages { get; private set; }

    // Derived
    public bool HasErrors { get { return Messages.Any(x => x.Severity == CompileMessageSeverity.Error); } }
    public bool IsEmpty { get { return Messages.Count == 0; } }
    public int Count { get { return Messages.Count; } }

    public MessageBucket() {
      Messages = new List<CompileMessage>();
    }

    public void Clear() {
      Messages.Clear();
    }

    public void AddError(TreeElement element, string message) {
      AddMessage(CompileMessageSeverity.Error, element, message);
    }

    public void AddWarning(TreeElement element, string message) {
      AddMessage(CompileMessageSeverity.Warning, element, message);
    }

    public void AddInfo(TreeElement element, string message) {
      AddMessage(CompileMessageSeverity.Info, element, message);
    }

    public void AddMessage(CompileMessageSeverity severity, TreeElement element, string message) {
      Add(new CompileMessage() {
        TreeElement = element,
        Message = message,
        Severity = severity,
      });
    }

    public void Add(CompileMessage error) {
      Messages.Add(error);
    }
  }
}