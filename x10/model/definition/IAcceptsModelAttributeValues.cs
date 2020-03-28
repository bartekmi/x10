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

  public static class AttributeUtils {
    public static object FindValue(IAcceptsModelAttributeValues source, string attributeName) {
      ModelAttributeValue value = FindAttribute(source, attributeName);
      return value?.Value;
    }

    public static ModelAttributeValue FindAttribute(IAcceptsModelAttributeValues source, string attributeName) {
      return source.AttributeValues
        .FirstOrDefault(x => x.Definition.Name == attributeName);
    }
  }
}
