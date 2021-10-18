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
    string DebugPrintAs();
  }

  public static class IAcceptsUiAttributeValuesExtensions {
    public static object FindValue(this IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValueAtomic value = FindAttributeValue(source, attributeName) as UiAttributeValueAtomic;
      return value?.Value;
    }

    public static T FindValue<T>(this IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValueAtomic atomicValue = FindAttributeValue(source, attributeName) as UiAttributeValueAtomic;
      object value = atomicValue?.Value;

      if (value is T)
        return (T)value;

      return default(T);
    }

    public static UiAttributeValue FindAttributeValue(this IAcceptsUiAttributeValues source, string attributeName) {
      return source.AttributeValues
        .FirstOrDefault(x => x.Definition.Name == attributeName);
    }

    public static Instance FindSingleComplexAttributeInstance(this IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValueComplex cplxAttr = source.FindAttributeValue(attributeName) as UiAttributeValueComplex;
      return cplxAttr?.Instances.SingleOrDefault();
    }

    public static bool HasAttributeValue(this IAcceptsUiAttributeValues source, string attributeName) {
      return FindAttributeValue(source, attributeName) != null;
    }

    public static UiAttributeValue RemoveAttributeValue(this IAcceptsUiAttributeValues source, string attributeName) {
      UiAttributeValue value = FindAttributeValue(source, attributeName);
      if (value == null)
        return null;
      source.AttributeValues.Remove(value);
      return value;
    }

    public static IEnumerable<UiAttributeValueAtomic> AtomicAttributeValues(this IAcceptsUiAttributeValues source) {
      return source.AttributeValues.OfType<UiAttributeValueAtomic>();
    }
    public static IEnumerable<UiAttributeValueComplex> ComplexAttributeValues(this IAcceptsUiAttributeValues source) {
      return source.AttributeValues.OfType<UiAttributeValueComplex>();
    }


    public static void Print(this IAcceptsUiAttributeValues source, TextWriter writer, int indent, PrintConfig config = null) {
      PrintUtils.Indent(writer, indent);
      writer.Write("<" + source.DebugPrintAs());

      foreach (UiAttributeValueAtomic atomic in source.AtomicAttributeValues())
        atomic.Print(writer);

      Instance instance = source as Instance;

      if (config != null) {
        // There's no point printing RenderAs for ClassDefUse's since it's same as name
        if (config.AlwaysPrintRenderAs && instance is InstanceModelRef)
          writer.Write(" renderAs='{0}'", instance.RenderAs?.Name ?? "NULL");
        if (config.PrintModelMember && instance?.PathComponents?.Count > 0)
          writer.Write(" pathComps='{0}'", string.Join(", ", instance.PathComponents.Select(x => x.Name)));
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
        writer.WriteLine("</" + source.DebugPrintAs() + ">");
      }
    }
  }
}
