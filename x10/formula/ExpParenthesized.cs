using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpParenthesized : ExpBase {
    public ExpBase Expression { get; set; }

    public ExpParenthesized(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      return Expression.DetermineType(rootType);
    }
  }
}
