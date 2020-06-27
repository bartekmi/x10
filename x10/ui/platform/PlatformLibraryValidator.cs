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
      int blankNameClassDefs = platformLibrary.All.Count(x => string.IsNullOrWhiteSpace(x.LogicalName));
      if (blankNameClassDefs > 0)
        _messages.AddError(null, "{0} Platform Class Definitions have a blank Logical Name", blankNameClassDefs);

      // Check duplicate logical names
      IEnumerable<string> duplicateLogicalNames = platformLibrary.All.GroupBy(x => x.LogicalName)
        .Where(x => x.Count() > 1)
        .Select(x => x.Key)
        .Where(x => x != null);
      if (duplicateLogicalNames.Count() > 0)
        _messages.AddError(null, "Multiple definitions for Logical Names: {0}",
          string.Join(",", duplicateLogicalNames));

      // Individual validation of PlatformClassDef's
      foreach (PlatformClassDef classDef in platformLibrary.All) {
        ValidateNoDuplicateBindingAttributes(classDef);
        HydrateAndValidateLogicalClassDef(platformLibrary, logicalLibrary, classDef);
        if (classDef.LogicalClassDef != null) // Only hydrated after call above
          HydrateAndValidateAttributes(classDef.LogicalClassDef, classDef);
      }
    }

    private void ValidateNoDuplicateBindingAttributes(PlatformClassDef classDef) {
      if (classDef.PlatformAttributes.OfType<PlatformAttributeDataBind>().Count() > 1)
        _messages.AddError(null, "Platform UI Component {0} has multiple binding attributes",
          classDef.PlatformName);
    }

    private void HydrateAndValidateLogicalClassDef(PlatformLibrary platformLibrary, UiLibrary logicalLibrary, PlatformClassDef classDef) {
      // Logical Component
      classDef.LogicalClassDef = logicalLibrary.FindComponentByName(classDef.LogicalName);
      if (classDef.LogicalClassDef == null)
        _messages.AddError(null, "Platform UI Component {0} references Logical UI Component {1} which does not exist",
          classDef.PlatformName, classDef.LogicalName);
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
      if (attrDef == null) {
        if (dynamicAttr is PlatformAttributeDataBind) {
          // The main data-bind attribute doesn't need to exist as a logical attribute
        } else
          _messages.AddError(null, "Platform Attribute {0} refers to Logical Attribute {1} of {2} which does not exist",
            dynamicAttr.PlatformName, dynamicAttr.LogicalName, logical.Name);
      }
    }
  }
}
