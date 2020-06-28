using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

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

    public override ExpDataType DetermineTypeRaw(ExpDataType rootType) {
      ExpDataType expressionType = Expression.DetermineType(rootType);

      if (expressionType.IsError )
        return ExpDataType.ERROR;

      switch (Token) {
        case "-":
        case "+":
          if (expressionType.IsNumeric)
            return expressionType;
          break;
        case "!":
          if (expressionType.IsBoolean)
            return ExpDataType.Boolean;
          break;
        default:
          Parser.Errors.AddError(this, "Unexpected unary token: " + Token);
          return ExpDataType.ERROR;
      }

      return MismatchTypeError(expressionType);
    }

    private ExpDataType MismatchTypeError(ExpDataType type) {
      string message = string.Format("Cannot apply '{0}' to type {1}",
        Token, type);

      Parser.Errors.AddError(this, message);

      return ExpDataType.ERROR;
    }
  }
}
