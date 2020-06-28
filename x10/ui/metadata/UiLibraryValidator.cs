﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model;
using x10.model.metadata;
using x10.parsing;

namespace x10.ui.metadata {
  internal class UiLibraryValidator {

    private readonly MessageBucket _messages;

    internal UiLibraryValidator(MessageBucket messages) {
      _messages = messages;
    }

    internal void HydrateAndValidate(UiLibrary library) {
      int blankNameClassDefs = library.All.Count(x => string.IsNullOrWhiteSpace(x.Name));
      if (blankNameClassDefs > 0)
        _messages.AddError(null, "{0} Class Definitions have a blank name", blankNameClassDefs);

      foreach (ClassDef classDef in library.All) {
        HydrateOwner(classDef);
        HydrateAndValidateBaseClass(library, classDef);
        EnsurePropertiesValid(classDef);
        EnsureCorrectDataModelSpecification(classDef);
      }

      foreach (ClassDef classDef in library.All)
        EnsureNoCircularInheritance(classDef);

      // The other checks are susceptible to circular dependencies
      if (!_messages.HasErrors)
        foreach (ClassDef classDef in library.All) {
          EnsureNoDuplicateAttributes(classDef);
          EnsureMaxOnePrimaryAttribute(classDef);

          foreach (UiAttributeDefinition attrDef in classDef.LocalAttributeDefinitions)
            ValidateAttribute(library, attrDef);
        }
    }

    private void EnsureCorrectDataModelSpecification(ClassDef classDef) {
      if (classDef.AtomicDataModel != null && classDef.ComponentDataModel != null)
        _messages.AddError(null, "{0}: Only one of Atomic or Component data model can be specified", classDef.Name);
    }

    private void HydrateOwner(ClassDef classDef) {
      foreach (UiAttributeDefinition attrDef in classDef.LocalAttributeDefinitions)
        attrDef.Owner = classDef;
    }

    private void HydrateAndValidateBaseClass(UiLibrary library, ClassDef classDef) {
      string description = string.Format("Inherits-From parent of Class Definition {0}", classDef.Name);
      classDef.InheritsFrom = HydrateAndValidateClassDef(library, description, classDef.InheritsFrom, classDef.InheritsFromName, true);
    }

    private ClassDef HydrateAndValidateClassDef(UiLibrary library, string description, ClassDef theObject, string name, bool isMandatory) {
      if (isMandatory && name == null && theObject == null)
        _messages.AddError(null, "{0} is not defined", description);

      if (name != null) {
        theObject = library.FindComponentByName(name);
        if (theObject == null)
          _messages.AddError(null, "{0} '{1}' does not exist.",
            description, name);
      }

      return theObject;
    }

    private void EnsurePropertiesValid(ClassDef classDef) {
      MessageBucket messages = new MessageBucket();
      if (!ModelValidationUtils.ValidateUiElementName(classDef.Name, null, messages))
        throw new Exception(messages.Messages.Single().Message);
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

    private void ValidateAttribute(UiLibrary library, UiAttributeDefinition attrDef) {
      if (string.IsNullOrWhiteSpace(attrDef.Name)) {
        _messages.AddError(null, "{0} contains an attribute with no name", attrDef.Owner.Name);
        return;
      }

      if (attrDef is UiAttributeDefinitionComplex attrComplex) {
        ClassDef classDef = attrDef.Owner;
        string description = string.Format("Type of Complex Attribute {0}.{1}", classDef.Name, attrDef.Name);
        attrComplex.ComplexAttributeType = HydrateAndValidateClassDef(library, description, attrComplex.ComplexAttributeType, attrComplex.ComplexAttributeTypeName, true);

        string wrapperName = attrComplex.ModelRefWrapperComponentName;
        if (wrapperName != null) {
          description = string.Format("Model Reference Wrapper of {0}.{1}", classDef.Name, attrDef.Name);
          attrComplex.ModelRefWrapperComponent = HydrateAndValidateClassDef(library,
            description,
            attrComplex.ModelRefWrapperComponent,
            attrComplex.ModelRefWrapperComponentName,
            false);
        }
      }

      MessageBucket messages = new MessageBucket();
      if (attrDef is UiAttributeDefinitionAtomic) {
        if (!ModelValidationUtils.ValidateUiAtomicAttributeName(attrDef.Name, null, messages))
          throw new Exception(messages.Messages.Single().Message);
      } else if (attrDef is UiAttributeDefinitionComplex) {
        if (!ModelValidationUtils.ValidateUiComplexAttributeName(attrDef.Name, null, messages))
          throw new Exception(messages.Messages.Single().Message);
      } else
        throw new Exception("Unexpected attribute definition type: " + attrDef.GetType().Name);
    }
  }
}
