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

    // If true, this component only exists as a base-class
    // for other components - it will never be rendered directly
    public bool IsAbstract { get; set; }

    // Optional platform-specific style information that may be needed during code-generation
    public string StyleInfo { get; set; }

    // Optional parent class 
    public PlatformClassDef InheritsFrom { get; internal set; }

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

    internal PlatformAttributeDynamic FindDyamicAttribute(string logicalAttrName) {
      return DynamicPlatformAttributes.FirstOrDefault(x => x.LogicalName == logicalAttrName);
    }

    public override string ToString() {
      return string.Format("{0} => {1}", LogicalName, PlatformName);
    }
  }
}
