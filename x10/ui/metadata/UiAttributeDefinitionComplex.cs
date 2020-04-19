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

    public UiAttributeValueComplex CreateValueAndAddToOwnerComplex(IAcceptsUiAttributeValues owner, XmlBase xmlBase) {
      return (UiAttributeValueComplex)CreateValueAndAddToOwner(owner, xmlBase);
    }
  }
}
