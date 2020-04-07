using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;

namespace x10.ui.metadata {
  public abstract class ClassDef {
    public string Name { get; set; }
    public string Description { get; set; }

    // Attributes are (almost) always defined on native components
    // We put this here at the base class level in anticipation of having a mechanism
    // of definition attributes on X10 components so as to make them re-usable
    // with tweaks.
    public List<UiAttributeDefinition> AttributeDefinitions { get; set; }

    // Is this a visual component (as opposed to just a logical one
    // like TableColumn, etc). If true, can participate in the visual 
    // hierarchy tree.
    public bool IsUi { get; set; }
  
    // Every x10 UI Component must have a data model. In addition, specilized
    // "native" components might be crafted which also reference X10 data models.
    public Entity ComponentDataModel { get; set; }

    // Is the Component Data Model a list?
    public bool IsMany { get; set; }

    public override string ToString() {
      return "UiDefinition: " + Name;
    }
  }
}
