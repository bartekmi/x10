using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.model.metadata;
using x10.model;
using x10.ui.composition;
using x10.ui.metadata;
using System.Text.RegularExpressions;

namespace x10.compiler {

  public static class UiAttributeDefinitions {

    // Reserved attribute name for the "Name" of the XML node - i.e. the thing
    // that goes in the <> brackets: <MyElement>
    public const string ELEMENT_NAME = "Name";

    public static List<UiAttributeDefinition> All = new List<UiAttributeDefinition>() {

      //=========================================================================
      // UI Definition
      new UiAttributeDefinitionPrimitive() {
        Name = ELEMENT_NAME,
        Description = "The name of the UI Definition (component) being defined",
        AppliesTo = UiAppliesTo.UiDefinition,
        DataType = DataTypes.Singleton.String,
        Setter = "Name",

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
        IsMandatory = true,
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
        Setter = "IsMany",
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
          uiChild.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlScalar);
        },
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "path",
        Description = "A logical 'path' from the parent data Entity down to a lower-level Member. Use dot (.) to link multiple descending steps - e.g. 'address.city'",
        AppliesTo = UiAppliesTo.UiComponentUse,
        DataType = DataTypes.Singleton.String,
        Setter = "Path",
        Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
          Regex pathRegex = new Regex("[a-zA-Z0-9](\\.[a-zA-Z0-9])*");
          if (!pathRegex.IsMatch(xmlScalar.ToString()))
            messages.AddError(xmlScalar,
              string.Format("Illegal path '{0}'. Path must consist of valid member names separated by periods (.). For example: 'name' or 'client.address.zip'"
              , xmlScalar));
        },
      },

      //=========================================================================
      // UI Model Refrence
      new UiAttributeDefinitionPrimitive() {
        Name = ELEMENT_NAME,
        Description = "The name of the Entity Member (attribute or association) being referenced",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Setter = "Path",
      },
      new UiAttributeDefinitionPrimitive() {
        Name = "ui",
        Description = "The name of a visual UiDefinition which will be used to display this node",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Pass2Action = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
          UiChild uiChild = (UiChild)uiComponent;
          uiChild.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlScalar);
        },
      },
    };
  }
}
