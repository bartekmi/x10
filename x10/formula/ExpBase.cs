using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {

  public class ExpDataType {
    public DataType DataType { get; set; }
    public Entity Entity { get; set; }

    // Derived
    public bool IsEntity { get { return Entity != null; } }
    public bool IsPrimitive { get { return DataType != null; } }

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
      DataType = dataType;
    }

    internal ExpDataType(Entity entity) {
      Entity = entity;
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
        return "Entity " + Entity.Name;
      if (IsPrimitive)
        return "DataType " + DataType.Name;
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
        if (IsNull && other.IsNull)
          return true;
        if (IsError && other.IsError)
          return true;
      }
      return false;
    }
  }

  public abstract class ExpBase : IParseElement {
    // IParseElement
    public FileInfo FileInfo { get; private set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }
    public void SetFileInfo(FileInfo fileInfo) {
      FileInfo = fileInfo;
    }

    public abstract ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType);
  }
}
