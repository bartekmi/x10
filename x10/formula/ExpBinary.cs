using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpBinary : ExpBase {
    public string Token { get; set; }
    public ExpBase Left { get; set; }
    public ExpBase Right { get; set; }

    public ExpBinary(FormulaParser parser) : base(parser) { 
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitBinary(this);
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      ExpDataType leftType = Left.DetermineType(rootType);
      ExpDataType rightType = Right.DetermineType(rootType);

      if (leftType.IsError || rightType.IsError)
        return ExpDataType.ERROR;

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
          return ExpDataType.Integer;
        case "+":
          if (leftType.IsNumeric && rightType.IsNumeric)
            return ResultOfNumericOperation(leftType, rightType);

          if (leftType.IsString || rightType.IsString)
            return ExpDataType.String;

          return MismatchTypeError(leftType, rightType);
        case "&&":
        case "||":
          if (!leftType.IsBoolean || !rightType.IsBoolean)
            return MismatchTypeError(leftType, rightType);
          return ExpDataType.Boolean;
        case ">":
        case "<":
        case ">=":
        case "<=":
          if (leftType.IsNumeric && rightType.IsNumeric ||
              leftType.IsComparable && rightType.IsComparable &&
              leftType.DataType == rightType.DataType)
            return ExpDataType.Boolean;
          return MismatchTypeError(leftType, rightType);
        case "==":
        case "!=":
          if (leftType.IsNumeric && rightType.IsNumeric ||
              leftType.Equals(rightType) ||
              leftType.IsError || rightType.IsError)
            return ExpDataType.Boolean;

          // Upgrade right side to enum if possible
          if (leftType.IsEnum && Right is ExpLiteral rightLiteral) {
            rightLiteral.UpgradeToEnum(leftType.DataTypeAsEnum);
            return ExpDataType.Boolean;
          }

          // Upgrade left side to enum if possible
          if (rightType.IsEnum && Left is ExpLiteral leftLiteral) {
            leftLiteral.UpgradeToEnum(rightType.DataTypeAsEnum);
            return ExpDataType.Boolean;
          }

          return MismatchTypeError(leftType, rightType);
        case "??":
          if (leftType.IsError || rightType.IsError)
            return ExpDataType.ERROR;
          if (leftType.Equals(rightType))
            return leftType;

          string message = string.Format("Both sides of ?? operator must be the same, but left is {0} and right is {1}",
            leftType, rightType);
          Parser.Errors.AddError(this, message);

          return ExpDataType.ERROR;
        default:
          Parser.Errors.AddError(this, "Unexpected token: " + Token);
          return ExpDataType.ERROR;
      }
    }

    private ExpDataType MismatchTypeError(ExpDataType left, ExpDataType right) {
      string message = string.Format("Cannot {0} {1} and {2}",
        TokenToOperationName(), left, right);

      Parser.Errors.AddError(this, message);

      return ExpDataType.ERROR;
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

    private static ExpDataType ResultOfNumericOperation(ExpDataType left, ExpDataType right) {
      return left.IsFloat || right.IsFloat ? ExpDataType.Float : ExpDataType.Integer;
    }
  }
}
