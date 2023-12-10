using System.Collections.Generic;
using System.Linq;
using x10.ui.composition;
using x10.ui.libraries;

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
    /// Similar as the version for finding Instances, but returns a HashSet that 
    /// includes the passed-in ClassDefX10 and all nested ones.
    /// </summary>
    public static HashSet<ClassDefX10> ListSelfAndDescendants(ClassDefX10 classDef) {
      IEnumerable<Instance> instances = ListSelfAndDescendants(classDef.RootChild);
      IEnumerable<ClassDefX10> classDefX10s = instances.Select(x => x.RenderAs).OfType<ClassDefX10>();
      HashSet<ClassDefX10> hashSet = new HashSet<ClassDefX10>(classDefX10s);

      // We must explicitly add the inital classDef. Note that classDef.RootChild.RenderAs is
      // the native "Form" component, not classDef itself.
      hashSet.Add(classDef);

      return hashSet;
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

    public static bool IsForm(ClassDefX10 classDef) {
      return classDef.RootChild.RenderAs.Name == BaseLibrary.CLASS_DEF_FORM;
    }

  }
}
