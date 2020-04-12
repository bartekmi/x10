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
        definition.RootChild = ParseInstance(rootXmlChild);

        // Invoke Pass 2 actions
        CompileRecursively(definition.RootChild, new UiDataModel(definition.ComponentDataModel, definition.IsMany));
      }
    }

    #region Pass 2.1 - Build the Instance/AttributeValue tree

    private Instance ParseInstance(XmlElement xmlElement) {
      if (IsModelReference(xmlElement)) {
        InstanceModelRef instance = new InstanceModelRef() {
          XmlElement = xmlElement,
        };
        _attrReader.ReadAttributes(xmlElement, UiAppliesTo.UiModelReference, instance);
        return instance;
      } else if (IsClassDefUse(xmlElement)) {
        InstanceClassDefUse instance = ParseClassDefInstance(xmlElement);
        if (instance != null)
          _attrReader.ReadAttributes(xmlElement, UiAppliesTo.UiComponentUse, instance);
        return instance;
      } else {
        _messages.AddError(xmlElement, "Expecting either a Model Reference (e.g. <name\\>) or a Component Reference (e.g. <TextField path='name'\\> but got neither.");
        return null;
      }
    }

    private InstanceClassDefUse ParseClassDefInstance(XmlElement element) {
      ClassDef classDef = _allUiDefinitions.FindDefinitionByNameWithError(element.Name, element);
      if (classDef == null)
        return null;    // Error provided by FindDefinitionByNameWithError() above

      InstanceClassDefUse instance = new InstanceClassDefUse(classDef);

      List<XmlElement> primaryAtributeXmls = new List<XmlElement>();
      foreach (XmlElement xmlChild in element.Children) {
        if (IsComplexAttribute(xmlChild, out string attributeName)) {
          UiAttributeDefinition attrDefinition = classDef.FindComplexAttributeWithError(attributeName, _messages, element);
          if (attrDefinition == null) {
            _messages.AddError(xmlChild,
              string.Format("Complex Attribute '{0}' does not exist on Component '{1}'",
              attributeName, classDef.Name));
            continue;
          }

          if (attrDefinition is UiAttributeDefinitionComplex complexAttrDef) {
            if (xmlChild.Children.Count == 0) {
              _messages.AddWarning(xmlChild, "Empty Complex Attribute");
              continue;
            }
            ParseComplexAttribute(instance, xmlChild.Children, complexAttrDef);
          } else {
            _messages.AddError(xmlChild,
              string.Format("Atomic Attribute '{0}' of Component '{1}' found where Complex Attribute expected.",
              attributeName, classDef.Name));
            continue;
          }
        } else if (IsModelReference(xmlChild) || IsClassDefUse(xmlChild))
          primaryAtributeXmls.Add(xmlChild);
      }

      if (primaryAtributeXmls.Count > 0) 
        ParseComplexAttribute(instance, primaryAtributeXmls, classDef.PrimaryAttributeDef);

      return instance;
    }

    private void ParseComplexAttribute(Instance owner, List<XmlElement> children, UiAttributeDefinitionComplex attrDefinition) {
      UiAttributeValueComplex complexValue = (UiAttributeValueComplex)attrDefinition.CreateValueAndAddToOwner(owner, children.First().Parent);

      foreach (XmlElement child in children) {
        Instance instance = ParseInstance(child);
        if (instance != null)
          complexValue.AddInstance(instance);
      }
    }

    #region Helpers
    private bool IsModelReference(XmlElement element) {
      return ModelValidationUtils.IsMemberName(element.Name);
    }

    private bool IsComplexAttribute(XmlElement element, out string attributeName) {
      attributeName = null;
      string parentElementName = ((XmlElement)element.Parent)?.Name;
      if (parentElementName == null)
        return false;

      string prefix = parentElementName + ".";
      if (!element.Name.StartsWith(prefix) || element.Name.Length <= prefix.Length)
        return false;

      attributeName = element.Name.Substring(prefix.Length);
      return true;
    }

    private bool IsClassDefUse(XmlElement element) {
      return ModelValidationUtils.IsUiElementName(element.Name);
    }
    #endregion

    #endregion

    #region Pass 2.2 - Resolve Paths, Resolve "Render-As" components, Read Attributes
    private void CompileRecursively(Instance instance, UiDataModel parentDataModel) {
      if (instance == null)
        return;

      InvokePass2Actions(instance);
      UiDataModel myDataModel = ResolvePath(parentDataModel, instance);

      foreach (UiAttributeValueComplex value in instance.ComplexAttributeValues)
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
          string.Format("Member '{0}' does not exist on Entity {1}.", pathComponent, dataModel.Entity.Name));
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

    // TODO: This is dead code right now
    private void ValidateAttributes(Instance instance) {
      ClassDef classDef = instance.RenderAs;
      // Error if mandatory attributes missing
      foreach (UiAttributeDefinition attrDefinition in classDef.AttributeDefinitions)
        if (!instance.AttributeValues.Any(x => x.Definition == attrDefinition))
          if (attrDefinition.IsMandatory) {
            string attrType;
            if (attrDefinition is UiAttributeDefinitionComplex attrComplex)
              if (attrComplex.IsPrimary)
                attrType = "Primary";
              else
                attrType = "Complex";
            else
              attrType = "Atomic";

            _messages.AddError(instance.XmlElement,
              string.Format("Mandatory {0} Attribute '{1}' of Component '{2}' missing.",
              attrType, attrDefinition.Name, classDef.Name));
          }
    }

    private void InvokePass2Actions(IAcceptsUiAttributeValues component) {
      foreach (UiAttributeValueAtomic value in component.AttributeValues.OfType<UiAttributeValueAtomic>())
        value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, _allUiDefinitions, component, value);
    }
    #endregion
  }
}
