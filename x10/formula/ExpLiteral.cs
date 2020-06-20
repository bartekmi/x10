using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpLiteral : ExpBase {
    public object Value { get; set; }

    // Enums in formulas are represented simply as string contants
    // - e.g. "myEnumValue"
    // During the formula validation steps (where the type of each
    // expression is determined), if an enum value is expected and
    // String literal is encoutered, we attempt to convert and validate it.
    public DataTypeEnum InferredEnumType { get; private set; }
    public EnumValue InferredEnumValue { get; private set; }

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

    internal void UpgradeToEnum(DataTypeEnum enumType) {
      InferredEnumType = enumType;

      if (Value is String) {
        InferredEnumValue = enumType.FindEnumValue(Value);
        if (InferredEnumValue == null)
          Parser.Errors.AddError(this, "Attempted to upgrade '{0}' to Enumerated Type {1}. No such value exists. Available values: {2}",
            Value, enumType.Name, enumType.AvailableValuesAsString);
      } else
        Parser.Errors.AddError(this, "Attempted to upgrade '{0}' to Enumerated Type {1}. Expected a String, but got data type '{2}' instead.",
          Value, enumType.Name, Value.GetType().Name);
    }
  }
}
