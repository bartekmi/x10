using System;
using System.Collections.Generic;
using System.Linq;
using x10.ui.metadata;

namespace x10.ui.platform {
  public class PlatformClassDef {
    // The library which owns this definition
    public PlatformLibrary Owner { get; internal set; }

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

    // Optional parent class (name)
    public string InheritsFromName { get; set; }

    // In some cases, the platform-specific implementation of a logical components maps to multiple
    // nested components. In those, cases, use this property to define chained nested components.
    // One example is the logical TableColumn component which maps to multiple nested components in WPF.
    public PlatformClassDef NestedClassDef { get; set; }

    // If present, this specifies the name of the wrapper property that the primary content attribute
    // should be wrapped in. For example, column definitions are the primary attribute of Table in 
    // the logical model, but when generating code, they may have to be mapped to a specific 'columnDefinitions'
    // property.
    public string PrimaryAttributeWrapperProperty { get; set; }

    // If present, import the component from this directory.
    public string ImportDir {get;set;}

    // Attributes - both static and dynamic
    private IEnumerable<PlatformAttribute> _platformAttributes;
    public IEnumerable<PlatformAttribute> PlatformAttributes { 
      get { return _platformAttributes; }
      set {
        _platformAttributes = value;
        foreach (PlatformAttribute attribute in value)
          attribute.Owner = this;
      }
    }


    // Derived
    public IEnumerable<PlatformAttributeStatic> StaticPlatformAttributes {
      get { return PlatformAttributes.OfType<PlatformAttributeStatic>(); }
    }
    public IEnumerable<PlatformAttributeDynamic> DynamicPlatformAttributes {
      get { return PlatformAttributes.OfType<PlatformAttributeDynamic>(); }
    }
    public IEnumerable<PlatformAttributeByFunc> ByFuncPlatformAttributes {
      get { return PlatformAttributes.OfType<PlatformAttributeByFunc>(); }
    }
    public PlatformAttributeDynamic DataBindAttribute {
      get { return PlatformAttributes.OfType<PlatformAttributeDynamic>().SingleOrDefault(x => x.IsMainDatabindingAttribute); }
    }
    public string EffectivePlatformName {
      get { return PlatformName == null ? InheritsFrom.EffectivePlatformName : PlatformName; }
    }
    public string ImportPath {
      get {
        string dir = ImportDir == null ? Owner.ImportPath : ImportDir;
        return string.Format("{0}/{1}", dir, PlatformName);
      }
    }

    // Hydrated
    public ClassDef LogicalClassDef { get; internal set; }
    public PlatformClassDef InheritsFrom { get; set; }

    public PlatformClassDef() {
      PlatformAttributes = new List<PlatformAttribute>();
    }

    internal PlatformAttributeDynamic FindDyamicAttribute(string logicalAttrName) {
      if (InheritsFrom != null) {
        PlatformAttributeDynamic baseClasAttr = InheritsFrom.FindDyamicAttribute(logicalAttrName);
        if (baseClasAttr != null)
          return baseClasAttr;
      }
      return DynamicPlatformAttributes.FirstOrDefault(x => x.LogicalName == logicalAttrName);
    }

    public override string ToString() {
      return string.Format("{0} => {1}", LogicalName, EffectivePlatformName);
    }
  }
}
