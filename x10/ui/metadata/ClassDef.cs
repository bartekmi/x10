using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.parsing;
using x10.model.definition;

namespace x10.ui.metadata {

  public enum DataModelType {
    Scalar,
    Entity,
  }

  public abstract class ClassDef {
    public string Name { get; set; }
    public string Description { get; set; }

    // Attributes are (almost) always defined on native components
    // We put this here at the base class level in anticipation of having a mechanism
    // of definition attributes on X10 components so as to make them re-usable
    // with tweaks.
    public IEnumerable<UiAttributeDefinition> _attributeDefinitions { get; private set; }

    // Is this a visual component (as opposed to just a logical one
    // like TableColumn, etc). If true, can participate in the visual 
    // hierarchy tree.
    public bool IsUi { get; set; }

    // Every x10 UI Component must have a data model. In addition, specilized
    // "native" components might be crafted which also reference X10 data models.
    public Entity ComponentDataModel { get; set; }

    // Is the Component Data Model a list?
    public bool? IsMany { get; set; }

    // If present, specifies the type of the expected data model
    public DataModelType? DataModelType { get; set; }

    // Inheritance - in the object-oriented sense. Children inherit all the Attribute
    // Definitions of their ancestors
    public ClassDef InheritsFrom { get; set; }

    // Derived
    public bool CaresAboutDataModel {
      get { return DataModelType != null && IsMany != null; }
    }
    public IEnumerable<UiAttributeDefinition> AttributeDefinitions {
      get {
        return InheritsFrom == null ?
          _attributeDefinitions :
          InheritsFrom.AttributeDefinitions.Concat(_attributeDefinitions);
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

    protected ClassDef(IEnumerable<UiAttributeDefinition> attrDefinitions) {
      _attributeDefinitions = attrDefinitions;
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

    public override string ToString() {
      return "UiDefinition: " + Name;
    }
  }
}
