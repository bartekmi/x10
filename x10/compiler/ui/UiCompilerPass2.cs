using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using x10.model;
using x10.ui.metadata;
using x10.ui.composition;

namespace x10.compiler {

  internal class UiDataModel {
    internal Entity Entity { get; private set; }
    internal bool IsMany { get; private set; }
    internal Member Member { get; private set; }

    internal UiDataModel(Entity entity, bool isMany) {
      Entity = entity;
      IsMany = isMany;
    }

    internal UiDataModel(Member member) {
      if (member is Association association) {
        Entity = association.ReferencedEntity;
        IsMany = association.IsMany;
      } else {
        Entity = member.Owner;
        IsMany = false;
      }

      Member = member;
    }

    public override string ToString() {
      return string.Format("Entity: {0}, IsMany: {1}, Member: {2}", Entity?.Name, IsMany, Member?.Name);
    }
  }


  public class UiCompilerPass2 {

    private readonly MessageBucket _messages;
    private readonly UiAttributeReader _attrReader;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllUiDefinitions _allUiDefinitions;

    internal UiCompilerPass2(
      MessageBucket messages,
      UiAttributeReader attributeReader,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions) {

      _messages = messages;
      _attrReader = attributeReader;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allUiDefinitions = allUiDefinitions;
    }

    internal void CompileAllUiDefinitions() {

      // Verify Uniqueness of all UI Component names
      _allUiDefinitions.UiComponentUniquenessCheck();

      foreach (ClassDefX10 definition in _allUiDefinitions.All) {
        InvokePass2Actions(definition);

        XmlElement rootXmlChild = ((XmlElement)definition.XmlElement).Children.SingleOrDefault();
        if (rootXmlChild == null)
          continue;   // Pass 1 already warns about this

        // Walk the XML tree and create a data model based on Instance and UiAttributeValue
        definition.RootChild = ParseRecursively(rootXmlChild);

        // Invoke Pass 2 actions
        CompileRecursively(definition.RootChild, new UiDataModel(definition.ComponentDataModel, definition.IsMany));
      }
    }


    #region Pass 2.1
    private Instance ParseRecursively(XmlElement xmlElement) {
      Instance instance;
      UiAppliesTo? appliesTo;

      if (IsModelReference(xmlElement)) {  // Model Reference (starts with lower-case)
        instance = new InstanceModelRef();
        appliesTo = UiAppliesTo.UiModelReference;
        // TODO: Error if has XML children
      } else if (IsClassDefUse(xmlElement)) {   // Class Definition Use (starts with upper-case)
        instance = new InstanceClassDefUse();
        appliesTo = UiAppliesTo.UiComponentUse;

        ClassDef classDef = _allUiDefinitions.FindDefinitionByNameWithError(xmlElement.Name, xmlElement);
        if (classDef == null)
          return null;

        UiAttributeDefinition primaryAttributeDef = classDef.PrimaryAttributeDef;
        if (primaryAttributeDef == null) {
          if (xmlElement.Children.Count > 0)
            _messages.AddError(xmlElement,
              string.Format("Class Definition '{0}' does not accept child elements (because it does not have a primary Attribute Definition)",
              xmlElement.Name));
        } else {
          UiAttributeValueComplex complexValue = (UiAttributeValueComplex)primaryAttributeDef.CreateAndAddValue(instance, xmlElement);

          // Attribute mandatory but missing
          if (primaryAttributeDef.IsMandatory && xmlElement.Children.Count == 0)
            _messages.AddError(xmlElement,
              string.Format("Class Definition '{0}' must have children, but doesn't",
              xmlElement.Name));

          // Single attribute but multiple children provided
          if (!primaryAttributeDef.IsMany && xmlElement.Children.Count > 1)
            _messages.AddError(xmlElement,
              string.Format("Class Definition '{0}' accepts only a single child, but it has {1}",
              xmlElement.Name, xmlElement.Children.Count));

          foreach (XmlElement xmlChild in xmlElement.Children)
            complexValue.AddInstance(ParseRecursively(xmlChild));
        }
      } else if (IsComplexAttribute(xmlElement)) {
        throw new NotImplementedException();
      } else {
        _messages.AddError(xmlElement,
          string.Format("Xml Element name '{0}' was not recognized as either a Entity *memberName* or a *UiComponentName* or as a *Complex.property*",
          xmlElement.Name));
        return null;
      }

      instance.XmlElement = xmlElement;
      _attrReader.ReadAttributes(xmlElement, appliesTo.Value, instance);

      return instance;
    }


