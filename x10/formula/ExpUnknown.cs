using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpUnknown : ExpBase {
    public string DiagnosticMessage { get; set; }

    public ExpUnknown(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      // If a message is present, report it
      if (DiagnosticMessage != null)
        Parser.Errors.AddError(this, DiagnosticMessage);
      return ExpDataType.ERROR;
    }
  }
}
