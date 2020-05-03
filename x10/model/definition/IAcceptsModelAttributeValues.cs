using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.parsing;

namespace x10.model.definition {
  public interface IAcceptsModelAttributeValues {
    // This is where the values of custom properties are stored
    List<ModelAttributeValue> AttributeValues { get; }

    // The tree element from which this building block was created so if there are errors,
    // we can trace them back to the code file and location
    TreeElement TreeElement { get; }
  }

  public static class IAcceptsUiAttributeValuesExtensions {
    public static object FindValue(this IAcceptsModelAttributeValues source, string attributeName) {
      ModelAttributeValue value = FindAttribute(source, attributeName);
      return value?.Value;
    }

    public static bool FindValue<T>(this IAcceptsModelAttributeValues source, string attributeName, out T objValue) {
      ModelAttributeValue value = FindAttribute(source, attributeName);
      objValue = (T)value?.Value;
      return value != null;
    }

    public static T FindValue<T>(this IAcceptsModelAttributeValues source, string attributeName) where T : class {
      ModelAttributeValue value = FindAttribute(source, attributeName);
      return value?.Value as T;
    }

    public static ModelAttributeValue FindAttribute(this IAcceptsModelAttributeValues source, string attributeName) {
      return source.AttributeValues
        .FirstOrDefault(x => x.Definition.Name == attributeName);
    }

    public static bool FindBoolean(this IAcceptsModelAttributeValues source, string attrName, bool defaultIfNotFound) {
      object value = FindValue(source, attrName);
      if (!(value is bool))
        return defaultIfNotFound;
      return (bool)value;
    }
  }
}
