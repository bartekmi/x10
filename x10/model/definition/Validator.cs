using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {

  public enum ValidatorSeverity {
    Info,
    Warning,
    Error,
  }

  public class Validator {
    public string Message { get; set; }
    public ValidatorSeverity Severity { get; set; }
    public string Rule { get; set; }
  }
}
