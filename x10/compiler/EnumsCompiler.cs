﻿using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EnumsCompiler {
    private readonly MessageBucket _messages;
    private readonly AttributeReader _attrReader;

    internal EnumsCompiler(MessageBucket messages, AttributeReader attrReader) {
      _messages = messages;
      _attrReader = attrReader;
    }

    internal void CompileEnumFile(TreeNode rootNodeUntyped) {
      TreeSequence rootNode = rootNodeUntyped as TreeSequence;
      if (rootNode == null) {
        _messages.AddError(rootNodeUntyped, "The root node of an Enums file must be an Array, but was: " + rootNodeUntyped.GetType().Name);
        return;
      }

      foreach (TreeNode enumRootNode in rootNode.Children)
        CompileEnum(enumRootNode);
    }

    internal void CompileEnum(TreeNode enumRootNode) {
      TreeHash enumHash = AttributeReader.EnsureObjectWithAttributresIsHash(enumRootNode, _messages);
      if (enumHash == null)
        return;

      DataType theEnum = new DataType();
      DataTypes.Singleton.AddModelEnum(theEnum);

      // Extract the enum values
      TreeNode enumValues = TreeUtils.GetMandatoryAttribute(enumHash, "values", _messages);
      if (enumValues != null) {
        if (enumValues is TreeSequence sequence)
          foreach (TreeNode enumValueNode in sequence.Children) {
            EnumValue compositeEnumValue = new EnumValue();
            theEnum.EnumValues.Add(compositeEnumValue);
            _attrReader.ReadAttributes(enumValueNode, AppliesTo.EnumValue, compositeEnumValue);
          }
        else if (enumValues is TreeScalar scalar) {
          string[] enumValuesArray = scalar.Value.ToString().Split(',');
          foreach (string enumValue in enumValuesArray) {
            EnumValue simpleEnumValue = new EnumValue() {
              Value = enumValue.Trim(),
            };
            theEnum.EnumValues.Add(simpleEnumValue);
            // TODO: Validate the enum values names
          }
        }

        // Check uniqueness of enum value names
        UniquenessChecker.Check("value",
          theEnum.EnumValues,
          _messages,
          "The value '{0}' is not unique among all the values of this Enum.");
      }

      // This must go after extracting values because the default value is checked
      // for validity using the actual values
      _attrReader.ReadAttributes(enumRootNode, AppliesTo.EnumType, theEnum, "values");
    }
  }
}