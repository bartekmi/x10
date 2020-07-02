﻿using System;

using x10.model.definition;
using x10.model.metadata;

namespace x10.formula {

  public class ExpDataType {
    public DataType DataType { get; private set; }
    public Entity Entity { get; private set; }
    public bool IsMany { get; private set; }

    // Derived
    public bool IsEntity { get { return Entity != null; } }
    public bool IsPrimitive { get { return DataType != null; } }
    public bool IsEnum { get { return DataType is DataTypeEnum; } }

    public bool IsNull { get { return this == NULL; } }
    public bool IsError { get { return this == ERROR; } }

    public bool IsNumeric { get { return DataType == DataTypes.Singleton.Integer || DataType == DataTypes.Singleton.Float; } }
    public bool IsFloat { get { return DataType == DataTypes.Singleton.Float; } }
    public bool IsInteger { get { return DataType == DataTypes.Singleton.Integer; } }
    public bool IsString { get { return DataType == DataTypes.Singleton.String; } }
    public bool IsBoolean { get { return DataType == DataTypes.Singleton.Boolean; } }
    public bool IsComparable { get { return IsPrimitive && !IsBoolean; } } // Can participate in >, <, >=, <= comparisons

    public DataTypeEnum DataTypeAsEnum { get { return DataType as DataTypeEnum; } }

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

    internal ExpDataType(Entity entity, bool isMany) {
      if (entity == null)
        throw new Exception("entity is null");

      Entity = entity;
      IsMany = isMany;
    }

    internal ExpDataType(Member member) {
      if (member is X10Attribute attr)
        DataType = attr.DataType;
      else if (member is Association assoc) {
        Entity = assoc.ReferencedEntity;
        IsMany = assoc.IsMany;
      } else
        throw new Exception("Added a new Member type and forgot to update this code?");
    }

    internal ExpDataType Clone(bool isMany) {
      return new ExpDataType() {
        IsMany = isMany,
        DataType = DataType,
        Entity = Entity,
      };
    }

    public override string ToString() {
      if (IsEntity)
        return IsMany ? string.Format("Many<{0}>", Entity.Name) : Entity.Name;
      if (IsPrimitive)
        return DataType.Name;
      if (IsError)
        return "Error";
      if (IsNull)
        return "Null";

      throw new Exception("Added new type and forgot to update code");
    }

    public override bool Equals(object obj) {
      if (obj is ExpDataType other) {
        if (IsEntity && other.IsEntity)
          return Entity == other.Entity && IsMany == other.IsMany;
        if (IsPrimitive && other.IsPrimitive)
          return DataType == other.DataType;
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
      if (IsNull || IsError)
        return GetHashCode();
      throw new Exception("Should never be here");
    }
  }
}
