﻿using System;
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
      objValue = default;
      if (value == null)
        return false;

      objValue = (T)(value.Value);
      return value != null;
    }

    public static T FindValue<T>(this IAcceptsModelAttributeValues source, string attributeName, out ModelAttributeValue attrValue) {
      attrValue = FindAttribute(source, attributeName);
      return attrValue == null ? default : (T)attrValue?.Value;
    }

    public static T FindValue<T>(this IAcceptsModelAttributeValues source, string attributeName) {
      return FindValue<T>(source, attributeName, out ModelAttributeValue _);
    }

    public static bool HasAttribute(this IAcceptsModelAttributeValues source, string attributeName) {
      return FindAttribute(source, attributeName) != null;
    }

    public static ModelAttributeValue FindAttribute(this IAcceptsModelAttributeValues source, string attributeName) {
      ModelAttributeValue value = FindAttributeNoInheritance(source, attributeName);

      // For some attributes, search the inheritance chain
      if (value == null && source is Entity entity) {
        while (entity.InheritsFrom != null) {
          entity = entity.InheritsFrom;
          value = FindAttributeNoInheritance(entity, attributeName);

          if (value != null) {
            if (value.Definition.SearchInheritanceTree)
              return value;
            else
              return null;
          }
        }
      }

      return value;
    }

    private static ModelAttributeValue FindAttributeNoInheritance(IAcceptsModelAttributeValues source, string attributeName) {
      return source.AttributeValues.FirstOrDefault(x => x.Definition.Name == attributeName);
    }

    public static bool FindBoolean(this IAcceptsModelAttributeValues source, string attrName, bool defaultIfNotFound) {
      object value = FindValue(source, attrName);
      if (!(value is bool))
        return defaultIfNotFound;
      return (bool)value;
    }
  }
}
