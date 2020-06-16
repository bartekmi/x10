﻿using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpIdentifier : ExpBase {
    public string Name { get; set; }

    public ExpIdentifier(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override ExpDataType DetermineType(ExpDataType rootType) {
      if (Name == FormulaParser.CONTEXT_NAME) {
        Entity context = Parser.AllEntities.FindContextEntityWithError(this);
        if (context == null)
          return ExpDataType.ERROR;
        return new ExpDataType(context);
      }

      if (rootType == null) {
        Parser.Errors.AddError(this, "In this context, there is no Entity to access");
        return ExpDataType.ERROR;
      }

      DataTypeEnum enumName = Parser.AllEnums.FindEnumErrorIfMultiple(Name, this);
      if (enumName != null)
        return ExpDataType.CreateEnumNameTemporaryType(enumName);

      return ExpMemberAccess.GetMemberAccessDataType(this, Parser.Errors, rootType, Name);
    }
  }
}