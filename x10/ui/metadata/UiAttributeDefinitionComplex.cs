using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.ui.composition;

namespace x10.ui.metadata {
  public class UiAttributeDefinitionComplex : UiAttributeDefinition {
    // The type of every complex attribute definition (enalogous to an attribute in WPF
    // which has to be defined with the <Class.Attribute> syntax) is defined
    // by a ClassDef object
    public ClassDef ComplexAttributeType { get; set; }

    // Will by hydrated into the actual object above
    public string ComplexAttributeTypeName { get; set; }

    // If true, the data model passed to this attribute "reduces" from many to one.
    // Classical case for this is the child of a multi-display component such as list or table
    public bool ReducesManyToOne { get; set; }

    // Specifies how to deal with Model References (i.e. InstanceModelRef).
    // For example, in a Table, model ref's correspond to columns; in a Form, 
    // model ref's correspond to fields with validation, and so on.
    // If this definition is present, a model ref will be translated 
    // to an instance of this component type (InstanceClassDefUse)
    // with the apropriate path delivering the model reference
    // This name will be hydrated into the actual object
    public string ModelRefWrapperComponentName { get; set; }

    // Hydrated
    public ClassDef ModelRefWrapperComponent { get; set; }

    public UiAttributeValueComplex CreateValueAndAddToOwnerComplex(IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      return (UiAttributeValueComplex)CreateValueAndAddToOwner(owner, xmlBase);
    }
  }
}
