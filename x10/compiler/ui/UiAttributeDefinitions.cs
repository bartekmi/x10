using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using x10.parsing;
using x10.model.metadata;
using x10.model;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {

  public static class UiAttributeDefinitions {

    public const string PATH = "path";
    public const string NAME = ParserXml.ELEMENT_NAME;
    public const string URL = "url";

    public static UiAttributeDefinitionAtomic FindAttribute(UiAppliesTo appliesTo, string attributeName) {
      UiAttributeDefinitionAtomic attrDef = All.SingleOrDefault(x => 
        x.AppliesToType(appliesTo) && x.Name == attributeName);

      if (attrDef == null)
        throw new Exception(string.Format("Attribute {0} - applies to {1} - does not exist",
          attributeName, appliesTo));

      return attrDef;
    }

    public static IEnumerable<UiAttributeDefinitionAtomic> AllApplicable(UiAppliesTo appliesTo) {
      return All.Where(x => x.AppliesToType(appliesTo));
    }

    public static List<UiAttributeDefinitionAtomic> All = new List<UiAttributeDefinitionAtomic>() {

      //=========================================================================
      // Calss Definition (ClassDef)
      new UiAttributeDefinitionAtomic() {
        Name = ParserXml.ELEMENT_NAME,
        Description = "The name of the UI Definition (component) being defined",
        AppliesTo = UiAppliesTo.ClassDef,
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
      new UiAttributeDefinitionAtomic() {
        Name = "description",
        Description = "The description of the UI Definition. Used for documentary purposes and int GUI builder tools.",
        AppliesTo = UiAppliesTo.ClassDef,
        DataType = DataTypes.Singleton.String,
        Setter = "Description",
      },
      new UiAttributeDefinitionAtomic() {
        Name = "model",
        Description = "The name of the X10 model that this UIDefinition expects as data",
        AppliesTo = UiAppliesTo.ClassDef,
        DataType = DataTypes.Singleton.String,

        Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
          ClassDef definition = (ClassDef)uiComponent;
          definition.ComponentDataModel = allEntities.FindEntityByNameWithError(xmlScalar.Value.ToString(), xmlScalar);
        },
      },
      new UiAttributeDefinitionAtomic() {
        Name = "many",
        Description = "If true, this UiDefinition expects a list of models (e.g. it's a table)",
        AppliesTo = UiAppliesTo.ClassDef,
        DataType = DataTypes.Singleton.Boolean,
        Setter = "IsMany",
        DefaultValue = false,
      },
      new UiAttributeDefinitionAtomic() {
        Name = URL,
        Description = "For top-level components, this is the top-level string to which the application can jump to show the UI",
        AppliesTo = UiAppliesTo.ClassDef,
        DataType = DataTypes.Singleton.String,
        Setter = "Url",
      },
      new UiAttributeDefinitionAtomic() {
        Name = "query",
        Description = "Analogous to GraphQL query parameters - or SQL query parameters. Narrows the scope of data from a all 'Entities' accessible to user",
        AppliesTo = UiAppliesTo.ClassDef,
        DataType = DataTypes.Singleton.String,
      },


      //=========================================================================
      // UI Component Use
      new UiAttributeDefinitionAtomic() {
        Name = ParserXml.ELEMENT_NAME,
        Description = "The name of the UI Definition (component) being referenced",
        AppliesTo = UiAppliesTo.UiComponentUse,
        DataType = DataTypes.Singleton.String,
        Pass2ActionPre = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
          Instance instance = (Instance)uiComponent;
          instance.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlBase);
        },
      },
      new UiAttributeDefinitionAtomic() {
        Name = PATH,
        Description = "A logical 'path' from the parent data Entity down to a lower-level Member. Use dot (.) to link multiple descending steps - e.g. 'address.city'",
        AppliesTo = UiAppliesTo.UiComponentUse,
        DataType = DataTypes.Singleton.String,
        Setter = "Path",
        Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
          Regex pathRegex = new Regex("/*[a-zA-Z0-9](\\.[a-zA-Z0-9])*");
          if (!pathRegex.IsMatch(xmlScalar.ToString()))
            messages.AddError(xmlScalar,
              string.Format("Illegal path '{0}'. Path must consist of valid member names separated by periods (.). For example: 'name' or 'client.address.zip'"
              , xmlScalar));
        },
      },

      //=========================================================================
      // UI Model Refrence
      new UiAttributeDefinitionAtomic() {
        Name = ParserXml.ELEMENT_NAME,
        Description = "The name of the Entity Member (attribute or association) being referenced",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Setter = "Path",
      },
      new UiAttributeDefinitionAtomic() {
        Name = "ui",
        Description = "The name of a visual UiDefinition which will be used to display this node",
        AppliesTo = UiAppliesTo.UiModelReference,
        DataType = DataTypes.Singleton.String,
        Pass2ActionPre = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
          Instance instance = (Instance)uiComponent;
          instance.RenderAs = allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlBase);
        },
      },
    };
  }
}
