using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model.metadata;
using x10.parsing;

namespace x10.ui.metadata {
  public class UiLibraryValidator {

    private MessageBucket _messages;

    public UiLibraryValidator(MessageBucket messages) {
      _messages = messages;
    }

    public void HydrateAndValidate(UiLibrary library) {
      foreach (ClassDef classDef in library.All) {
        HydrateOwner(classDef);
        HydrateEnsureBaseClassPresent(library, classDef);
      }

      foreach (ClassDef classDef in library.All)
        EnsureNoCircularInheritance(classDef);

      // The other checks are susceptible to circular dependencies
      if (!_messages.HasErrors)
        foreach (ClassDef classDef in library.All) {
          EnsureNoDuplicateAttributes(classDef);
          EnsureMaxOnePrimaryAttribute(classDef);
        }
    }

    private void HydrateOwner(ClassDef classDef) {
      foreach (UiAttributeDefinition attrDef in classDef.LocalAttributeDefinitions)
        attrDef.Owner = classDef;
    }

    private void HydrateEnsureBaseClassPresent(UiLibrary library, ClassDef classDef) {
      if (classDef.InheritsFromName == null && classDef.InheritsFrom == null)
        _messages.AddError(null, "{0} does not specify Inherits-From", classDef.Name);

      if (classDef.InheritsFromName != null) {
        classDef.InheritsFrom = library.FindComponentByName(classDef.InheritsFromName);
        if (classDef.InheritsFrom == null)
          _messages.AddError(null, "Specified Inherits-From {0} on Class Definition {1} does not exist.",
            classDef.InheritsFromName, classDef.Name);
      }
    }

    private void EnsureNoDuplicateAttributes(ClassDef classDef) {
      var collisions = classDef.AttributeDefinitions.GroupBy(x => x.Name).Where(x => x.Count() > 1);
      if (collisions.Count() > 0)
        _messages.AddError(null, "The following attributes of {0} are non-unique: {1}",
          classDef.Name, string.Join(", ", collisions.Select(x => x.Key)));
    }

    private void EnsureMaxOnePrimaryAttribute(ClassDef classDef) {
      var primary = classDef.AttributeDefinitions.Where(x => x.IsPrimary);
      if (primary.Count() > 1)
        _messages.AddError(null, "{0} contains more than one Primary Attribute: {1}",
          classDef.Name, string.Join(", ", primary.Select(x => x.Name)));
    }

    private void EnsureNoCircularInheritance(ClassDef classDef) {
      HashSet<ClassDef> visited = new HashSet<ClassDef>();
      ClassDef pointer = classDef;

      do {
        visited.Add(pointer);
        pointer = pointer.InheritsFrom;

        if (visited.Contains(pointer)) {
          _messages.AddError(null, "{0} is involved in a circular inheritance dependency",
            classDef.Name);
          break;
        }

      } while (pointer != null);
    }
  }
}
