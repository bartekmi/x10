using System.Linq;
using System.Collections.Generic;

using x10.ui.composition;

namespace x10.ui.platform {
  public static class PlatformUtils {

    // Return dot-separated models path for this instance.
    // Correctly handle situation if the instance is a Wrapper Component (e.g. TableColumn)
    public static string ComposePath(Instance thisInstance) {
      List<string> names = new List<string>();

      thisInstance = Unwrap(thisInstance);

      foreach (Instance instance in UiUtils.ListSelfAndAncestors(thisInstance).Reverse()) {
        if (instance.PathComponents == null)
          continue;
        names.AddRange(instance.PathComponents.Select(x => x.Name));
      }

      return string.Join(".", names);
    }

    // Return <instance>, or its single child, if this instance is a wrapper
    public static Instance Unwrap(Instance instance) {
      if (instance.IsWrapper) 
        instance = (instance.PrimaryValue as UiAttributeValueComplex).Instances.Single();
      return instance;
    }
  }
}