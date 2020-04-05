using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.parsing;

namespace x10.ui.composition {
  public interface IAcceptsUiAttributeValues {
    // This is where the values of custom properties are stored
    List<UiAttributeValue> AttributeValues { get; }

    // The xml element from which this building block was created so if there are errors,
    // we can trace them back to the code file and location
    XmlBase XmlElement { get; }
  }

  public static class AttributeUtils {
    public static object FindValue(IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValue value = FindAttribute(source, attributeName);
      return value?.Value;
    }

    public static UiAttributeValue FindAttribute(IAcceptsUiAttributeValues source, string attributeName) {
      return source.AttributeValues
        .FirstOrDefault(x => x.Definition.Name == attributeName);
    }
  }
}
