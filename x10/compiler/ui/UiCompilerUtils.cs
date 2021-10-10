using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.ui.composition;
using x10.ui.metadata;
using x10.ui;


namespace x10.compiler.ui {
  public static class UiCompilerUtils {

    // Get the binding path of an instance as a list of members
    public static IEnumerable<Member> GetBindingPath(Instance startInstance) {
      List<Member> members = new List<Member>();

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

    // Returns true if the given instance is ALWAYS read-only and can be rendered
    // using read-only components.
    public static bool IsReadOnly(Instance instance) {
      // First, check if the Models force read-only (if model defines read-only, the member can NEVER be editable)
      IEnumerable<Member> path = GetBindingPath(instance);
      if (path.Any(x => x != null && x.IsReadOnly))
        return true;

      // Second, check if this instance or any above it have the Read Only attribute
      UiAttributeValueAtomic readOnlyAttr = instance.FindAttributeValueRespectInheritable(ClassDefNative.ATTR_READ_ONLY_OBJ)
        as UiAttributeValueAtomic;

      if (readOnlyAttr != null)
        return (bool)readOnlyAttr.Value;

      return true; // By default, we do NOT allow editing
    }

  }
}