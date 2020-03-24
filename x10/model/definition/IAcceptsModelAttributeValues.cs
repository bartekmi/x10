using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.model.definition {
  public interface IAcceptsModelAttributeValues {
    List<ModelAttributeValue> AttributeValues { get; }
  }

  public static class AttributeUtils {
    public static object FindValue(IAcceptsModelAttributeValues source, string attributeName) {
      ModelAttributeValue value = source.AttributeValues.FirstOrDefault(x => x.Definition.Name == attributeName);
      return value == null ? null : value.Value;
    }
  }
}
