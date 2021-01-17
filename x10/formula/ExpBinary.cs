using System;
using System.Collections.Generic;
using x10.model.metadata;

namespace x10.formula {
  public class ExpBinary : ExpBase {
    public string Token { get; set; }
    public ExpBase Left { get; set; }
    public ExpBase Right { get; set; }

    // Derived
    public bool IsComparison => Token == "<" || Token == ">" || Token == "<=" || Token == ">=";

    public ExpBinary(FormulaParser parser) : base(parser) { 
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitBinary(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return new ExpBase[] { Left, Right };
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      X10DataType leftType = Left.DetermineType(rootType);
      X10DataType rightType = Right.DetermineType(rootType);

      if (leftType.IsError || rightType.IsError)
        return X10DataType.ERROR;

      switch (Token) {
        case "-":
        case "/":
        case "*":
          if (!leftType.IsNumeric || !rightType.IsNumeric)
            return MismatchTypeError(leftType, rightType);
          return ResultOfNumericOperation(leftType, rightType);
        case "%":
          if (!leftType.IsInteger || !rightType.IsInteger)
            return MismatchTypeError(leftType, rightType);
          return X10DataType.Integer;
        case "+":
          if (leftType.IsNumeric && rightType.IsNumeric)
            return ResultOfNumericOperation(leftType, rightType);

          if (leftType.IsString || rightType.IsString)
            return X10DataType.String;

          return MismatchTypeError(leftType, rightType);
        case "&&":
        case "||":
          if (!leftType.IsBoolean || !rightType.IsBoolean)
            return MismatchTypeError(leftType, rightType);
          return X10DataType.Boolean;
        case ">":
        case "<":
        case ">=":
        case "<=":
          if (leftType.IsComparable && leftType.DataType == rightType.DataType)
            return X10DataType.Boolean;
          return MismatchTypeError(leftType, rightType);
        case "==":
        case "!=":
          if (leftType.IsNumeric && rightType.IsNumeric ||
              leftType.Equals(rightType) ||
              leftType.IsError || rightType.IsError ||
              leftType.IsNull || rightType.IsNull)
            return X10DataType.Boolean;

          // Upgrade right side to enum if possible
          if (leftType.IsEnum && Right is ExpLiteral rightLiteral) {
            rightLiteral.UpgradeToEnum(leftType.DataTypeAsEnum);
            return X10DataType.Boolean;
          }

          // Upgrade left side to enum if possible
          if (rightType.IsEnum && Left is ExpLiteral leftLiteral) {
            leftLiteral.UpgradeToEnum(rightType.DataTypeAsEnum);
            return X10DataType.Boolean;
          }

          return MismatchTypeError(leftType, rightType);
        case "??":
          if (leftType.IsError || rightType.IsError)
            return X10DataType.ERROR;
          if (leftType.Equals(rightType))
            return leftType;

          string message = string.Format("Both sides of ?? operator must be the same, but left is {0} and right is {1}",
            leftType, rightType);
          Parser.Errors.AddError(this, message);

          return X10DataType.ERROR;
        default:
          Parser.Errors.AddError(this, "Unexpected binary token: " + Token);
          return X10DataType.ERROR;
      }
    }

    private X10DataType MismatchTypeError(X10DataType left, X10DataType right) {
      string message = string.Format("Cannot {0} {1} and {2}",
        TokenToOperationName(), left, right);

      Parser.Errors.AddError(this, message);

      return X10DataType.ERROR;
    }

    private string TokenToOperationName() {
      switch (Token) {
        case "-": return "subtract";
        case "*": return "multiply";
        case "/": return "divide";
        case "%": return "take remainder of";
        case "+": return "add";
        case "&&": return "logical-and";
        case "||": return "logical-or";
        case ">":
        case ">=":
        case "<":
        case "<=":
          return "compare";
        case "==": return "test equality of";
        case "!=": return "test inequality of";
        default:
          throw new Exception("Unexpected binary operation token: " + Token);
      }
    }

    private static X10DataType ResultOfNumericOperation(X10DataType left, X10DataType right) {
      return left.IsFloat || right.IsFloat ? X10DataType.Float : X10DataType.Integer;
    }
  }
}
