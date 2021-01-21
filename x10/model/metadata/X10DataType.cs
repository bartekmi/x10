using System;

using x10.model.definition;

namespace x10.model.metadata {

  public class X10DataType {
    public DataType DataType { get; private set; }  // Either this one...
    public Entity Entity { get; private set; }      // ...or this one must be set
    public bool IsMany { get; private set; }

    // May or may not be present. X10DataType stands on its own with just the above
    // properties, but "Member" can provide more context on where the data type
    // came from. 
    public Member Member { get; private set; }

    // Derived
    public bool IsEntity { get { return !IsPrimitive && Entity != null; } }
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

    public static X10DataType NULL = new X10DataType();
    public static X10DataType ERROR = new X10DataType();
    public static X10DataType Integer = new X10DataType(DataTypes.Singleton.Integer);
    public static X10DataType Float = new X10DataType(DataTypes.Singleton.Float);
    public static X10DataType String = new X10DataType(DataTypes.Singleton.String);
    public static X10DataType Boolean = new X10DataType(DataTypes.Singleton.Boolean);

    private X10DataType() { }

    internal X10DataType(DataType dataType) {
      if (dataType == null)
        throw new Exception("dataType is null");
      DataType = dataType;
    }

    internal X10DataType(Entity entity, bool isMany) {
      if (entity == null)
        throw new Exception("entity is null");

      Entity = entity;
      IsMany = isMany;
    }

    internal X10DataType(Member member) {
      if (member is X10Attribute attr) {
        Entity = member.Owner;
        DataType = attr.DataType;
      } else if (member is Association assoc) {
        Entity = assoc.ReferencedEntity;
        IsMany = assoc.IsMany;
      } else
        throw new Exception("Added a new Member type and forgot to update this code?");

      Member = member;
    }

    internal X10DataType Clone(bool isMany) {
      return new X10DataType() {
        IsMany = isMany,
        DataType = DataType,
        Entity = Entity,
        Member = Member,
      };
    }

    internal X10DataType ReduceManyToOne() {
      return Clone(false);
    }


    public override string ToString() {
      if (IsPrimitive)
        return DataType.Name;
      if (IsEntity)
        return IsMany ? string.Format("Many<{0}>", Entity.Name) : Entity.Name;
      if (IsError)
        return "Error";
      if (IsNull)
        return "Null";

      return "Blank";
    }

    public override bool Equals(object obj) {
      if (obj is X10DataType other) {
        if (IsPrimitive && other.IsPrimitive)
          return DataType == other.DataType;
        if (IsEntity && other.IsEntity)
          return Entity == other.Entity && IsMany == other.IsMany;
        if (IsNull && other.IsNull)
          return true;
        if (IsError && other.IsError)
          return true;
      }
      return false;
    }

    // Not used, but want to avoid warning, and, technically, it should be defined
    public override int GetHashCode() {
      if (IsPrimitive)
        return DataType.GetHashCode();
      if (IsEntity)
        return Entity.GetHashCode();
      if (IsNull || IsError)
        return GetHashCode();
      throw new Exception("Should never be here");
    }
  }
}
