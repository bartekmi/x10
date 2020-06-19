using System;
using System.Collections.Generic;
using System.Text;
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

    public override ExpDataType DetermineType(ExpDataType rootType) {
      ExpDataType expressionDataType = Expression.DetermineType(rootType);
      if (expressionDataType.IsEnumName)
        return GetEnumValue(expressionDataType.EnumName);
      return GetMemberAccessDataType(this, Parser.Errors, expressionDataType, MemberName);
    }

    private ExpDataType GetEnumValue(DataTypeEnum enumType) {
      if (enumType.HasEnumValue(MemberName))
        return new ExpDataType(enumType);

      Parser.Errors.AddError(this, "Enum '{0}' does not have value '{1}'", enumType.Name, MemberName);
      return ExpDataType.ERROR;
    }

    internal static ExpDataType GetMemberAccessDataType(ExpBase expression, MessageBucket errors, ExpDataType type, string memberName) {
      if (type.IsError)
        return ExpDataType.ERROR;

      // TODO: In the future, we may allow member access on DataTypes - e.g. DateTime could have 'Day', etc
      if (!type.IsEntity) {
        errors.AddError(expression, "The context here is NOT an Entity. It is {0}", type);
        return ExpDataType.ERROR;
      }

      Entity entity = type.Entity;
      Member member = entity.FindMemberByName(memberName);
      if (member == null) {
        errors.AddError(expression, "Entity '{0}' does not contain an Attribute or Association '{1}'", entity.Name, memberName);
        return ExpDataType.ERROR;
      }

      return new ExpDataType(member);
    }
  }
}

