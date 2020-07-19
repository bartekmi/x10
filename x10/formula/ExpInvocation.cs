using System.Collections.Generic;
using x10.model;
using x10.model.metadata;

namespace x10.formula {
  public class ExpInvocation : ExpBase {
    public string FunctionName { get; set; }
    public List<ExpBase> Arguments { get; set; }

    public ExpInvocation(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitInvocation(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return Arguments;
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      List<X10DataType> argTypes = new List<X10DataType>();
      foreach (ExpBase argumentExpression in Arguments)
        argTypes.Add(argumentExpression.DetermineType(rootType));

      Function function = Parser.AllFunctions.FindFunctionErrorIfMultiple(FunctionName, this);
      if (function == null)
        return X10DataType.ERROR;

      if (argTypes.Count == function.Arguments.Count) {
        for (int ii = 0; ii < argTypes.Count; ii++) {
          Argument expectedArg = function.Arguments[ii];
          X10DataType actualType = argTypes[ii];
          ExpBase expression = Arguments[ii];

          if (actualType == X10DataType.ERROR)
            continue;

          if (expectedArg.Type is DataTypeEnum enumType && expression is ExpLiteral literal) {
            literal.UpgradeToEnum(enumType);
            continue;
          }

          if (!actualType.Equals(new X10DataType(expectedArg.Type)))
            Parser.Errors.AddError(expression, "For argument at position {0}, function '{1}' expects data type {2}, but was given {3}",
              ii + 1, FunctionName, expectedArg.Type, actualType);
        }
      } else
        Parser.Errors.AddError(this, "Function '{0}' expects {1} argument(s) but was given {2}",
          FunctionName, function.Arguments.Count, argTypes.Count);

      return new X10DataType(function.ReturnType);
    }
  }
}