    private bool IsModelReference(XmlElement element) {
      return ModelValidationUtils.IsMemberName(element.Name);
    }

    private bool IsComplexAttribute(XmlElement element) {
      string parentElementName = ((XmlElement)element.Parent)?.Name;
      if (parentElementName == null)
        return false;

      return element.Name.StartsWith(parentElementName + ".");
    }

    private bool IsClassDefUse(XmlElement element) {
      return ModelValidationUtils.IsUiElementName(element.Name);
    }
    #endregion


    #region Pass 2.2
    private void CompileRecursively(Instance element, UiDataModel parentDataModel) {
      if (element == null)
        return;

      InvokePass2Actions(element);
      UiDataModel myDataModel = ResolvePath(parentDataModel, element);

      foreach (UiAttributeValueComplex value in element.AttributeValues.OfType<UiAttributeValueComplex>())
        foreach (Instance childInstance in value.Instances)
          CompileRecursively(childInstance, myDataModel);
    }

    // TODO: Should this code live in UiAttributeDefintions (Pass2)?
    private UiDataModel ResolvePath(UiDataModel dataModel, Instance instance) {
      if (dataModel == null)
        return null;

      string path = instance.Path;

      // It is perfectly valid for a UiChildComponentUse to not specify a path
      if (path == null)
        return dataModel;

      if (instance is InstanceModelRef modelReference) {
        dataModel = AdvancePathByOne(dataModel, path, instance.XmlElement);
        if (dataModel == null)
          return null;
        instance.ModelMember = dataModel.Member;
        instance.RenderAs = ResolveUiComponent(modelReference);
      } else if (instance is InstanceClassDefUse componentUse) {
        XmlBase pathScalar = UiAttributeUtils.FindAttribute(instance, "path").XmlBase;
        string[] pathComponents = path.Split('.');    // Note that path is already validated in UiAttributeDefintions, Pass1.

        foreach (string pathComponent in pathComponents) {
          dataModel = AdvancePathByOne(dataModel, pathComponent, pathScalar);
          if (dataModel == null)
            return null;
        }
      } else
        throw new Exception("Unexpected instance type: " + instance.GetType().Name);

      // TODO: validate the compatibility of the resolved data model and the receiving component:
      // One->One and Many->Many ok, but mismatch is an error.

      // TODO: validate the compatibility of resolved component with respect to the Entity
      // If the component declares and entity, it must match.

      return dataModel;
    }

    private UiDataModel AdvancePathByOne(UiDataModel dataModel, string pathComponent, XmlBase xmlBase) {
      Member member = dataModel.Entity.FindMemberByName(pathComponent);
      if (member == null) {
        _messages.AddError(xmlBase,
          string.Format("Member {0} does not exist on Entity {1}.", pathComponent, dataModel.Entity.Name));
        return null;
      }

      return new UiDataModel(member);
    }

    // For a model reference component (e.g. <myField>), we resolve the actual ui component to use at three levels:
    // 1. The reference itself may dicatate a component
    // 2. The Member may dictate a component
    // 3. Finally, the DataType MUST dictate a component
    private ClassDef ResolveUiComponent(InstanceModelRef modelReference) {
      // If this has been set, it comes from the Pass2 action in UiAttributeDefinitions
      if (modelReference.RenderAs != null)
        return modelReference.RenderAs;

      Member member = modelReference.ModelMember;
      if (member != null) {
        if (member.Ui != null)
          return member.Ui;
        if (member is X10Attribute attribute)
          return attribute.DataType.Ui;
      }

      return null;
    }

    private void InvokePass2Actions(IAcceptsUiAttributeValues component) {
      foreach (UiAttributeValueAtomic value in component.AttributeValues.OfType<UiAttributeValueAtomic>())
        value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, _allUiDefinitions, component, value);
    }
    #endregion
  }
}
