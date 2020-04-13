using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

namespace x10.model.metadata {
  public class ModelAttributeDefinitionValidatorTest {

    private readonly ITestOutputHelper _output;
    private ModelAttributeDefinitionValidator _validator = new ModelAttributeDefinitionValidator();

    public ModelAttributeDefinitionValidatorTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void ValidatePresenceOfSetter() {
      List<ModelAttributeDefinition> definitions = new List<ModelAttributeDefinition>() {
        new ModelAttributeDefinition() {
          Name = "missing",
          AppliesTo = AppliesTo.Entity | AppliesTo.EnumType,
          Setter = "NoSuchSetter",
        }
      };

      List<ValidationError> errors = _validator.Validate(definitions);
      ShowErrors(errors, _output);

      Assert.Equal(2, errors.Count);

      Assert.Equal("Attribute 'missing': Setter property 'NoSuchSetter' does not exist on type x10.model.definition.Entity", errors.First().ToString());
      Assert.Equal("Attribute 'missing': Setter property 'NoSuchSetter' does not exist on type x10.model.metadata.DataTypeEnum", errors.Last().ToString());
    }

    [Fact]
    public void ValidateNoDuplicates() {
      List<ModelAttributeDefinition> definitions = new List<ModelAttributeDefinition>() {
        new ModelAttributeDefinition() {
          Name = "duplicate",
          AppliesTo = AppliesTo.Entity | AppliesTo.EnumType,
        },
        new ModelAttributeDefinition() {
          Name = "duplicate",
          AppliesTo = AppliesTo.EnumType | AppliesTo.EnumValue 
        }
      };

      List<ValidationError> errors = _validator.Validate(definitions);
      ShowErrors(errors, _output);

      Assert.Single(errors);

      Assert.Equal("Attribute 'duplicate': This attribute is defined multiple times on EnumType", errors.Single().ToString());
    }

    internal static void ShowErrors(List<ValidationError> errors, ITestOutputHelper output) {
      foreach (ValidationError error in errors)
        output.WriteLine(error.ToString());
    }
  }
}