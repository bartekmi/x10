using System;
using System.Collections.Generic;
using System.Text;
using x10.ui.composition;

namespace x10.ui {
  public static class UiUtils {
    /// <summary>
    /// Recursively list all instances including this one and all descendents
    /// </summary>
    public static IEnumerable<Instance> ListSelfAndDescendants(Instance instance) {
      List<Instance> instances = new List<Instance>();
      ListAllInstances(instances, instance);
      return instances;
    }

    private static void ListAllInstances(List<Instance> instances, Instance instance) {
      if (instance == null)
        return;

      instances.Add(instance);

      foreach (Instance child in instance.ChildInstances)
        ListAllInstances(instances, child);
    }

    /// <summary>
    /// List self, then all ancestors in order down to root
    /// </summary>
    public static IEnumerable<Instance> ListSelfAndAncestors(Instance instance) {
      List<Instance> all = new List<Instance>();

      while (instance != null) {
        all.Add(instance);
        instance = instance.ParentInstance;
      }

      return all;
    }
  }
}
