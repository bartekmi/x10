using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using x10.ui.metadata;

namespace x10.ui.platform {
  public class PlatformClassDef {
    // Name of equivalent component in the logical "ClassDef" world
    public string LogicalName { get; set; }

    // Name of this component in the platform-specific world.
    // Used for actual code-generation
    public string PlatformName { get; set; }

    // Optional platform-specific style information that may be needed during code-generation
    public string StyleInfo { get; set; }

    // Attributes - both static and dynamic
    public IEnumerable<PlatformAttribute> PlatformAttributes { get; set; }


    // Derived
    public IEnumerable<PlatformAttributeStatic> StaticPlatformAttributes {
      get { return PlatformAttributes.OfType<PlatformAttributeStatic>(); }
    }
    public IEnumerable<PlatformAttributeDynamic> DynamicPlatformAttributes {
      get { return PlatformAttributes.OfType<PlatformAttributeDynamic>(); }
    }
    public PlatformAttributeDataBind DataBindAttribute {
      get { return PlatformAttributes.OfType<PlatformAttributeDataBind>().SingleOrDefault(); }
    }

    // Hydrated
    public ClassDef LogicalClassDef { get; internal set; }

    public PlatformClassDef() {
      PlatformAttributes = new List<PlatformAttribute>();
    }
  }
}
