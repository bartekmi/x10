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
    internal X10Attribute Attribute { get; private set; }
    internal bool IsMany { get; private set; }

    internal UiDataModel(Entity entity, bool isMany) {
      Entity = entity;
      IsMany = isMany;
    }

    internal UiDataModel(X10Attribute attribute) {
      Entity = attribute.Owner;
      Attribute = attribute;
    }
  }


  public class UiEntityCompilerPass2 {

    public MessageBucket _messages { get; private set; }
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllUiDefinitions _allUiDefinitions;

    public UiEntityCompilerPass2(
      MessageBucket messages,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions) {

      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allUiDefinitions = allUiDefinitions;
    }

    internal void CompileAllEntities() {

      // Verify Uniqueness of all UI Component names
      _allUiDefinitions.UiComponentUniquenessCheck();

      // Invoke Pass 2 actions
      foreach (UiDefinitionX10 definition in _allUiDefinitions.All) {
        InvokePass2Actions(definition);
        RecursiveyInvokePass2(definition.RootChild, new UiDataModel(definition.ComponentDataModel, definition.IsMany));
      }
    }

    private void RecursiveyInvokePass2(UiChild element, UiDataModel dataModel) {
      if (element == null)
        return;

      InvokePass2Actions(element);
      if (element is UiChildComponentUse componentUse)
        foreach (UiChild child in componentUse.Children) {
          UiDataModel childDataModel = ResolvePath(dataModel, child);
          RecursiveyInvokePass2(child, childDataModel);
        }
    }

    // TODO: Should this code live in UiAttributeDefintions (Pass2)
    private UiDataModel ResolvePath(UiDataModel dataModel, UiChild child) {
      if (dataModel == null)
        return null;

      string path = child.Path;

      // It is perfectly valid for a UiChildComponentUse to not specify a path
      if (path == null)
        return dataModel;


      XmlScalar pathScalar = UiAttributeUtils.FindAttribute(child, "path").XmlScalar;

      string[] pathComponents = path.Split('.');    // Note that path is already validated in UiAttributeDefintions, Pass1.

      foreach (string pathComponent in pathComponents) {
        dataModel = AdvancePathByOne(dataModel, pathComponent, pathScalar);
        if (dataModel == null)
          break;
      }

      if (child is UiChildModelReference modelReference)
        child.RenderAs = ResolveUiComponent(modelReference);

      // TODO: validate the compatibility of the resolved data model and the receiving component:
      // One->One and Many->Many ok, but mismatch is an error.

      // TODO: validate the compatibility of resolved component with respect to the Entity
      // If the component declares and entity, it must match.

      return dataModel;
    }

    private UiDataModel AdvancePathByOne(UiDataModel dataModel, string pathComponent, XmlScalar pathScalar) {
      Member member = dataModel.Entity.FindMemberByName(pathComponent);
      if (member == null) {
        _messages.AddError(pathScalar,
          string.Format("Member {0} does not exist on Entity {1}.", pathComponent, dataModel.Entity.Name));
        return null;
      }

      UiDataModel newModel;
      if (member is Association association) {
        newModel = new UiDataModel(association.ReferencedEntity, association.IsMany);
      } else if (member is X10Attribute attribute) {
        newModel = new UiDataModel(attribute);
      } else
        throw new Exception("Unexpected member type: " + member.GetType().Name);

      return newModel;
    }

    // For a model reference component (e.g. <myField>), we resolve the actual ui component to use at three levels:
    // 1. The reference itself may dicatate a component
    // 2. The Member may dictate a component
    // 3. Finally, the DataType MUST dictate a component
    private UiDefinition ResolveUiComponent(UiChildModelReference modelReference) {
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
      foreach (UiAttributeValue value in component.AttributeValues)
        value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, _allUiDefinitions, component, value);
    }
  }
}















