using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.parsing;
using x10.ui.metadata;

namespace x10.ui.platform {
  class PlatformLibraryValidator {
    private readonly MessageBucket _messages;

    internal PlatformLibraryValidator(MessageBucket messages) {
      _messages = messages;
    }

    internal void HydrateAndValidate(UiLibrary logicalLibrary, PlatformLibrary platformLibrary) {
      int blankNameClassDefs = platformLibrary.All.Count(x => string.IsNullOrWhiteSpace(x.PlatformName));
      if (blankNameClassDefs > 0)
        _messages.AddError(null, "{0} Platform Class Definitions have a blank Platform Name", blankNameClassDefs);

      foreach (PlatformClassDef classDef in platformLibrary.All) {
        ClassDef logical = HydrateAndValidateLogicalClassDef(logicalLibrary, classDef);
        if (logical != null)
          HydrateAndValidateAttributes(logical, classDef);
      }
    }

    private ClassDef HydrateAndValidateLogicalClassDef(UiLibrary logicalLibrary, PlatformClassDef classDef) {
      ClassDef logical = logicalLibrary.FindComponentByName(classDef.LogicalName);
      if (logical == null)
        _messages.AddError(null, "Platform UI Component {0} references Logical UI Component {1} which does not exist",
          classDef.PlatformName, classDef.LogicalName);
      else
        classDef.LogicalClassDef = logical;

      return logical;
    }

    private void HydrateAndValidateAttributes(ClassDef logical, PlatformClassDef platform) {
      foreach (PlatformAttribute attribute in platform.PlatformAttributes) {
        if (attribute is PlatformAttributeStatic staticAttr) 
          ValidateStaticAttribute(platform, staticAttr);
        else if (attribute is PlatformAttributeDynamic dynamicAttr)
          ValidateDynamicAttribute(logical, platform, dynamicAttr);


      }
    }

    private void ValidateStaticAttribute(PlatformClassDef platform, PlatformAttributeStatic staticAttr) {
      if (string.IsNullOrWhiteSpace(staticAttr.PlatformName) ||
          string.IsNullOrWhiteSpace(staticAttr.Value))
        _messages.AddError(null, "Platform Attribute {0} of Platform Component {1} must specify both name and value",
          staticAttr, platform.PlatformName);
    }

    private void ValidateDynamicAttribute(ClassDef logical, PlatformClassDef platform, PlatformAttributeDynamic dynamicAttr) {
      UiAttributeDefinition attrDef = logical.FindAttribute(dynamicAttr.LogicalName);
      if (attrDef == null)
        _messages.AddError(null, "Platform Attribute {0} refers to Logical Attribute {1} of {2} which does not exist",
          dynamicAttr.PlatformName, dynamicAttr.LogicalName, logical.Name);
    }
  }
}
