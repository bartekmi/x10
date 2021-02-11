using System;
using System.Collections.Generic;
using System.Text;

using x10.gen;
using x10.ui.composition;

namespace x10.ui.platform {
  // Base class for static and dynamic platform-specific attributes
  public abstract class PlatformAttribute {

    public abstract object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet);

    // Name of this attribute in the platform-specific world.
    // Used for actual code-generation
    public string PlatformName { get; set; }

    // If true, this attribute generates a Code Snippet
    public bool IsCodeSnippet { get; set; }

    // If otherwise the value would be null and this field is present,
    // it will be returned
    public object DefaultValue { get; set; }

    // If present, if the value is Equal() to this, don't bother generating an attribute
    // This is to prevent things like "readOnly={ false }" which just pollute the code
    public object AttributeUnnecessaryWhen {get;set;}

    public PlatformClassDef Owner { get; set; }

    public override string ToString() {
      return PlatformName;
    }
  }
}
