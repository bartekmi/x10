using System;
using x10.model.metadata;

namespace x10.formula {
  public class ExpLiteral : ExpBase {
    public object Value { get; set; }

    public ExpLiteral(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitLiteral(this);
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      if (Value is string)
        return new X10DataType(DataTypes.Singleton.String);
      if (Value is int)
        return new X10DataType(DataTypes.Singleton.Integer);
      if (Value is double)
        return new X10DataType(DataTypes.Singleton.Float);
      if (Value is bool)
        return new X10DataType(DataTypes.Singleton.Boolean);
      if (Value is int)
        return new X10DataType(DataTypes.Singleton.Integer);
      if (Value == null)
        return X10DataType.NULL;

      Parser.Errors.AddError(this, "Unexpected Data Type: " + Value.GetType().Name);
      return X10DataType.ERROR;
    }

    internal void UpgradeToEnum(DataTypeEnum enumType) {
      IsEnumLiteral = true;
      DataType = new X10DataType(enumType);

      if (Value is string) {
        EnumValue inferredEnumValue = enumType.FindEnumValue(Value);
        if (inferredEnumValue == null)
          Parser.Errors.AddErrorDidYouMean(this, Value.ToString(), enumType.AvailableValuesAsStrings, "Attempted to upgrade '{0}' to Enumerated Type {1}. No such value exists.",
            Value, enumType.Name);
      } else if (Value == null) { 
        // An enum field could legitimately be null
      } else
        Parser.Errors.AddError(this, "Attempted to upgrade '{0}' to Enumerated Type {1}. Expected a String, but got data type '{2}' instead.",
          Value, enumType.Name, Value.GetType().Name);
    }
  }
}
