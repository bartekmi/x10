using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.parsing;
using x10.model.definition;

namespace x10.model.metadata {

  public static class ModelAttributeDefinitions {

    public static List<ModelAttributeDefinition> All = new List<ModelAttributeDefinition>() {
      //============================================================================
      // Misc
      new ModelAttributeDefinition() {
        Name = "description",
        Description = "The description of the model. Used for documentary purposes and int GUI builder tools.",
        AppliesTo = AppliesTo.Entity | AppliesTo.Association | AppliesTo.Attribute | AppliesTo.EnumType,
        ErrorSeverityIfMissing = CompileMessageSeverity.Warning,
        MessageIfMissing = "Providing a description is strongly encouraged - the description is used in auto-generated documentation and is key to understanding the overall Data Model",
        DataType = DataTypes.Singleton.String,
        Setter = "Description",
      },
      new ModelAttributeDefinition() {
        Name = "label",
        Description = "The human-readable label to use for this item if the one derived from its nanme is not appropriate",
        AppliesTo = AppliesTo.Entity | AppliesTo.Association | AppliesTo.Attribute | AppliesTo.DerivedAttribute | AppliesTo.EnumType,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinition() {
        Name = "ui",
        Description = "The default UI element to render this type of item",
        AppliesTo = AppliesTo.Entity | AppliesTo.Association | AppliesTo.Attribute | AppliesTo.DerivedAttribute | AppliesTo.EnumType,
        DataType = DataTypes.Singleton.String,
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string value = scalarNode.Value.ToString();
          ModelValidationUtils.ValidateUiElementName(value, scalarNode, messages);
        }
      },
      new ModelAttributeDefinition() {
        Name = "toolTip",
        Description = "The default UI Tool Tipe for this type of item",
        AppliesTo = AppliesTo.Entity | AppliesTo.Association | AppliesTo.Attribute | AppliesTo.DerivedAttribute | AppliesTo.EnumType,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinition() {
        Name = "applicableWhen",
        Description = "A formula for when this Member or Enum Value is applicable. If not applicable, it will be hidden in the UI.",
        AppliesTo = AppliesTo.Association | AppliesTo.Attribute | AppliesTo.DerivedAttribute | AppliesTo.EnumType,
        DataType = DataTypes.Singleton.String,
      },

      //============================================================================
      // Entity
      new ModelAttributeDefinition() {
        Name = "name",
        Description = "The name of the model. Must be upper-case and match the filename of the file where it is defined. This is the tag used everywhere else to refer to the model",
        AppliesTo = AppliesTo.Entity,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.Singleton.String,
        Setter = "Name",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string entityName = scalarNode.Value.ToString();
          bool isNameValid = ModelValidationUtils.ValidateEntityName(entityName, scalarNode, messages);

          // Name must be same as filename
          if (isNameValid) {
            string fileNameNoExtension = Path.GetFileNameWithoutExtension(scalarNode.FileInfo.FilePath);
            if (entityName != fileNameNoExtension)
              messages.AddError(scalarNode,
                string.Format("The name of the entity '{0}' must match the name of the file: {1}",
                entityName, fileNameNoExtension));
          }
        }
      },
      new ModelAttributeDefinition() {
        Name = "inheritsFrom",
        Description = "Optional parent entity name from which this entity 'inherits' all attributes and associations. The classic case is that Mouse and Lion would both inherit from Animal.",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.String,
        Setter = "InheritsFromName",
        
        // Entity name format validation is not needed, as it will be caught when no entity matches
        
        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          Entity entity = (Entity)modelComponent;
          entity.InheritsFrom = allEntities.FindEntityByNameWithError(entity.InheritsFromName, attributeValue);
        },
      },
      new ModelAttributeDefinition() {
        Name = "defaultStringRepresentation",
        Description = @"A formula expressed in terms of the members of this Entity to show 
a default string representation of instances. Can recursively use associations if desired.
Typical use would be if entities are going to be represented on a drop-down.",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.String,

        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          // FUTURE: Validate the formula
        },
      },

