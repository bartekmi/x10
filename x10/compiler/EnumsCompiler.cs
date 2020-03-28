using System;
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
      DataType theEnum = new DataType();
      if (!_attrReader.ReadAttributes(enumRootNode, AppliesTo.EnumType, theEnum, "values"))
        return;

      DataTypes.Singleton.AddModelEnum(theEnum);

      TreeHash enumHash = (TreeHash)enumRootNode;
      TreeSequence enumValues = TreeUtils.GetOptional<TreeSequence>(enumHash, "values", _messages);
      if (enumValues == null) {
        _messages.AddError(enumHash, "Mandatory enum property 'values' missing");
        return;
      }

      foreach (TreeNode enumValueNode in enumValues.Children) {
        EnumValue enumValue = new EnumValue();
        theEnum.EnumValues.Add(enumValue);
        _attrReader.ReadAttributes(enumValueNode, AppliesTo.EnumValue, enumValue);
      }

      // Check uniqueness of enum value names
      UniquenessChecker.Check("value",
        theEnum.EnumValues,
        _messages,
        "The value '{0}' is not unique among all the values of this Enum.");
    }
  }
}
