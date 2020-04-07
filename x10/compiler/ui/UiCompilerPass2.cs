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

    private MessageBucket _messages;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllUiDefinitions _allUiDefinitions;

    public UiCompilerPass2(
      MessageBucket messages,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions) {

      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allUiDefinitions = allUiDefinitions;
    }

    internal void CompileAllUiDefinitions() {

      // Verify Uniqueness of all UI Component names
      _allUiDefinitions.UiComponentUniquenessCheck();

      // Invoke Pass 2 actions
      foreach (ClassDefX10 definition in _allUiDefinitions.All) {
        InvokePass2Actions(definition);
        CompileRecursively(definition.RootChild, new UiDataModel(definition.ComponentDataModel, definition.IsMany));
      }
    }

    private void CompileRecursively(Instance element, UiDataModel parentDataModel) {
      if (element == null)
        return;

      InvokePass2Actions(element);
      UiDataModel myDataModel = ResolvePath(parentDataModel, element);

      if (element is InstanceClassDefUse componentUse)
        foreach (Instance instance in componentUse.Children) 
          CompileRecursively(instance, myDataModel);
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
  }
}















