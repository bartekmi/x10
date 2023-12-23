using System;
using System.Collections.Generic;
using System.Linq;

namespace x10.parsing {
  public class MessageBucket {
    public HashSet<CompileMessage> Messages { get; private set; }
    public bool ThrowExceptionOnFirstError { get; set; }

    // Derived
    public bool HasErrors { get { return Messages.Any(x => x.Severity == CompileMessageSeverity.Error); } }
    public int ErrorCount { get { return Messages.Count(x => x.Severity == CompileMessageSeverity.Error); } }
    public IEnumerable<CompileMessage> Errors { get { return Messages.Where(x => x.Severity == CompileMessageSeverity.Error); } }
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

    private string _preamble;
    public void PushPreamble(string preamble) {
      if (_preamble != null)
        throw new Exception("Only one push allowed");
      _preamble = preamble;
    }
    public void PopPreamble() {
      _preamble = null;
    }

    public void AddError(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Error, element, message, substitutions);
    }

    public void AddErrorDidYouMean(
      IParseElement element, 
      string actualValue, 
      IEnumerable<string> allowedValues, 
      string message, 
      params object[] substitutions) {

      AddMessage(CompileMessageSeverity.Error, element, actualValue, allowedValues, message, substitutions);
    }

    public void AddWarning(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Warning, element, message, substitutions);
    }

    public void AddInfo(IParseElement element, string message, params object[] substitutions) {
      AddMessage(CompileMessageSeverity.Info, element, message, substitutions);
    }

    public void AddMessage(CompileMessageSeverity severity, IParseElement element, string message, params object[] substitutions) {
      AddMessage(severity, element, null, null, message, substitutions);
    }

    public void AddMessage(
      CompileMessageSeverity severity, 
      IParseElement element, 
      string actualValue, 
      IEnumerable<string> allowedValues, 
      string message, 
      params object[] substitutions) {

      if (substitutions != null && substitutions.Length > 0)
        message = string.Format(message, substitutions);

      if (ThrowExceptionOnFirstError && severity == CompileMessageSeverity.Error)
        throw new System.Exception(message);

      CompileMessage compileMessage = new CompileMessage() {
        ParseElement = element,
        Message = _preamble == null ? message : string.Format("{0} > {1}", _preamble, message),
        Severity = severity,
        ActualValue = actualValue,
        AllowedValues = allowedValues,
      };

      Add(compileMessage);
    }

    public void Add(CompileMessage message) {
      Messages.Add(message);
    }

    #region Output
    public void DumpErrors() {
      foreach (CompileMessage message in Errors)
        Console.WriteLine(message);
    }
    #endregion
  }
}