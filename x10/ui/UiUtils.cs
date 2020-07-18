using System;
using System.Collections.Generic;
using System.Text;
using x10.ui.composition;

namespace x10.ui {
  public static class UiUtils {
    /// <summary>
    /// Recursively list all instances including this one and all descendents
    /// </summary>
    public static IEnumerable<Instance> ListAllInstances(Instance instance) {
      List<Instance> instances = new List<Instance>();
      ListAllInstances(instances, instance);
      return instances;
    }

    private static void ListAllInstances(List<Instance> instances, Instance instance) {
      instances.Add(instance);

      foreach (Instance child in instance.ChildInstances)
        ListAllInstances(instances, child);
    }
  }
}
