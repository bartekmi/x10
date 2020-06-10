using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpUnknown : ExpBase {
    public string DiagnosticMessage { get; set; }

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      // If a message is present, report it
      if (DiagnosticMessage != null)
        errors.AddError(this, DiagnosticMessage);
      return ExpDataType.ERROR;
    }
  }
}
