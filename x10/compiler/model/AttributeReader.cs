using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  internal class AttributeReader {

    private readonly MessageBucket _messages;

    internal AttributeReader(MessageBucket messages) {
      _messages = messages;
    }

    internal static TreeHash EnsureObjectWithAttributresIsHash(TreeNode node, MessageBucket messages) {
      if (node is TreeHash hash)
        return hash;

      messages.AddError(node, "Expected a Hash type node, but was: " + node.GetType().Name);
      return null;
    }

    // Read the attributes of this node (which should be a hash)
    // Returns true if it was a hash node, false otherwise
    internal bool ReadAttributes(TreeNode node, AppliesTo type, IAcceptsModelAttributeValues modelComponent, params string[] ignoreAttributes) {
      TreeHash hash = EnsureObjectWithAttributresIsHash(node, _messages);
      if (hash == null)
        return false;

      // Iterate known attributes and extract
      // NOTE: Attributes must be checked in order. Don't convert this to a hash.
      // Reason: 'dataType' must be parsed before the 'default' attribute
      foreach (ModelAttributeDefinition attrDef in ModelAttributeDefinitions.All)
        if (attrDef.AppliesToType(type))
          ReadAttribute(hash, type, modelComponent, attrDef);

      ErrorOnUnknownAttributes(hash, type, ignoreAttributes);
      return true;
    }

    private void ReadAttribute(TreeHash hash, AppliesTo type, IAcceptsModelAttributeValues modelComponent, ModelAttributeDefinition attrDef) {
      // Error if mandatory attribute missing
      TreeNode attrNode = hash.FindNode(attrDef.Name);
      if (attrNode == null) {
        if (attrDef.ErrorSeverityIfMissing != null)
          _messages.AddMessage(attrDef.ErrorSeverityIfMissing.Value, hash,
            string.Format("The attribute '{0}' is missing from {1}", attrDef.Name, type));
        return;
      }

      object typedValue;

      if (attrDef is ModelAttributeDefinitionAtomic attrDefAtomic) {
        // Ensure that the value of the attribute is a scalar (not a list, etc)
        TreeScalar scalarNode = attrNode as TreeScalar;
        if (scalarNode == null) {
          _messages.AddError(attrNode, string.Format("The attribute '{0}' should be a simple string of the correct type, but is a {1}",
            attrDef.Name, attrNode.GetType().Name));
          return;
        }

        // Attempt to parse the string attribute value according to its data type
        string value = scalarNode.Value.ToString();
        if (attrDefAtomic.DataTypeMustBeSameAsAttribute)
          typedValue = value;
        else {
          DataType dataType = attrDefAtomic.DataType;
          typedValue = dataType.Parse(value, _messages, scalarNode, attrDef.Name);
          if (typedValue == null)
            return;
        }

        // Do validation, if requried
        attrDefAtomic.ValidationFunction?.Invoke(_messages, scalarNode, modelComponent, type);
      } else if (attrDef is ModelAttributeDefinitionComplex attrDefComplex) {
        typedValue = attrDefComplex.ParseFunction.Invoke(_messages, attrNode);
      } else
        throw new Exception("Unexpected type: " + attrDef.GetType().Name);

      // If a setter has been provided, use it; 
      SetValueViaSetter(attrDef, modelComponent, typedValue);

      // A ModelAttributeValue is always stored, even if a setter exists. For one thing,
      // this is the only way we can track where the attribute came from in the code.
      modelComponent.AttributeValues.Add(new ModelAttributeValue(attrNode) {
        Value = typedValue,
        Definition = attrDef,
      });
    }

    internal static void SetValueViaSetter(ModelAttributeDefinition attrDef, IAcceptsModelAttributeValues modelComponent, object typedValue) {
      if (attrDef.Setter != null) {
        Type modelComponentType = modelComponent.GetType();
        PropertyInfo info = attrDef.GetPropertyInfo(modelComponentType);
        if (info == null) {
          throw new Exception(string.Format("Setter property '{0}' on Model Attribute Definition '{1}' does not exist on type {2}",
            attrDef.Setter, attrDef.Name, modelComponentType.Name));
        }
        info.SetValue(modelComponent, typedValue);
      }
    }

    private void ErrorOnUnknownAttributes(TreeHash hash, AppliesTo type, string[] ignoreAttributes) {
      HashSet<string> validAttributeNames =
        new HashSet<string>(ModelAttributeDefinitions.All.Where(x => x.AppliesToType(type))
          .Select(x => x.Name));

      foreach (TreeAttribute attribute in hash.Attributes) {
        if (ignoreAttributes.Contains(attribute.Key))
          continue;

        if (!validAttributeNames.Contains(attribute.Key))
          _messages.AddError(attribute,
            string.Format("Unknown attribute '{0}' on {1}", attribute.Key, type));
      }
    }

    internal static bool IsFormula(string valueOrFormula, out string strippedFormula) {
      string trimmed = valueOrFormula.Trim();

      if (trimmed.StartsWith("=")) {
        strippedFormula = trimmed.Substring(1);
        return true;
      }

      strippedFormula = null;
      return false;
    }
  }
}
