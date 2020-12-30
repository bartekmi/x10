using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.ui.composition;
using x10.ui;

namespace x10.gen {
  public static class CodeGenUtils {

    // Get the binding path of an instance as a list of members
    public static List<Member> GetBindingPath(Instance instance) {
      List<Member> members = new List<Member>();

      foreach (Instance item in UiUtils.ListSelfAndAncestors(instance).Reverse()) {
        if (item.PathComponents == null)
          continue;
        members.AddRange(item.PathComponents);
      }

      return members;
    }


  }
}