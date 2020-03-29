using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

namespace x10.model.metadata {
  public class ModelAttributeDefinitionTest {

    private readonly ITestOutputHelper _output;

    public ModelAttributeDefinitionTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void ValidateAllDefinitions() {
      ModelAttributeDefinitionValidator _validator = new ModelAttributeDefinitionValidator();
      List<ValidationError> errors = _validator.Validate(ModelAttributeDefinitions.All);
      ModelAttributeDefinitionValidatorTest.ShowErrors(errors, _output);

      Assert.Empty(errors);
    }
  }
}