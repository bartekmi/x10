using System;
using System.Collections.Generic;
using System.Text;
using x10.model;
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
      List<ExpDataType> argTypes = new List<ExpDataType>();
      foreach (ExpBase argumentExpression in Arguments)
        argTypes.Add(argumentExpression.DetermineType(rootType));

      Function function = Parser.AllFunctions.FindFunctionErrorIfMultiple(FunctionName, this);
      if (function == null)
        return ExpDataType.ERROR;

      if (argTypes.Count == function.Arguments.Count) {
        for (int ii = 0; ii < argTypes.Count; ii++) {
          Argument expectedArg = function.Arguments[ii];
          ExpDataType actualType = argTypes[ii];
          ExpBase expression = Arguments[ii];

          if (actualType != ExpDataType.ERROR && !actualType.Equals(new ExpDataType(expectedArg.Type)))
            Parser.Errors.AddError(expression, "For argument at position {0}, function '{1}' expects data type {2}, but was given {3}",
              ii + 1, FunctionName, expectedArg.Type, actualType);
        }
      } else
        Parser.Errors.AddError(this, "Function '{0}' expects {1} argument(s) but was given {2}",
          FunctionName, function.Arguments.Count, argTypes.Count);

      return new ExpDataType(function.ReturnType);
    }
  }
}
