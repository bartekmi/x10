using System;

using x10.model.definition;
using x10.model.metadata;

namespace x10.formula {

  public class ExpDataType {
    public DataType DataType { get; set; }
    public Entity Entity { get; set; }
    public DataTypeEnum EnumName { get; set; }  // Used for enum name (left side of EnumNam.Value)

    // Derived
    public bool IsEntity { get { return Entity != null; } }
    public bool IsPrimitive { get { return DataType != null; } }
    public bool IsEnumName { get { return EnumName != null; } }

    public bool IsNull { get { return this == NULL; } }
    public bool IsError { get { return this == ERROR; } }

    public bool IsNumeric { get { return DataType == DataTypes.Singleton.Integer || DataType == DataTypes.Singleton.Float; } }
    public bool IsFloat { get { return DataType == DataTypes.Singleton.Float; } }
    public bool IsInteger { get { return DataType == DataTypes.Singleton.Integer; } }
    public bool IsString { get { return DataType == DataTypes.Singleton.String; } }
    public bool IsBoolean { get { return DataType == DataTypes.Singleton.Boolean; } }
    public bool IsComparable { get { return IsPrimitive && !IsBoolean; } } // Can participate in >, <, >=, <= comparisons

    public static ExpDataType NULL = new ExpDataType();
    public static ExpDataType ERROR = new ExpDataType();
    public static ExpDataType Integer = new ExpDataType(DataTypes.Singleton.Integer);
    public static ExpDataType Float = new ExpDataType(DataTypes.Singleton.Float);
    public static ExpDataType String = new ExpDataType(DataTypes.Singleton.String);
    public static ExpDataType Boolean = new ExpDataType(DataTypes.Singleton.Boolean);

    private ExpDataType() { }

    internal ExpDataType(DataType dataType) {
      if (dataType == null)
        throw new Exception("dataType is null");
      DataType = dataType;
    }

    internal ExpDataType(Entity entity) {
      if (entity == null)
        throw new Exception("entity is null");
      Entity = entity;
    }

    internal static ExpDataType CreateEnumNameTemporaryType(DataTypeEnum enumName) {
      if (enumName == null)
        throw new Exception("enumName is null");
      return new ExpDataType() {
        EnumName = enumName,
      };
    }

    internal ExpDataType(Member member) {
      if (member is X10Attribute attr)
        DataType = attr.DataType;
      else if (member is Association assoc)
        Entity = assoc.ReferencedEntity;
      else
        throw new Exception("Added a new Member type and forgot to update this code?");
    }

    public override string ToString() {
      if (IsEntity)
        return Entity.Name;
      if (IsPrimitive)
        return DataType.Name;
      if (IsEnumName)
        return EnumName.Name;
      if (IsError)
        return "Error";
      if (IsNull)
        return "Null";

      throw new Exception("Added new type and forgot to update code");
    }

    public override bool Equals(object obj) {
      if (obj is ExpDataType other) {
        if (IsEntity && other.IsEntity)
          return Entity == other.Entity;
        if (IsPrimitive && other.IsPrimitive)
          return DataType == other.DataType;
        if (IsEnumName && other.IsEnumName)
          return EnumName == other.EnumName;
        if (IsNull && other.IsNull)
          return true;
        if (IsError && other.IsError)
          return true;
      }
      return false;
    }

    // Not used, but want to avoid warning, and, technically, it should be defined
    public override int GetHashCode() {
      if (IsEntity)
        return Entity.GetHashCode();
      if (IsPrimitive)
        return DataType.GetHashCode();
      if (IsEnumName)
        return EnumName.GetHashCode();
      if (IsNull || IsError)
        return GetHashCode();
      throw new Exception("Should never be here");
    }
  }
}
