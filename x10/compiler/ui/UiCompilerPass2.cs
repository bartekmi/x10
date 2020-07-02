﻿using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.model.definition;
using x10.model;
using x10.ui.metadata;
using x10.ui.composition;
using x10.model.metadata;
using x10.formula;

namespace x10.compiler {

  public class UiCompilerPass2 {

    #region Properties, Constructor, Top Level
    private readonly MessageBucket _messages;
    private readonly UiAttributeReader _attrReader;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllUiDefinitions _allUiDefinitions;
    private readonly FormulaParser _parser;

    private static readonly string[] PASS_2_1_MODEL_REF_ATTRIBUTES = new string[] { ParserXml.ELEMENT_NAME, "ui" };
    private static readonly string[] PASS_2_1_CLASS_DEF_USE_ATTRIBUTES = new string[] { ParserXml.ELEMENT_NAME, "path" };

    internal UiCompilerPass2(
      MessageBucket messages,
      UiAttributeReader attributeReader,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions,
      AllFunctions allFunctions) {

      _messages = messages;
      _attrReader = attributeReader;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allUiDefinitions = allUiDefinitions;
      _parser = new FormulaParser(_messages, _allEntities, _allEnums, allFunctions);
    }

    internal void CompileAllUiDefinitions() {

      // Verify Uniqueness of all UI Component names
      _allUiDefinitions.UiComponentUniquenessCheck();

      foreach (ClassDefX10 definition in _allUiDefinitions.All) {
        InvokePass2Actions(definition);

        XmlElement rootXmlChild = ParseComponentDefinition(definition);
        if (rootXmlChild == null)
          continue;

        // Parse the Root Child
        definition.RootChild = ParseInstance(rootXmlChild, null);
        
        X10DataType rootDataModel = definition.ComponentDataModel == null ?
          X10DataType.NULL :
          new X10DataType(definition.ComponentDataModel, definition.IsMany.Value);

        foreach (UiAttributeValueComplex attribute in definition.ComplexAttributeValues())
          foreach (Instance instance in attribute.Instances)
            CompileRecursively(instance, rootDataModel);

        // Now that we have parsed the State complex attribute, we can safely set the "other variables" of our Parser
        var stateVars = definition.GetStateVariables(_allEnums);
        if (stateVars != null)
          _parser.OtherAvailableVariables = stateVars.ToDictionary(x => x.Variable, x => x.DataType);

        // Walk the XML tree and create a data model based on Instance and UiAttributeValue
        CompileRecursively(definition.RootChild, rootDataModel);
      }
    }

    private XmlElement ParseComponentDefinition(ClassDefX10 definition) {
      XmlElement rootElement = (XmlElement)definition.XmlElement;

      List<XmlElement> primaryAtributeXmls = ParseComplexAndPrimaryAttributes(rootElement, ClassDefNative.UiClassDefClassDef, definition);
      ValidateMandatoryComplexAttributes(ClassDefNative.UiClassDefClassDef, definition);

      // Is there a Primary Complex attribute? If so, parse it.
      if (primaryAtributeXmls.Count == 0) {
        _messages.AddWarning(rootElement,
          "UI Component Definition '{0}' is empty (if it has any complex attributes, these don't count). " +
          " It will not be rendered as a visual component.",
          definition.Name);
        return null;
      } else if (primaryAtributeXmls.Count > 1) {
        _messages.AddError(rootElement,
          "UI Component Definition '{0}' has multiple root-level components",
          definition.Name);
        return null;
      }

      return primaryAtributeXmls.Single();
    }
    #endregion

    #region Pass 2.1 - Build the Instance/AttributeValue tree

