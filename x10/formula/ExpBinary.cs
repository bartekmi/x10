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

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      ExpDataType leftType = Left.DetermineType(errors, context, rootType);
      ExpDataType rightType = Right.DetermineType(errors, context, rootType);

      switch (Token) {
        case "-":
        case "/":
        case "*":
          if (!leftType.IsNumeric || !rightType.IsNumeric) {
            errors.AddError(this, "Both sides of operator '{0}' must be numeric (Integer or Float)", Token);
            return ExpDataType.ERROR;
          }
          return ResultOfNumericOperation(leftType, rightType);
        case "%":
          if (!leftType.IsInteger || !rightType.IsInteger) {
            errors.AddError(this, "Both sides of operator '%' must be of type Integer", Token);
            return ExpDataType.ERROR;
          }
          return ExpDataType.Integer;
        case "+":
          if (leftType.IsNumeric && rightType.IsNumeric)
            return ResultOfNumericOperation(leftType, rightType);

          if (leftType.IsString || rightType.IsString)
            return ExpDataType.String;

          errors.AddError(this, "Eith both sides of operator '+' must be numeric (Integer or Float), or one side must be of type String");
          return ExpDataType.ERROR;
        case "&&":
        case "||":
          if (!leftType.IsBoolean || !rightType.IsBoolean) {
            errors.AddError(this, "Both sides of '{0}' must be Boolean (True/False)", Token);
            return ExpDataType.ERROR;
          }
          return ExpDataType.Boolean;
        case ">":
        case "<":
        case ">=":
        case "<=":
          if (leftType.IsNumeric && rightType.IsNumeric ||
              leftType.IsComparable && rightType.IsComparable &&
              leftType.DataType == rightType.DataType)
            return ExpDataType.Boolean;

          errors.AddError(this, "Cannot compare {0} and {1}", leftType, rightType);
          return ExpDataType.ERROR;
        case "==":
        case "!=":
          if (leftType.IsNumeric && rightType.IsNumeric ||
              leftType.Equals(rightType) ||
              leftType.IsError || rightType.IsError)
            return ExpDataType.Boolean;

          errors.AddError(this, "Cannot compare {0} and {1}", leftType, rightType);
          return ExpDataType.ERROR;
        default:
          errors.AddError(this, "Unexpected token: " + Token);
          return ExpDataType.ERROR;
      }
    }

    private static ExpDataType ResultOfNumericOperation(ExpDataType left, ExpDataType right) {
      return left.IsFloat || right.IsFloat ? ExpDataType.Float : ExpDataType.Integer;
    }
  }
}
