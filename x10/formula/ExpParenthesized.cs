using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpParenthesized : ExpBase {
    public ExpBase Expression { get; set; }

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      return Expression.DetermineType(errors, context, rootType);
    }
  }
}