    private Instance ParseInstance(XmlElement xmlElement, UiAttributeValueComplex owner) {
      if (IsModelReference(xmlElement)) {
        Instance instance = ParseModelRefInstance(xmlElement, owner);
        return instance;
      } else if (IsClassDefUse(xmlElement)) {
        InstanceClassDefUse instance = ParseClassDefInstance(xmlElement, owner);
        if (instance != null)
          _attrReader.ReadSpecificAttributes(instance, UiAppliesTo.UiComponentUse, PASS_2_1_CLASS_DEF_USE_ATTRIBUTES);
        return instance;
      } else {
        _messages.AddError(xmlElement, "Expecting either a Model Reference (e.g. <name\\>) or a Component Reference (e.g. <TextField path='name'\\> but got neither.");
        return null;
      }
    }

    private Instance ParseModelRefInstance(XmlElement xmlElement, UiAttributeValueComplex owner) {
      Instance modelRefInstance;
      Instance returnInstance;

      ClassDef wrapperClassDef = FindModelRefWrapper(owner);
      if (wrapperClassDef != null) {
        XmlElement fakeXmlElemnt = xmlElement.CloneFileLocation();
        returnInstance = new InstanceClassDefUse(wrapperClassDef, fakeXmlElemnt, owner);
        UiAttributeDefinitionComplex wrapperPrimaryAttr = wrapperClassDef.PrimaryAttributeDef;
        UiAttributeValueComplex primaryAttrValue = wrapperPrimaryAttr.CreateValueAndAddToOwnerComplex(returnInstance, fakeXmlElemnt);
        modelRefInstance = new InstanceModelRef(xmlElement, primaryAttrValue);
        primaryAttrValue.AddInstance(modelRefInstance);
      } else
        returnInstance = modelRefInstance = new InstanceModelRef(xmlElement, owner);

      _attrReader.ReadSpecificAttributes(modelRefInstance, UiAppliesTo.UiModelReference, PASS_2_1_MODEL_REF_ATTRIBUTES);

      return returnInstance;
    }

    // For some components, occurrences of InstanceModelRef are wrapped in another "adapter" 
    // component. Classic case is that model ref's in a Table are wrapped in TableColumn
    private ClassDef FindModelRefWrapper(UiAttributeValueComplex complexAttr) {
      while (complexAttr != null) {
        // If the originally passed instance is already embedded in a wrapper, do not embed it again
        if (_allUiDefinitions.IsWrapper((complexAttr.Owner as Instance)?.RenderAs))
          break;

        ClassDef wrapper = complexAttr.DefinitionComplex.ModelRefWrapperComponent;
        if (wrapper != null)
          return wrapper;

        complexAttr = (complexAttr.Owner as Instance)?.Owner;
      }

      return null;
    }

    private InstanceClassDefUse ParseClassDefInstance(XmlElement xmlElement, UiAttributeValueComplex owner) {
      ClassDef classDef = _allUiDefinitions.FindDefinitionByNameWithError(xmlElement.Name, xmlElement);
      if (classDef == null)
        return null;    // Error provided by FindDefinitionByNameWithError() above

      InstanceClassDefUse instance = new InstanceClassDefUse(classDef, xmlElement, owner);
      if (classDef == ClassDefNative.RawHtml)
        return instance;

      List<XmlElement> primaryAtributeXmls = ParseComplexAndPrimaryAttributes(xmlElement, classDef, instance);

      // Is there a Primary Complex attribute? If so, parse it.
      if (primaryAtributeXmls.Count > 0) {
        UiAttributeDefinitionComplex primaryAttrDef = classDef.PrimaryAttributeDef;
        if (primaryAttrDef == null)
          _messages.AddError(xmlElement, "Class Definition '{0}' does not define a Primary Attribute, yet has child elements.",
            classDef.Name);
        else
          ParseComplexAttribute(instance, primaryAtributeXmls, primaryAttrDef);
      }

      ValidateMandatoryComplexAttributes(classDef, instance);

      return instance;
    }

