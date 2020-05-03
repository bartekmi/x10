using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.parsing;
using x10.ui.metadata;
using x10.utils;

namespace x10.ui.composition {
  public interface IAcceptsUiAttributeValues {
    // This is where the values of custom properties are stored
    List<UiAttributeValue> AttributeValues { get; }

    // The xml element from which this building block was created so if there are errors,
    // we can trace them back to the code file and location
    XmlElement XmlElement { get; }

    ClassDef ClassDef { get; }
    string GetElementName();
  }

  public static class IAcceptsUiAttributeValuesExtensions {
    public static object FindValue(this IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValueAtomic value = FindAttributeValue(source, attributeName) as UiAttributeValueAtomic;
      return value?.Value;
    }

    public static T FindValue<T>(this IAcceptsUiAttributeValues source, string attributeName) where T : class {
      UiAttributeValueAtomic value = FindAttributeValue(source, attributeName) as UiAttributeValueAtomic;
      return value?.Value as T;
    }

    public static UiAttributeValue FindAttributeValue(this IAcceptsUiAttributeValues source, string attributeName) {
      return source.AttributeValues
        .FirstOrDefault(x => x.Definition.Name == attributeName);
    }

    public static IEnumerable<UiAttributeValueAtomic> AtomicAttributeValues(this IAcceptsUiAttributeValues source) {
      return source.AttributeValues.OfType<UiAttributeValueAtomic>();
    }
    public static IEnumerable<UiAttributeValueComplex> ComplexAttributeValues(this IAcceptsUiAttributeValues source) {
      return source.AttributeValues.OfType<UiAttributeValueComplex>();
    }


    public static void Print(this IAcceptsUiAttributeValues source, TextWriter writer, int indent, PrintConfig config = null) {
      PrintUtils.Indent(writer, indent);
      writer.Write("<" + source.GetElementName());

      foreach (UiAttributeValueAtomic atomic in source.AtomicAttributeValues())
        atomic.Print(writer);

      if (config != null) {
        if (config.AlwaysPrintRenderAs)
          if (source is InstanceModelRef modelRef)
            writer.Write(" renderAs='{0}'", modelRef.RenderAs == null ? "NULL" : modelRef.RenderAs.Name);
      }

      if (source.ComplexAttributeValues().Count() == 0 && !(source is ClassDefX10))
        writer.WriteLine("/>");
      else {
        writer.WriteLine(">");

        foreach (UiAttributeValueComplex complex in source.ComplexAttributeValues())
          complex.Print(writer, indent + 1, config);

        if (source is ClassDefX10 classDef)
          classDef.RootChild.Print(writer, indent + 1, config);

        PrintUtils.Indent(writer, indent);
        writer.WriteLine("</" + source.GetElementName() + ">");
      }
    }
  }
}
