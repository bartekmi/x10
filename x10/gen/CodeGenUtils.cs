using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.ui;
using x10.formula;

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

    public static ExpBase PathToExpression(IEnumerable<Member> path) {
      FormulaParser parser = new FormulaParser(null, null, null, null);
      string pathAsString = string.Join(".", path.Select(x => x.Name));
      return parser.Parse(null, pathAsString, new X10DataType(path.First().Owner, false));
    }
  }
}