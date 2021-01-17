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
    public static List<Member> GetBindingPath(Instance instance) {
      List<Member> members = new List<Member>();
      instance = Unwrap(instance);

      foreach (Instance item in UiUtils.ListSelfAndAncestors(instance).Reverse()) {
        if (item.PathComponents == null)
          continue;
        members.AddRange(item.PathComponents);
      }

      return members;
    }

    public static string GetBindingPathAsString(Instance instance) {
      return string.Join(".", GetBindingPath(instance).Select(x => x.Name));
    }

    // Return <instance>, or its single child, if this instance is a wrapper
    public static Instance Unwrap(Instance instance) {
      if (instance.IsWrapper) 
        instance = (instance.PrimaryValue as UiAttributeValueComplex).Instances.Single();
      return instance;
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
      UiAttributeValueAtomic readOnlyAttr = instance.FindAttributeValueRespectInheritable(ClassDefNative.ATTR_READ_ONLY_OBJ);
      if (readOnlyAttr != null)
        return (bool)readOnlyAttr.Value;

      return false; // By default, we allow editing
    }
  }
}