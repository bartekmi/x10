using Xunit;

namespace x10.model {
  public class ModelValidationUtilsTest {

    [Fact]
    public void IsMemberName() {
      Assert.True(ModelValidationUtils.IsMemberName("camelCase"));
      Assert.True(ModelValidationUtils.IsMemberName("camelCase123"));
      Assert.True(ModelValidationUtils.IsMemberName("singleword"));
      Assert.True(ModelValidationUtils.IsMemberName("snake_case"));

      Assert.False(ModelValidationUtils.IsMemberName("Upper"));
      Assert.False(ModelValidationUtils.IsMemberName("UpperCamelCase"));
      Assert.False(ModelValidationUtils.IsMemberName("Uppser_Snake_Case"));
    }
  }
}