using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using static x10.ui.metadata.ClassDefNative;
using x10.ui.composition;
using x10.model;

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
    public bool IsMany { get; set; }

    // Spacifies Inheritance base class - in the object-oriented sense. Children inherit 
    // all the Attribute Definitions of their ancestors
    // This name will be hydrated into the actual object
    public string InheritsFromName { get; set; }

    // Hydrated
    public ClassDef InheritsFrom { get; set; }

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
      get { return DataModelType != null; }
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

    // If this ClassDef has a primary attribute - the attribute which corresponds to the content between
    // the XML tags, as in: <MyClassDef>...my content...</MyClassDef>, return it.
    public UiAttributeDefinition PrimaryAttributeDef {
      get { return AttributeDefinitions.SingleOrDefault(x => x.IsPrimary); }
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