    private List<XmlElement> ParseComplexAndPrimaryAttributes(XmlElement xmlElement, ClassDef classDef, IAcceptsUiAttributeValues owner) {
      List<XmlElement> primaryAtributeXmls = new List<XmlElement>();
      foreach (XmlElement xmlChild in xmlElement.Children) {
        if (IsComplexAttribute(xmlChild, out string attributeName)) {
          UiAttributeDefinition attrDefinition = classDef.FindAttribute(attributeName);
          if (attrDefinition == null) {
            _messages.AddError(xmlChild,
              "Complex Attribute '{0}' does not exist on Component '{1}'",
              attributeName, classDef.Name);
            continue;
          }

          if (attrDefinition is UiAttributeDefinitionComplex complexAttrDef) {
            if (xmlChild.Children.Count == 0) {
              _messages.AddWarning(xmlChild, "Empty Complex Attribute");
              continue;
            }
            ParseComplexAttribute(owner, xmlChild.Children, complexAttrDef);
          } else {
            _messages.AddError(xmlChild,
              "Atomic Attribute '{0}' of Component '{1}' found where Complex Attribute expected.",
              attributeName, classDef.Name);
            continue;
          }
        } else if (IsModelReference(xmlChild) || IsClassDefUse(xmlChild))
          primaryAtributeXmls.Add(xmlChild);
        else
          _messages.AddError(xmlChild,
            "Expecting either a Model Reference (e.g. <name\\>) or a Component Reference (e.g. <TextField path='name'\\>) " +
            "or a Complex Attribute (e.g. SomeComponent.property) but got {0}.", xmlChild.Name);
      }

      return primaryAtributeXmls;
    }

    private void ValidateMandatoryComplexAttributes(ClassDef classDef, IAcceptsUiAttributeValues owner) {
      foreach (UiAttributeDefinitionComplex complexAttr in classDef.ComplexAttributeDefinitions)
        if (complexAttr.IsMandatory && !owner.AttributeValues.Any(x => x.Definition == complexAttr))
          _messages.AddError(owner.XmlElement,
            "Mandatory {0} Attribute '{1}' of Class Definition '{2}' is missing",
            complexAttr.IsPrimary ? "Primary" : "Complex", complexAttr.Name, classDef.Name);
    }

    private void ParseComplexAttribute(IAcceptsUiAttributeValues owner,
      List<XmlElement> children,
      UiAttributeDefinitionComplex attrComplex) {

      if (attrComplex == null)
        return;

      UiAttributeValueComplex complexValue = attrComplex.CreateValueAndAddToOwnerComplex(owner, children.First().Parent);

      foreach (XmlElement child in children) {
        Instance instance = ParseInstance(child, complexValue);
        if (instance != null)
          complexValue.AddInstance(instance);
      }
    }

