using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.ui.metadata;
using x10.ui;
using x10.formula;
using x10.parsing;

namespace x10.gen {
  public static class CodeGenUtils {

    // Get the binding path of an instance as a list of members
    public static IEnumerable<Member> GetBindingPath(Instance startInstance) {
      List<Member> members = new List<Member>();
      startInstance = startInstance.Unwrap();

      foreach (Instance instance in UiUtils.ListSelfAndAncestors(startInstance)) {
        if (instance.PathComponents != null)
          // We are building the path backwards, but InstancePathComponents is listed
          // in forward order, so we must revere it.
          members.AddRange(instance.PathComponents.ToArray().Reverse());

        // Stop the binding path if parent display a list items
        if (instance.ParentInstance?.RenderAs?.PrimaryAttributeDef is UiAttributeDefinitionComplex complex &&
          complex.ReducesManyToOne)
          break;
      }

      return members.ToArray().Reverse();
    }

    public static string GetBindingPathAsString(Instance instance) {
      return string.Join(".", GetBindingPath(instance).Select(x => x.Name));
    }

    public static ExpBase PathToExpression(IEnumerable<Member> path) {
      MessageBucket messages = new MessageBucket();
      FormulaParser parser = new FormulaParser(messages, null, new model.AllEnums(messages), null);
      string pathAsString = string.Join(".", path.Select(x => x.Name));
      return parser.Parse(null, pathAsString, new X10DataType(path.First().Owner, false));
    }

    public static bool IsReadOnly(Instance instance) {
      // First, check if the Models force read-only (if model defines read-only, the member can NEVER be editable)
      IEnumerable<Member> path = GetBindingPath(instance);
      if (path.Any(x => x.IsReadOnly))
        return true;

      // Second, check if this instance or any above it have the Read Only attribute
      UiAttributeValueAtomic readOnlyAttr = instance.FindAttributeValueRespectInheritable(ClassDefNative.ATTR_READ_ONLY_OBJ)
        as UiAttributeValueAtomic;

      if (readOnlyAttr != null)
        return (bool)readOnlyAttr.Value;

      return false; // By default, we allow editing
    }
  }
}