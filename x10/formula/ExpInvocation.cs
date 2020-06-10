using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpInvocation : ExpBase {
    public string FunctionName { get; set; }
    public List<ExpBase> Arguments { get; set; }

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      // TODO... Potentially need external source of function signatures
      foreach (ExpBase argumentExpression in Arguments)
        argumentExpression.DetermineType(errors, context, rootType);

      return ExpDataType.NULL;
    }
  }
}
