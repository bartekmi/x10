using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using x10.parsing;
using x10.formula;
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

    private void ReadAttribute(
      TreeHash hash,
      AppliesTo type,
      IAcceptsModelAttributeValues modelComponent,
      ModelAttributeDefinition attrDef) {

      // Error if mandatory attribute missing
      TreeNode attrNode = hash.FindNode(attrDef.Name);
      object typedValue;
      string formula = null;

      if (attrNode == null) {
        if (attrDef.DefaultFunc != null)
          typedValue = attrDef.DefaultFunc(modelComponent);
        else {
          if (attrDef.ErrorSeverityIfMissing != null)
            _messages.AddMessage(attrDef.ErrorSeverityIfMissing.Value, hash,
              "The attribute '{0}' is missing from {1}", attrDef.Name, type);
          return;
        }
      } else {
        bool success = ExtractTypedValue(attrNode, type, modelComponent, attrDef, out typedValue, out formula);
        if (!success)
          return;
      }

      // If a setter has been provided, use it; 
      if (typedValue != null)
        SetValueViaSetter(attrDef, modelComponent, typedValue);

      // A ModelAttributeValue is always stored, even if a setter exists. For one thing,
      // this is the only way we can track where the attribute came from in the YAML.
      modelComponent.AttributeValues.Add(new ModelAttributeValue(attrNode) {
        Value = typedValue,
        Formula = formula,
        Definition = attrDef,
      });
    }

    private bool ExtractTypedValue(
      TreeNode attrNode,
      AppliesTo type,
      IAcceptsModelAttributeValues modelComponent,
      ModelAttributeDefinition attrDef,
      out object typedValue,
      out string formula
    ) {
      typedValue = null;
      formula = null;

      if (attrDef is ModelAttributeDefinitionAtomic attrDefAtomic) {
        // Ensure that the value of the attribute is a scalar (not a list, etc)
        TreeScalar scalarNode = attrNode as TreeScalar;
        if (scalarNode == null) {
          _messages.AddError(attrNode, "The attribute '{0}' should be a simple string of the correct type, but is a {1}",
            attrDef.Name, attrNode.GetType().Name);
          return false;
        }

        string value = scalarNode.Value.ToString();
        if (FormulaUtils.IsFormula(value, out string strippedFormula))
          formula = strippedFormula;
        else {
          if (attrDefAtomic.MustBeFormula) {
            _messages.AddError(attrNode, "Attribute '{0}' must be a formula (must start with '=').", attrDefAtomic.Name);
            return false;
          }

          if (attrDefAtomic.DataTypeMustBeSameAsAttribute)
            typedValue = value;
          else {
            // Attempt to parse the string attribute value according to its data type
            DataType dataType = attrDefAtomic.DataType;
            typedValue = dataType.Parse(value, _messages, scalarNode, attrDef.Name);
            if (typedValue == null)
              return false;
          }

          // Do validation, if requried
          attrDefAtomic.ValidationFunction?.Invoke(_messages, scalarNode, modelComponent, type);
        }
      } else if (attrDef is ModelAttributeDefinitionComplex attrDefComplex) {
        typedValue = attrDefComplex.ParseFunction.Invoke(_messages, attrNode);
      } else
        throw new Exception("Unexpected type: " + attrDef.GetType().Name);

      return true;
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
        new HashSet<string>(ModelAttributeDefinitions.All
          .Where(x => x.AppliesToType(type))
          .Select(x => x.Name));

      foreach (TreeAttribute attribute in hash.Attributes) {
        string attrName = attribute.Key;
        if (ignoreAttributes.Contains(attrName))
          continue;

        if (!validAttributeNames.Contains(attrName))
          _messages.AddError(attribute, attrName, validAttributeNames, "Unknown attribute '{0}' on {1}", attrName, type);
      }
    }
  }
}
