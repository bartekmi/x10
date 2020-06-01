using System;
using System.Collections.Generic;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EntityCompilerPass1 {

    private readonly MessageBucket _messages;
    private readonly EnumsCompiler _enumsCompiler;
    private readonly AttributeReader _attrReader;

    internal EntityCompilerPass1(MessageBucket messages, 
      EnumsCompiler enumsCompiler,
      AttributeReader attributeReader) {

      _messages = messages;
      _enumsCompiler = enumsCompiler;
      _attrReader = attributeReader;
    }

    internal Entity CompileEntity(TreeNode rootNodeUntyped) {   
      TreeHash rootNode = rootNodeUntyped as TreeHash;
      if (rootNode == null) {
        _messages.AddError(rootNodeUntyped, "The root node of an Entity file must be a Hash, but was: " + rootNodeUntyped.GetType().Name);
        return null;
      }

      Entity entity = new Entity() {
        TreeElement = rootNode
      };

      // Read top-level (entity) attributes
      _attrReader.ReadAttributes(rootNode, AppliesTo.Entity, entity, "attributes", "associations", "enums", "derivedAttributes");

      // Read Entity Attributes
      TreeSequence attributes = TreeUtils.GetOptional<TreeSequence>(rootNode, "attributes", _messages);
      if (attributes != null) {
        foreach (TreeNode attribute in attributes.Children) {
          X10Attribute x10Attribute = new X10RegularAttribute();
          x10Attribute.TreeElement = attribute;
          entity.AddMembers(x10Attribute);
          _attrReader.ReadAttributes(attribute, AppliesTo.Attribute, x10Attribute);
        }
      }

      // Read Entity Derived Attributes
      TreeSequence derivedAttributes = TreeUtils.GetOptional<TreeSequence>(rootNode, "derivedAttributes", _messages);
      if (derivedAttributes != null) {
        foreach (TreeNode attribute in derivedAttributes.Children) {
          X10Attribute x10Attribute = new X10DerivedAttribute();
          x10Attribute.TreeElement = attribute;
          entity.AddMembers(x10Attribute);
          _attrReader.ReadAttributes(attribute, AppliesTo.DerivedAttribute, x10Attribute);
        }
      }

      // Read Associations
      TreeSequence associations = TreeUtils.GetOptional<TreeSequence>(rootNode, "associations", _messages);
      if (associations != null) {
        foreach (TreeNode attribute in associations.Children) {
          Association association = new Association();
          association.TreeElement = attribute;
          entity.AddMembers(association);
          _attrReader.ReadAttributes(attribute, AppliesTo.Association, association);
        }
      }

      // Read Enums
      TreeSequence enums = TreeUtils.GetOptional<TreeSequence>(rootNode, "enums", _messages);
      if (enums != null)
        foreach (TreeNode enumRootNode in enums.Children)
          _enumsCompiler.CompileEnum(enumRootNode);

      return entity;
    }
  }
}
