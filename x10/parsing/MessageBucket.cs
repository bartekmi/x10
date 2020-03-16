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

        public void Add(CompileMessage error) {
            Messages.Add(error);
        }
    }
}