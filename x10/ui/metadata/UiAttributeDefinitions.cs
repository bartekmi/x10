using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.parsing;
using x10.model.metadata;
using x10.ui.composition;
using x10.model;

namespace x10.ui.metadata {

  public static class UiAttributeDefinitions {

    // Reserved attribute name for the "Name" of the XML node - i.e. the thing
    // that goes in the <> brackets: <MyElement>
    private const string ELEMENT_NAME = "Name";

    public static List<UiAttributeDefinition> All = new List<UiAttributeDefinition>() {

      //=========================================================================
      // UI Definition
      new UiAttributeDefinitionPrimitive() {
        Name = ELEMENT_NAME,
        Description = "The name of the UI Definition (component) being defined",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,

        Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
          string uiDefinitionName = xmlScalar.Value.ToString();
          bool isNameValid = ModelValidationUtils.ValidateUiElementName(uiDefinitionName, xmlScalar, messages);

          // Name must be same as filename
          if (isNameValid) {
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(xmlScalar.FileInfo.FilePath);
            if (uiDefinitionName != fileNameNoExtension)
              messages.AddError(xmlScalar,
                string.Format("The name of the UI Component '{0}' must match the name of the file: {1}",
                uiDefinitionName, fileNameNoExtension));
          }
        },
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "description",
        Description = "The description of the UI Definition. Used for documentary purposes and int GUI builder tools.",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,
        Setter = "Description",
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "model",
        Description = "The name of the X10 model that this UIDefinition expects as data",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,

        Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
          UiDefinition definition = (UiDefinition)uiComponent;
          definition.ComponentDataModel = allEntities.FindEntityByNameWithError(xmlScalar.Value.ToString(), xmlScalar);
        },
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "many",
        Description = "If true, this UiDefinition expects a list of models (e.g. it's a table)",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.Boolean,
        DefaultValue = false,
      },


      //=========================================================================
      // UI Component Use
      new UiAttributeDefinitionPrimitive() {
        Name = ELEMENT_NAME,
        Description = "The name of the UI Definition (component) being referenced",
        AppliesTo = UiAppliesTo.UiComponentUse,
        DataType = DataTypes.Singleton.String,
        Pass2Action = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
          UiChild uiChild = (UiChild)uiComponent;
          uiChild.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlBase);
        },
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "path",
        Description = "A logical 'path' from the parent data Entity down to a lower-level Member. Use dot (.) to link multiple descending steps - e.g. 'address.city'",
        AppliesTo = UiAppliesTo.UiComponentUse,
        DataType = DataTypes.Singleton.String,
        Pass1Action = (messages, allEntities, allEnums, uiComponent, attributeValue) => {
          // TODO: validate path and set the referred-to Member
        },
      },

      //=========================================================================
      // UI Model Refrence
      new UiAttributeDefinitionPrimitive() {
        Name = ELEMENT_NAME,
        Description = "The name of the Entity Member (attribute or association) being referenced",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Pass1Action = (messages, allEntities, allEnums, uiComponent, attributeValue) => {
          // TODO: validate the one-member "path" and set ModelMember
        },
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "ui",
        Description = "The name of a visual UiDefinition which will be used to display this node",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Pass2Action = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
          UiChild uiChild = (UiChild)uiComponent;
          uiChild.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlBase);
        },
      },
    };
  }
}