    #region Helpers
    private bool IsModelReference(XmlElement element) {
      string name = element.Name;
      if (name.StartsWith(".") || name.EndsWith("."))
        return false;
      string[] pieces = element.Name.Split(".");

      return pieces.Length > 0 &&
        pieces.All(x => ModelValidationUtils.IsMemberName(x));
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
    private void CompileRecursively(Instance instance, X10DataType parentDataModel) {
      if (instance == null)
        return;

      // Process this instance...
      InvokePass2Actions(instance);

      X10DataType myDataModel = ResolvePath(parentDataModel, instance);
      if (instance is InstanceModelRef modelReference) {
        ResolveUiComponent(modelReference);
        _attrReader.ReadAttributesForInstance(instance, PASS_2_1_MODEL_REF_ATTRIBUTES);
      } else if (instance is InstanceClassDefUse)
        _attrReader.ReadAttributesForInstance(instance, PASS_2_1_CLASS_DEF_USE_ATTRIBUTES);
      else
        throw new Exception("Unexpected instance type: " + instance.GetType().Name);

      ValidateRenderAsType(instance);
      ParseAndValidateFormulas(instance, parentDataModel);

      // Recurse...
      foreach (UiAttributeValueComplex value in instance.ComplexAttributeValues()) {
        X10DataType childDataModel = myDataModel != null && value.DefinitionComplex.ReducesManyToOne ?
          myDataModel.ReduceManyToOne() :
          myDataModel;

        foreach (Instance childInstance in value.Instances)
          CompileRecursively(childInstance, childDataModel);
      }
    }

    #region Resolve Path
    private X10DataType ResolvePath(X10DataType dataModel, Instance instance) {
      if (dataModel == null)
        return null;

      string path = instance.Path;

      if (path != null) {   // It is perfectly valid for a UiChildComponentUse to not specify a path
        XmlBase pathScalar;

        if (instance is InstanceModelRef)
          pathScalar = instance.XmlElement;
        else if (instance is InstanceClassDefUse)
          pathScalar = instance.FindAttributeValue("path").XmlBase;
        else
          throw new Exception("Unexpected instance type: " + instance.GetType().Name);

        string[] pathComponents = path.Split('.');    // Note that path is already validated in UiAttributeDefintions, Pass1.

        foreach (string pathComponent in pathComponents) {
          dataModel = AdvancePathByOne(dataModel, pathComponent, pathScalar);
          if (dataModel == null)
            return null;
        }

        instance.ModelMember = dataModel.Member;
      }

      ValidateDataModelCompatibility(dataModel, instance);

      return dataModel;
    }

    private X10DataType AdvancePathByOne(X10DataType dataModel, string pathComponent, XmlBase xmlBase) {

      // Root-Level (Context) paths are indicated by a leading '/'
      if (pathComponent.StartsWith('/')) {
        dataModel = new X10DataType(_allEntities.FindContextEntityWithError(xmlBase), false);
        pathComponent = pathComponent.Substring(1);
      }

      if (dataModel.Entity == null)
        return null;

      if (dataModel.IsMany) {
        _messages.AddError(xmlBase, "Attempt to access member '{0}' in context of {1}", pathComponent, dataModel);
        return null;
      }

      Member member = dataModel.Entity.FindMemberByName(pathComponent);
      if (member == null) {
        _messages.AddError(xmlBase, "Member '{0}' does not exist on '{1}'.", pathComponent, dataModel);
        return null;
      }

      return new X10DataType(member);
    }

    private void ValidateDataModelCompatibility(X10DataType dataModel, Instance instance) {
      ClassDef renderAs = instance.RenderAs;
      if (renderAs == null || dataModel.IsNull)
        return;

      Entity expectedEntity = renderAs.ComponentDataModel;

      // Ensure correct Entity delivered (if applicable)
      if (expectedEntity != null) {
        if (dataModel.Entity == null) {
          // There must have been an error in path somewhere up the chain. Nothing new to report.
        } else if (dataModel.Entity.IsA(expectedEntity)) {
          // All is well - the Entity type handed down by path matches what the component expects
        } else
          _messages.AddError(instance.XmlElement,
            string.Format("Data Type mismatch. Component {0} expects Entity '{1}', but the path is delivering Entity '{2}'",
            renderAs.Name, expectedEntity.Name, dataModel.Entity.Name));
      }

      // Ensure correct atomic type delivered (if applicable)
      if (dataModel.Member is X10Attribute x10attribute) {
        DataType dataTypeProvided = x10attribute.DataType;
        if (renderAs.DataModelType == DataModelType.Scalar)
          if (renderAs.AtomicDataModel != dataTypeProvided)
            _messages.AddError(instance.XmlElement,
              "The component {0} expects {1}, but the path is delivering {2}",
              renderAs.Name, renderAs.AtomicDataModel.Name, dataTypeProvided.Name);
      }

      // Validate the compatibility of the resolved data model and the receiving component:
      // One->One and Many->Many ok, but mismatch is an error.
      if (renderAs.CaresAboutDataModel) {
        string dataModelFromPath = dataModel.Member == null ?
          dataModel.Entity?.Name :
          string.Format("{0}.{1}", dataModel.Entity?.Name, dataModel.Member.Name);

        string entityOrScalarProvided = dataModel.Member is X10Attribute ?
          (dataModel.IsMany ? "values" : "value") :
          (dataModel.IsMany ? "Entities" : "Entity");

        string entityOrScalarExpected = renderAs.DataModelType == DataModelType.Scalar ?
          (renderAs.IsMany.Value ? "values" : "value") :
          (renderAs.IsMany.Value ? "Entities" : "Entity");

        if (renderAs.IsMany.Value && !dataModel.IsMany)
          _messages.AddError(instance.XmlElement,
            string.Format("The component {0} expects MANY {1}, but the path is delivering a SINGLE '{2}' {3}",
            renderAs.Name, entityOrScalarExpected, dataModelFromPath, entityOrScalarProvided));
        else if (!renderAs.IsMany.Value && dataModel.IsMany)
          _messages.AddError(instance.XmlElement,
            string.Format("The component {0} expects a SINGLE {1}, but the path is delivering MANY '{2}' {3}",
            renderAs.Name, entityOrScalarExpected, dataModelFromPath, entityOrScalarProvided));
      }
    }
    #endregion

    // For a model reference component (e.g. <myField>), we resolve the actual ui component to use at three levels:
    // 1. The reference itself may dicatate a component
    // 2. The Member may dictate a component
    // 3. Finally, the DataType MUST dictate a component
    private void ResolveUiComponent(InstanceModelRef modelReference) {
      ClassDef classDefForModelRef = null;
      Member member = modelReference.ModelMember;

      // If this has been set, it comes from the Pass2 action in UiAttributeDefinitions
      if (modelReference.RenderAs != null)
        classDefForModelRef = modelReference.RenderAs;
      else if (member != null) {
        if (member.Ui != null)
          classDefForModelRef = member.Ui;
        else if (member is X10Attribute attribute)
          classDefForModelRef = _allUiDefinitions.FindUiComponentForDataType(attribute, modelReference.XmlElement);
        else if (member is Association)
          _messages.AddError(modelReference.XmlElement, "Could not identify UI Component for Association '{0}' of Entity '{1}'",
            member.Name, member.Owner.Name);
        else
          throw new Exception("Unexpected member type: " + member.GetType().Name);
      }

      modelReference.RenderAs = classDefForModelRef;
    }

    private void ValidateRenderAsType(Instance instance) {
      ClassDef requiredType = instance.Owner?.DefinitionComplex.ComplexAttributeType;
      if (requiredType == null)
        return;

      if (instance.RenderAs != null && !instance.RenderAs.IsA(requiredType))
        _messages.AddError(instance.XmlElement,
         "Complex Attribute value must be of type {0} or inherit from it",
         requiredType.Name);
    }

    #region Formula Parsing and Validation
    private void ParseAndValidateFormulas(Instance instance, X10DataType rootDataModel) {
      foreach (UiAttributeValueAtomic value in instance.AtomicAttributeValues())
        if (value.Formula != null) {
          value.Expression = _parser.Parse(value.XmlBase, value.Formula, rootDataModel);
          ValidateReturnedDataType(value);
        };
    }

    private void ValidateReturnedDataType(UiAttributeValueAtomic value) {
      X10DataType returnedDataType = value.Expression.DataType;
      if (returnedDataType.IsError)
        return;

      X10DataType expectedReturnType = new X10DataType(value.DefinitionAtomic.DataType);

      // Since anything can be converted to String, don't worry about type checking
      if (expectedReturnType.IsString)
        return;

      if (!returnedDataType.Equals(expectedReturnType))
        _messages.AddError(value.XmlBase, "Expected expression to return {0}, but it returns {1}",
          expectedReturnType, returnedDataType);
    }
    #endregion

    private void InvokePass2Actions(IAcceptsUiAttributeValues component) {
      foreach (UiAttributeValueAtomic value in component.AttributeValues.OfType<UiAttributeValueAtomic>())
        value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, _allUiDefinitions, component, value);
    }
    #endregion
  }
}
