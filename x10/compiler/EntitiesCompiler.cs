using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EntitiesCompiler {

    public MessageBucket Messages { get; private set; }

    public List<Entity> Compile(string dirPath) {
      Messages = new MessageBucket();
      Parser parser = new ParserYaml();
      List<TreeNode> files = parser.RecursivelyParseDirectory(dirPath);

      List<Entity> entities = new List<Entity>();

      foreach (TreeNode file in files) {
        Entity entity = CompileEntity(file);
        if (entity != null)
          entities.Add(entity);
      }

      return entities;
    }

    private void ReadAttributes(TreeNode node, AppliesTo type, IAcceptsModelAttributeValues modelComponent) {
      TreeHash hash = node as TreeHash;
      if (hash == null) {
        AddError(node, "Expected a Hash type node, but was: " + node.GetType().Name);
        return;
      }

      // Iterate known attributes and extract
      // NOTE: Attributes must be checked in order. Don't convert this to a hash.
      // Reason: 'dataType' must be parsed before the 'default' attribute
      foreach (ModelAttributeDefinition attrDef in ModelAttributeDefinitions.All) {
        if (!attrDef.AppliesToType(type))
          continue;

        ReadAttribute(hash, type, modelComponent, attrDef);
      }

      // Iterate all attributes and error if unknown
      HashSet<string> validAttributeNames =
        new HashSet<string>(ModelAttributeDefinitions.All.Where(x => x.AppliesToType(type))
          .Select(x => x.Name));

      foreach (TreeAttribute attribute in hash.Attributes)
        if (!validAttributeNames.Contains(attribute.Key))
          AddError(attribute,
            string.Format("Unknown attribute '{0}' on {1}", attribute.Key, type));
    }

    private void ReadAttribute(TreeHash hash, AppliesTo type, IAcceptsModelAttributeValues modelComponent, ModelAttributeDefinition attrDef) {
      // Error if mandatory attribute missing
      TreeNode attrNode = hash.FindNode(attrDef.Name);
      if (attrNode == null) {
        if (attrDef.ErrorSeverityIfMissing != null)
          AddMessage(attrDef.ErrorSeverityIfMissing.Value, attrNode,
            string.Format("The attribute '{0}' is missing from {1}", attrDef.Name, type));
        return;
      }

      // Ensure that the value of the attribute is a scalar (not a list, etc)
      TreeScalar scalarNode = attrNode as TreeScalar;
      if (scalarNode == null) {
        AddError(scalarNode, string.Format("The attribute '{0}' should be simple string of the correct type, but is a {1}",
          attrDef.Name, attrNode.GetType().Name));
        return;
      }

      // There is a special case when the data type is not fixed, but must match
      // the data type of the X10Attribute
      DataType dataType = attrDef.DataType == DataTypes.SameAsDataType ?
        ((X10Attribute)modelComponent).DataType :
        attrDef.DataType;
      if (dataType == null)
        return;

      // Attempt to parse the string attribute value according to its data type
      object typedValue = attrDef.DataType.Parse(scalarNode.Value.ToString());
      if (typedValue == null) {
        AddError(scalarNode, string.Format("For attribute '{0}', could not parse a(n) {1} from '{2}'. Examples of valid data of this type: {3}",
          attrDef.Name, attrDef.DataType.Name, scalarNode.Value, attrDef.DataType.Examples));
        return;
      }

      // If a setter has been provided, use it; 
      // Otherwise, strore the attribute value in a ModelAttributeValue instance
      if (attrDef.Setter != null) {
        Type modelComponentType = modelComponent.GetType();
        PropertyInfo info = modelComponentType.GetProperty(attrDef.Setter);
        if (info == null) {
          throw new Exception(string.Format("Setter property '{0}' on Model Attribute Definition '{1}' does not exist on type {2}",
            attrDef.Setter, attrDef.Name, modelComponentType.Name));
        }
        info.SetValue(modelComponent, typedValue);
      } else {
        modelComponent.AttributeValues.Add(new ModelAttributeValue() {
          Value = typedValue,
          Definition = attrDef,
        });
      }

      // Do validation, if requried
      if (attrDef.ValidationFunction != null)
        attrDef.ValidationFunction(Messages, scalarNode, modelComponent, type);
    }

    private Entity CompileEntity(TreeNode rootNodeUntyped) {
      TreeHash rootNode = rootNodeUntyped as TreeHash;
      if (rootNodeUntyped == null) {
        AddError(rootNodeUntyped, "The root node of an entity must be a Hash, but was: " + rootNodeUntyped.GetType().Name);
        return null;
      }

      Entity entity = new Entity();

      // Read top-level (entity) attributes
      ReadAttributes(rootNode, AppliesTo.Entity, entity);

      // Read Entity Attributes
      TreeSequence attributes = TreeUtils.GetOptional<TreeSequence>(rootNode, "attributes", Messages);
      if (attributes != null) {
        foreach (TreeNode attribute in attributes.Children) {
          X10Attribute x10Attribute = new X10Attribute();
          entity.Members.Add(x10Attribute);
          ReadAttributes(attribute, AppliesTo.Attribute, x10Attribute);
        }
      }

      // Read Associations
      TreeSequence associations = TreeUtils.GetOptional<TreeSequence>(rootNode, "associations", Messages);
      if (associations != null) {
        foreach (TreeNode attribute in associations.Children) {
          Association association = new Association();
          entity.Members.Add(association);
          ReadAttributes(attribute, AppliesTo.Association, association);
        }
      }

      // Read Enums
      TreeSequence enums = TreeUtils.GetOptional<TreeSequence>(rootNode, "enums", Messages);
      if (enums != null) {
        foreach (TreeNode treeNode in enums.Children) {
          CompileEntity(treeNode);
        }
      }

      return entity;
    }

    private void CompileEnum(TreeNode enumRootNode) {
      DataType theEnum = new DataType();
      DataTypes.AddModelEnum(theEnum);
      ReadAttributes(enumRootNode, AppliesTo.EnumValue, theEnum);

      TreeHash enumHash = enumRootNode as TreeHash;
      if (enumHash == null)
        return;

      TreeSequence enumValues = TreeUtils.GetOptional<TreeSequence>(enumHash, "values", Messages);
      if (enumValues == null) {
        AddError(enumHash, "Mandatory enum property 'values' missing");
        return;
      }

      foreach (TreeNode enumValueNode in enumValues.Children) {
        EnumValue enumValue = new EnumValue();
        theEnum.EnumValues.Add(enumValue);
        ReadAttributes(enumValueNode, AppliesTo.EnumValue, enumValue);
      }
    }

    #region Utilities

    private void AddError(TreeElement element, string message) {
      AddMessage(CompileMessageSeverity.Error, element, message);
    }

    private void AddMessage(CompileMessageSeverity severity, TreeElement element, string message) {
      Messages.AddMessage(severity, element, message);
    }
    #endregion
  }
}
