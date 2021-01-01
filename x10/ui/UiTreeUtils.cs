using System;

using x10.ui.metadata;
using x10.ui.composition;

namespace x10.ui {
  // This is a set of powerful tools for easily manipulating the UI Logical tree,
  // rooted in ClassDefX10
  public static class UiTreeUtils {

    // Insert newParent between instance and its original parent/owner
    // This is used (for example) to insert an intermediate component to control
    // visibility when rendering to JavaScript.
    // One of the requirements of this method is that <intermediateClassDef> must have a primary complex attribute
    //
    // Before: parentInst->parentCplxAttr-> childInst
    // After:  parentInst->parentCplxAttr-> intermedInst->intermedCplxAttr-> childInst
    public static InstanceClassDefUse InsertIntermediateParent(Instance childInst, ClassDef intermidateClassDef) {
      UiAttributeValueComplex parentCplxAttr = childInst.Owner;

      UiAttributeDefinitionComplex wrapperPrimaryAttr = intermidateClassDef.PrimaryAttributeDef as UiAttributeDefinitionComplex;
      if (wrapperPrimaryAttr == null)
        throw new Exception(string.Format("ClassDef '{0}' does not have a complex primary attribute", intermidateClassDef.Name));

      // Create the new intermediate instance and its main content 
      InstanceClassDefUse intermedInst = new InstanceClassDefUse(intermidateClassDef, childInst.XmlElement, parentCplxAttr);
      UiAttributeValueComplex intermedCplxAttr = wrapperPrimaryAttr.CreateValueAndAddToOwnerComplex(intermedInst, childInst.XmlElement);
      
      // Hook-up parent/child relationships 
      parentCplxAttr.ReplaceInstance(childInst, intermedInst);
      intermedCplxAttr.AddInstance(childInst);

      return intermedInst;
    }
  }
}