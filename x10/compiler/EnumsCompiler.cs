using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EnumsCompiler {
    public MessageBucket Messages { get; private set; }
    private AttributeReader _attrReader;

    internal EnumsCompiler(MessageBucket messages, AttributeReader attrReader) {
      Messages = messages;
      _attrReader = attrReader;
    }

    internal void CompileEnumFile(TreeNode rootNode) {

    }

    internal void CompileEnum(TreeNode enumRootNode) {
      DataType theEnum = new DataType();
      DataTypes.Singleton.AddModelEnum(theEnum);
      _attrReader.ReadAttributes(enumRootNode, AppliesTo.EnumType, theEnum, "values");

      TreeHash enumHash = enumRootNode as TreeHash;
      if (enumHash == null)
        return;

      TreeSequence enumValues = TreeUtils.GetOptional<TreeSequence>(enumHash, "values", Messages);
      if (enumValues == null) {
        Messages.AddError(enumHash, "Mandatory enum property 'values' missing");
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
        Messages,
        "The value '{0}' is not unique among all the values of this Enum.");
    }
  }
}
