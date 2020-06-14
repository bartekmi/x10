using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpInvocation : ExpBase {
    public string FunctionName { get; set; }
    public List<ExpBase> Arguments { get; set; }

    public ExpInvocation(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      // TODO... Potentially need external source of function signatures
      foreach (ExpBase argumentExpression in Arguments)
        argumentExpression.DetermineType(rootType);

      return ExpDataType.NULL;
    }
  }
}
