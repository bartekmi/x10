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
      _attrReader.ReadAttributes(rootNode, AppliesTo.Entity, entity, "attributes", "derivedAttributes", "associations", "validations", "enums");

      ReadRegularAttributes(rootNode, entity);
      ReadDerivedAttributes(rootNode, entity);
      ReadAssociations(rootNode, entity);
      ReadEnums(rootNode);
      ReadValidations(rootNode, entity);

      return entity;
    }

    private void ReadRegularAttributes(TreeHash rootNode, Entity entity) {
      TreeSequence attributes = TreeUtils.GetOptional<TreeSequence>(rootNode, "attributes", _messages);
      if (attributes != null) {
        foreach (TreeNode attribute in attributes.Children) {
          X10Attribute x10Attribute = new X10RegularAttribute();
          x10Attribute.TreeElement = attribute;
          entity.AddMember(x10Attribute);
          _attrReader.ReadAttributes(attribute, AppliesTo.RegularAttribute, x10Attribute);
        }
      }
    }

    private void ReadDerivedAttributes(TreeHash rootNode, Entity entity) {
      TreeSequence derivedAttributes = TreeUtils.GetOptional<TreeSequence>(rootNode, "derivedAttributes", _messages);
      if (derivedAttributes != null) {
        foreach (TreeNode attribute in derivedAttributes.Children) {
          X10Attribute x10Attribute = new X10DerivedAttribute();
          x10Attribute.TreeElement = attribute;
          entity.AddMember(x10Attribute);
          _attrReader.ReadAttributes(attribute, AppliesTo.DerivedAttribute, x10Attribute);
        }
      }
    }

    private void ReadAssociations(TreeHash rootNode, Entity entity) {
      TreeSequence associations = TreeUtils.GetOptional<TreeSequence>(rootNode, "associations", _messages);
      if (associations != null) {
        foreach (TreeNode association in associations.Children) {
          Association x10Association = new Association();
          x10Association.TreeElement = association;
          entity.AddMember(x10Association);
          _attrReader.ReadAttributes(association, AppliesTo.Association, x10Association);
        }
      }
    }

    private void ReadEnums(TreeHash rootNode) {
      TreeSequence enums = TreeUtils.GetOptional<TreeSequence>(rootNode, "enums", _messages);
      if (enums != null)
        foreach (TreeNode enumRootNode in enums.Children)
          _enumsCompiler.CompileEnum(enumRootNode, false);
    }

    private void ReadValidations(TreeHash rootNode, Entity entity) {
      TreeSequence validations = TreeUtils.GetOptional<TreeSequence>(rootNode, "validations", _messages);
      if (validations != null) {
        foreach (TreeNode validation in validations.Children) {
          Validation x10Validation = new Validation();
          x10Validation.TreeElement = validation;
          entity.AddValidation(x10Validation);
          _attrReader.ReadAttributes(validation, AppliesTo.Validation, x10Validation);
        }
      }
    }
  }
}
