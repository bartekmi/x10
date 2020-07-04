using System.Collections.Generic;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpMemberAccess : ExpBase {
    public ExpBase Expression { get; set; }
    public string MemberName { get; set; }

    public ExpMemberAccess(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitMemberAccess(this);
    }

    public override ExpIdentifier FirstMemberOfPath() { 
      return Expression.FirstMemberOfPath(); 
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      X10DataType enumType = DetermineIfEnum();
      if (enumType != null)
        return enumType;

      X10DataType expressionDataType = Expression.DetermineType(rootType);
      return GetMemberAccessDataType(Parser.Errors, expressionDataType, MemberName);
    }

    // Check for "MyEnum.myEnumValue"
    private X10DataType DetermineIfEnum() {
      if (Expression is ExpIdentifier expIdent) {
        DataTypeEnum enumType = Parser.AllEnums.FindEnumErrorIfMultiple(expIdent.Name, this);
        if (enumType != null) {
          if (enumType.HasEnumValue(MemberName))
            return new X10DataType(enumType);

          Parser.Errors.AddError(this, "Enum '{0}' does not have value '{1}'", enumType.Name, MemberName);
          return X10DataType.ERROR;
        }
      }

      return null;
    }

    internal X10DataType GetMemberAccessDataType(MessageBucket errors, X10DataType type, string memberName) {
      X10DataType returnType = GetMemberAccessDataTypeStatic(type, memberName);
      if (returnType != null)
        return returnType;

      if (type.IsPrimitive)
        errors.AddError(this, "Data Type '{0}' does not contain property '{1}'", type.DataType, memberName);
      else if (type.IsEntity)
        if (type.IsMany)
          errors.AddError(this, "{0} is not a valid property of a collection. The only valid properties are: count, first, last", memberName);
        else
          errors.AddError(this, "Entity '{0}' does not contain an Attribute or Association '{1}'", type.Entity.Name, memberName);
      
      return X10DataType.ERROR;
    }

    internal static X10DataType GetMemberAccessDataTypeStatic(X10DataType type, string memberName) {
      if (type.IsPrimitive) {
        DataTypeProperty property = type.DataType.FindProperty(memberName);
        if (property != null)
          return new X10DataType(property.Type);
      } else if (type.IsEntity) {
        if (type.IsMany)
          return GetIsManyDataType(type, memberName);

        Entity entity = type.Entity;
        Member member = entity.FindMemberByName(memberName);
        if (member != null)
          return new X10DataType(member);
      }

      return null;
    }

    private static X10DataType GetIsManyDataType(X10DataType type, string memberName) {
      switch (memberName) {
        case "count":
          return X10DataType.Integer;
        case "first":
        case "last":
          return type.Clone(false);
        default:
          return null;
      }
    }
  }
}

