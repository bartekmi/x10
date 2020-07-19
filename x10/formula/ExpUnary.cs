using System.Collections.Generic;
using x10.model.metadata;

namespace x10.formula {
  public class ExpUnary : ExpBase {
    public string Token { get; set; }
    public ExpBase Expression { get; set; }

    public ExpUnary(FormulaParser parser) : base(parser) { 
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitUnary(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return new ExpBase[] { Expression };
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      X10DataType expressionType = Expression.DetermineType(rootType);

      if (expressionType.IsError )
        return X10DataType.ERROR;

      switch (Token) {
        case "-":
        case "+":
          if (expressionType.IsNumeric)
            return expressionType;
          break;
        case "!":
          if (expressionType.IsBoolean)
            return X10DataType.Boolean;
          break;
        default:
          Parser.Errors.AddError(this, "Unexpected unary token: " + Token);
          return X10DataType.ERROR;
      }

      return MismatchTypeError(expressionType);
    }

    private X10DataType MismatchTypeError(X10DataType type) {
      string message = string.Format("Cannot apply '{0}' to type {1}",
        Token, type);

      Parser.Errors.AddError(this, message);

      return X10DataType.ERROR;
    }
  }
}
