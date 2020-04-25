﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;

namespace x10.ui.metadata {

  public enum DataModelType {
    Scalar,
    Entity,
  }

  public abstract class ClassDef {
    public string Name { get; set; }
    public string Description { get; set; }

    // Attributes are currently only defined on native components
    // We put this here at the base class level in anticipation of having a mechanism
    // of definition attributes on X10 components so as to make them re-usable
    // with tweaks.
    public IEnumerable<UiAttributeDefinition> LocalAttributeDefinitions;

    // Every x10 UI Component must have a data model. In addition, specilized
    // "native" components might be crafted which also reference X10 data models.
    public Entity ComponentDataModel { get; set; }

    // For Components which deal with atomic data (e.g. TextField) this specifies the expected data type
    public DataType AtomicDataModel { get; set; }

    // Is the Component Data Model a list?
    public bool? IsMany { get; set; }

    // Spacifies Inheritance base class - in the object-oriented sense. Children inherit 
    // all the Attribute Definitions of their ancestors
    // This name will be hydrated into the actual object
    public string InheritsFromName { get; set; }

    // Specifies how to deal with Model References (i.e. InstanceModelRef).
    // For example, in a Table, model ref's correspond to columns; in a Form, model ref's correspond to fields with validation,
    // and so on.
    // If this definition is present, a model ref will be translated to an instance of this component type (InstanceClassDefUse)
    // with the apropriate path delivering the model reference
    // This name will be hydrated into the actual object
    public string ModelRefWrapperComponentName { get; set; }

    // Hydrated
    public ClassDef InheritsFrom { get; set; }
    public ClassDef ModelRefWrapperComponent { get; set; }

    // Derived

    // If present, specifies the type of the expected data model
    public DataModelType? DataModelType {
      get {
        if (ComponentDataModel == null && AtomicDataModel == null)
          return null;
        return AtomicDataModel == null ? metadata.DataModelType.Entity : metadata.DataModelType.Scalar;
      }
    }
    public bool CaresAboutDataModel {
      get { return DataModelType != null && IsMany != null; }
    }
    public IEnumerable<UiAttributeDefinition> AttributeDefinitions {
      get {
        return InheritsFrom == null ?
          LocalAttributeDefinitions :
          InheritsFrom.AttributeDefinitions.Concat(LocalAttributeDefinitions);
      }
    }
    public IEnumerable<UiAttributeDefinitionAtomic> AtomicAttributeDefinitions {
      get { return AttributeDefinitions.OfType<UiAttributeDefinitionAtomic>(); }
    }
    public IEnumerable<UiAttributeDefinitionComplex> ComplexAttributeDefinitions {
      get { return AttributeDefinitions.OfType<UiAttributeDefinitionComplex>(); }
    }

    // For now, we will limit the primary attribute to be complex. In general, this should 
    // not have to be the case. In particular, it would be very convenient to have a Text
    // component where it's possible to type plain text as the "primary attribute" - 
    // or "content attribute" as it's known is XAML
    public UiAttributeDefinitionComplex PrimaryAttributeDef {
      get {
        UiAttributeDefinition attribute = AttributeDefinitions.SingleOrDefault(x => x.IsPrimary);
        if (attribute != null && !(attribute is UiAttributeDefinitionComplex))
          throw new Exception("Primary attribute must be complex - see comments above");
        return (UiAttributeDefinitionComplex)attribute;
      }
    }

    //----------------------------------------------------------------------------------

    protected ClassDef() {
      LocalAttributeDefinitions = new List<UiAttributeDefinition>();
    }

    // Is-a in an object-oriented sense. Returns true if the passed in parameter is this class-def
    // or if this class is a descndent of classDefOrAncestor
    public bool IsA(ClassDef classDefOrAncestor) {
      ClassDef classDef = this;
      while (classDef != null) {
        if (classDef == classDefOrAncestor)
          return true;
        classDef = classDef.InheritsFrom;
      }
      return false;
    }


    public UiAttributeDefinition FindAttribute(string attrName) {
      return AttributeDefinitions.SingleOrDefault(x => x.Name == attrName);
    }

    public UiAttributeDefinitionAtomic FindAtomicAttribute(string attrName) {
      return AtomicAttributeDefinitions.SingleOrDefault(x => x.Name == attrName);
    }

    public UiAttributeDefinitionComplex FindComplexAttribute(string attrName) {
      return ComplexAttributeDefinitions.SingleOrDefault(x => x.Name == attrName);
    }

    public override string ToString() {
      return "UiDefinition: " + Name;
    }
  }
}