      //============================================================================
      // Attribute & Association
      new ModelAttributeDefinition() {
        Name = "name",
        Description = "The name of the attribute or association. Must be lower-case. This is the tag used everywhere else to refer to it",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association | AppliesTo.DerivedAttribute,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.Singleton.String,
        Setter = "Name",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string name = scalarNode.Value.ToString();
          switch (appliesTo) {
            case AppliesTo.Attribute:
            case AppliesTo.DerivedAttribute:
              ModelValidationUtils.ValidateAttributeName(name, scalarNode, messages);
              break;
            case AppliesTo.Association:
              ModelValidationUtils.ValidateAssociationName(name, scalarNode, messages);
              break;
            default:
              throw new Exception("Unexpected appliesTo: " + appliesTo);
          }
        }
      },
      new ModelAttributeDefinition() {
        Name = "mandatory",
        Description = "Specifies whether an Attribute or Association is required. This will affect form validation and UI generation",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Singleton.Boolean,
        Setter = "IsMandatory",
      },
      new ModelAttributeDefinition() {
        Name = "readOnly",
        Description = "If true, this field can never be directly edited by a user. Determines what kind of UI is generated",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Singleton.Boolean,
        Setter = "IsReadOnly",
      },

      //============================================================================
      // Members
      new ModelAttributeDefinition() {
        Name = "placeholderText",
        Description = "Placeholder text for UI components (e.g. the text that go into TextBoxe's before the user enters anything)",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association,
        DataType = DataTypes.Singleton.String,
      },

      //============================================================================
      // Attributes (Regular and Derived)

      // It is crucial that this attribute be listed before the 'default' attribute
      new ModelAttributeDefinition() {
        Name = "dataType",
        Description = "The data type of this attribute",
        AppliesTo = AppliesTo.Attribute | AppliesTo.DerivedAttribute,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.Singleton.String,
        Setter = "DataTypeName",
        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          X10Attribute attr = (X10Attribute)modelComponent;
          attr.DataType = allEnums.FindDataTypeByNameWithError(attr.DataTypeName, attributeValue);
        },
      },
      new ModelAttributeDefinition() {
        Name = "default",
        Description = "Default value for the attribute. This is significant when the user creates new entities of this type",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
        Setter = "DefaultValueAsString",

        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          X10RegularAttribute attr = (X10RegularAttribute)modelComponent;
          if (attr.DataType == null)
            return;

          ModelAttributeValue defaultValue = AttributeUtils.FindAttribute(attr, "default");
          attr.DefaultValue = attr.DataType.Parse(attr.DefaultValueAsString, messages, defaultValue.TreeElement, "default");
        },
      },
      new ModelAttributeDefinition() {
        Name = "formula",
        Description = "Formula that describes the value of this attribute in terms of other attributes (regular or derived)",
        AppliesTo = AppliesTo.DerivedAttribute,
        DataType = DataTypes.Singleton.String,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        Setter = "Formula",
      },

      //============================================================================
      // Associations
      new ModelAttributeDefinition() {
        Name = "dataType",
        Description = "The name of the Entity which this association is pointing to.",
        AppliesTo = AppliesTo.Association,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.Singleton.String,
        Setter = "ReferencedEntityName",

        // Validation is not needed here, as incorrect values will be caught in the second pass
        // of the compilation process.

        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          Association association = (Association)modelComponent;
          association.ReferencedEntity = allEntities.FindEntityByNameWithError(association.ReferencedEntityName, attributeValue);
        },
      },
      new ModelAttributeDefinition() {
        Name = "many",
        Description = "If true, this is a one-to-many association.",
        AppliesTo = AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Singleton.Boolean,
        Setter = "IsMany",
      },
      new ModelAttributeDefinition() {
        Name = "owns",
        Description = "If true, this object owns the child object(s) at the other end of the association.",
        AppliesTo = AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Singleton.Boolean,
        Setter = "Owns",
      },

      //============================================================================
      // Enum Types
      new ModelAttributeDefinition() {
        Name = "name",
        Description = "The name of the enum type. This is how the enum type is referred to everywhere else in the data models. Like any other type, must be upper-case.",
        AppliesTo = AppliesTo.EnumType,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.Singleton.String,
        Setter = "Name",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string name = scalarNode.Value.ToString();
          ModelValidationUtils.ValidateEnumName(name, scalarNode, messages);
        }
      },
      new ModelAttributeDefinition() {
        Name = "default",
        Description = "Default value for any attributes of this type. This is significant when the user creates new entities of this type",
        AppliesTo = AppliesTo.EnumType,
        DataType = DataTypes.Singleton.String,
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string defaultValue = scalarNode.Value.ToString();
          DataTypeEnum theEnum = (DataTypeEnum)modelComponent;
          if (theEnum.EnumValues != null &&
              !theEnum.EnumValues.Any(x => x.Value.ToString() == defaultValue))
            messages.AddError(scalarNode,
              string.Format("The default value '{0}' is not one of the available enum values", defaultValue));
        }
      },

      //============================================================================
      // Enums Values
      new ModelAttributeDefinition() {
        Name = "value",
        Description = "The raw value or symbol for this enum choice. This is what gets stored and communicated in the underlying data. Must be lower-case.",
        AppliesTo = AppliesTo.EnumValue,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,

        // The design intent is that this be allowed to be any data type, but must follow the data type of
        // the owning DataType instance.
        // For now, we are just being a little bit lazy.
        DataType = DataTypes.Singleton.String,

        Setter = "Value",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string value = scalarNode.Value.ToString();
          ModelValidationUtils.ValidateEnumValue(value, scalarNode, messages);
        }
      },
      new ModelAttributeDefinition() {
        Name = "label",
        Description = "The human-friendly label for this enum choice. This is what gets displayed to users on UI elements such as drop-downs. If missing, will be derived from the 'value'",
        AppliesTo = AppliesTo.EnumValue,
        DataType = DataTypes.Singleton.String,
        Setter = "Label",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string label = scalarNode.Value.ToString();
          if (string.IsNullOrWhiteSpace(label))
            messages.AddError(scalarNode,
            "There should never be a blank enum value. If the value is optional, just make the attribute that uses it non-mandatory.");
        }
      },
      new ModelAttributeDefinition() {
        Name = "icon",
        Description = "The name of the icon that correspond to this enum value. Will be used in UI as appropriate.",
        AppliesTo = AppliesTo.EnumValue,
        DataType = DataTypes.Singleton.String,
        Setter = "IconName",
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          string label = scalarNode.Value.ToString();
          // FUTURE - we probably need to introduce the concept of an icon library
        }
      },
    };
  }
}
