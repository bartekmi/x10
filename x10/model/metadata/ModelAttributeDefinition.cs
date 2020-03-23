using System.Collections.Generic;

using x10.parsing;

namespace x10.model.metadata {

  public enum AppliesTo {
    Entity = 1,
    Association = 2,
    Attribute = 4,
    EnumValue = 8,
  }

  public class ModelAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }
    public AppliesTo AppliesTo { get; set; }
    public CompileMessageSeverity? ErrorSeverityIfMissing { get; set; }
    public object DefaultIfMissing { get; set; }
    public string MessageIfMissing { get; set; }
    public DataType DataType { get; set; }
    public string Setter { get; set; }

    // Derived
    public bool AppliesToType(AppliesTo type) {
      return (AppliesTo & type) > 0;
    }
  }

  public static class ModelAttributeDefinitions {
    public static List<ModelAttributeDefinition> All = new List<ModelAttributeDefinition>() {
      // Entity
      new ModelAttributeDefinition() {
        Name = "name",
        Description = "The name of the model. Must be upper-case and match the filename of the file where it is defined. This is the tag used everywhere else to refer to the model",
        AppliesTo = AppliesTo.Entity,
        ErrorSeverityIfMissing = CompileMessageSeverity.Error,
        DataType = DataTypes.String,
        Setter = "Name",
      },
      new ModelAttributeDefinition() {
        Name = "description",
        Description = "The description of the model. Used for documentary purposes and int GUI builder tools.",
        AppliesTo = AppliesTo.Entity | AppliesTo.Association | AppliesTo.Attribute,
        ErrorSeverityIfMissing = CompileMessageSeverity.Warning,
        MessageIfMissing = "Providing a description is strongly encouraged - the description is used in auto-generated documentation and is key to understanding the overall Data Model",
        DataType = DataTypes.String,
        Setter = "Description",
      },

      // Attribute & Association
      new ModelAttributeDefinition() {
        Name = "mandatory",
        Description = "Specifies whether an Attribute or Association is required. This will affect form validation and UI generation",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Boolean,
        Setter = "IsMandatory,",
      },
      new ModelAttributeDefinition() {
        Name = "readOnly",
        Description = "If true, this field can never be directly edited by a user. Determines what kind of UI is generated",
        AppliesTo = AppliesTo.Attribute | AppliesTo.Association,
        DefaultIfMissing = false,
        DataType = DataTypes.Boolean,
        Setter = "IsReadOnly",
      },

      // Attribute

      // It is crucial that this attribute be listed before the 'default' attribute
      new ModelAttributeDefinition() {
        Name = "dataType",
        Description = "The data type of this attribute",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.DataType,
        Setter = "DataType",
      },
      new ModelAttributeDefinition() {
        Name = "default",
        Description = "Default value for the attribute. This is significant when the user creates new entities of this type",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.SameAsDataType,
      },
    };
  }
}