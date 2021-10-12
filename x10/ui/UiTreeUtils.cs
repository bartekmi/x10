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
    //
    // Note that there is a special case when intermedInst is the Root Child of a top-level ClassDef
    public static InstanceClassDefUse InsertIntermediateParent(Instance childInst, ClassDef intermidateClassDef) {
      UiAttributeDefinitionComplex wrapperPrimaryAttr = intermidateClassDef.PrimaryAttributeDef as UiAttributeDefinitionComplex;
      if (wrapperPrimaryAttr == null)
        throw new Exception(string.Format("ClassDef '{0}' does not have a complex primary attribute", intermidateClassDef.Name));

      // Create the new intermediate instance and its main content 
      InstanceClassDefUse intermedInst = new InstanceClassDefUse(intermidateClassDef, childInst.XmlElement, childInst.Owner);
      intermedInst.Id = childInst.Id;
      UiAttributeValueComplex intermedCplxAttr = wrapperPrimaryAttr.CreateValueAndAddToOwnerComplex(intermedInst, childInst.XmlElement);
      
      // Hook-up parent/child relationships 
      if (childInst.Owner == null) {
        // Special case: childInst is the "Root Child" of the top-most ClassDef
        ClassDefX10 topClassDef = (ClassDefX10)childInst.OwnerClassDef;
        if (topClassDef == null)
          throw new Exception("Neither Owner nor OwnerClassDef is set!");
        topClassDef.RootChild = intermedInst;
        childInst.Owner = intermedCplxAttr;
        intermedInst.OwnerClassDef = topClassDef;
      } else
        childInst.Owner.ReplaceInstance(childInst, intermedInst);

      intermedCplxAttr.AddInstance(childInst);

      return intermedInst;
    }
  }
}