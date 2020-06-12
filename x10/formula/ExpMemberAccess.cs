using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpMemberAccess : ExpBase {
    public ExpBase Expression { get; set; }
    public string MemberName { get; set; }

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      ExpDataType expressionDataType = Expression.DetermineType(errors, context, rootType);
      return GetMemberAccessDataType(this, errors, expressionDataType, MemberName);
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

