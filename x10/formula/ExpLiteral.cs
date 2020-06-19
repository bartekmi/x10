using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpLiteral : ExpBase {
    public object Value { get; set; }

    public ExpLiteral(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitLiteral(this);
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      if (Value is string)
        return new ExpDataType(DataTypes.Singleton.String);
      if (Value is int)
        return new ExpDataType(DataTypes.Singleton.Integer);
      if (Value is double)
        return new ExpDataType(DataTypes.Singleton.Float);
      if (Value is bool)
        return new ExpDataType(DataTypes.Singleton.Boolean);
      if (Value is int)
        return new ExpDataType(DataTypes.Singleton.Integer);

      // TODO: Deal with legitimate null

      Parser.Errors.AddError(this, "Unexpected Data Type: " + Value.GetType().Name);
      return ExpDataType.ERROR;
    }
  }
}
