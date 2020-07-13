using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using x10.model.definition;
using x10.utils;

// Validated the correctness of the Model Attribute Definitions
// 1) test that setters are valid. Especially important if we allow this
//    to be expandable.
// 2) Ensure setters accept a value of the correct type (e.g. InheritsFrom is
//    wrong because it tried to use String to assign to Entity
// 3) No duplicate attributes for same applies-to type

namespace x10.model.metadata {
  internal class ValidationError {
    internal string Messages;
    internal ModelAttributeDefinition Definition;

    public override string ToString() {
      return string.Format("Attribute '{0}': {1}", Definition.Name, Messages);
    }
  }

  internal class ModelAttributeDefinitionValidator {
    private List<ValidationError> _errors;

    internal List<ValidationError> Validate(IEnumerable<ModelAttributeDefinition> definitions) {
      _errors = new List<ValidationError>();
      HashSet<string> uniqueApplyToAndAttributeNames = new HashSet<string>();

      foreach (ModelAttributeDefinition definition in definitions) {
        ValidateSetter(definition);
        ValidateNoDuplicates(definition, uniqueApplyToAndAttributeNames);
      }

      return _errors;
    }

    private void ValidateSetter(ModelAttributeDefinition definition) {
      string setter = definition.Setter;
      if (setter == null)
        return;

      foreach (Type type in AppliesToHelper.GetTypesForAppliesTo(definition.AppliesTo)) {
        PropertyInfo info = definition.GetPropertyInfo(type);
        if (info == null) {
          string message = string.Format("Setter property '{0}' does not exist on type {1}",
            setter, type);

          _errors.Add(new ValidationError() {
            Messages = message,
            Definition = definition,
          });
        } else {
          // FUTURE: Validate type of setter
        }
      }
    }

    private void ValidateNoDuplicates(ModelAttributeDefinition definition, HashSet<string> uniqueApplyToAndAttributeNames) {
      foreach (AppliesTo appliesTo in AppliesToHelper.GetAllSingleAppliesTo(definition.AppliesTo)) {
        string key = string.Format("{0}.{1}", definition.Name, appliesTo);

        if (uniqueApplyToAndAttributeNames.Contains(key)) {
          string message = string.Format("This attribute is defined multiple times on {1}",
            definition.Name, appliesTo);

          _errors.Add(new ValidationError() {
            Messages = message,
            Definition = definition,
          });
        } else
          uniqueApplyToAndAttributeNames.Add(key);
      }
    }
  }
}
