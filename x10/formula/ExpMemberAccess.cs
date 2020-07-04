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

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      X10DataType enumType = DetermineIfEnum();
      if (enumType != null)
        return enumType;

      X10DataType expressionDataType = Expression.DetermineType(rootType);
      return GetMemberAccessDataType(this, Parser.Errors, expressionDataType, MemberName);
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

    internal static X10DataType GetMemberAccessDataType(ExpBase expression, MessageBucket errors, X10DataType type, string memberName) {
      if (type.IsError)
        return X10DataType.ERROR;

      if (type.IsPrimitive) {
        DataTypeProperty property = type.DataType.FindProperty(memberName);
        if (property == null) {
          errors.AddError(expression, "Data Type '{0}' does not contain property '{1}'", type.DataType, memberName);
          return X10DataType.ERROR;
        }
        return new X10DataType(property.Type);
      } else if (type.IsEntity) {
        if (type.IsMany)
          return GetIsManyDataType(expression, errors, type, memberName);

        Entity entity = type.Entity;
        Member member = entity.FindMemberByName(memberName);
        if (member == null) {
          errors.AddError(expression, "Entity '{0}' does not contain an Attribute or Association '{1}'", entity.Name, memberName);
          return X10DataType.ERROR;
        }

        return new X10DataType(member);
      } else {
        errors.AddError(expression, "Unexpected context - neither Entity nor Primitive Data Type", type);
        return X10DataType.ERROR;
      }
    }

    private static X10DataType GetIsManyDataType(ExpBase expression, MessageBucket errors, X10DataType type, string memberName) {
      switch (memberName) {
        case "count":
          return X10DataType.Integer;
        case "first":
        case "last":
          return type.Clone(false);
        default:
          errors.AddError(expression, "{0} is not a valid property of a collection. The only valid properties are: count, first, last", memberName);
          return X10DataType.ERROR;
      }
    }
  }
}

