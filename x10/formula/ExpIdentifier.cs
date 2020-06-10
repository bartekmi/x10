using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class ExpIdentifier : ExpBase {
    public string Name { get; set; }

    public override ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType) {
      if (Name == FormulaParser.CONTEXT_NAME)
        return new ExpDataType(context);

      if (rootType == null) {
        errors.AddError(this, "In this context, there is no Entity to access");
        return ExpDataType.ERROR;
      }

      return ExpMemberAccess.GetMemberAccessDataType(this, errors, rootType, Name);
    }
  }
}
