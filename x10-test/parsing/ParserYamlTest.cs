using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace x10.parsing {
  public class ParserYamlTest {
    [Fact]
    public void Parse() {
      Parser parser  =new ParserYaml();
      List<TreeNode> files = parser.RecursivelyParseDirectory("../../../parsing/data");

      Assert.Single(files);
      TreeNode file = files.First();

      Assert.IsType<TreeHash>(file);
      TreeHash root = file as TreeHash;
      Assert.Equal(3, root.Attributes.Count);

      VerifyAttribute(root, "name", "Person", 1, 1, 1, 7);
      VerifyAttribute(root, "description", "A human", 2, 1, 2, 14);

      TreeHash attributes = root.FindHash("attributes");
      Assert.Equal(2, attributes.Attributes.Count);

      TreeHash name = attributes.FindHash("name");
      VerifyAttribute(name, "dataType", "String", 6, 7, 6, 17);
      VerifyAttribute(name, "mandatory", "true", 7, 7, 7, 18);

      TreeHash dateOfBirth = attributes.FindHash("dateOfBirth");
      VerifyAttribute(name, "dataType", "Date", 9, 7, 9, 17);
      VerifyAttribute(name, "mandatory", "true", 10, 7, 10, 18);
    }

    private void VerifyAttribute(TreeHash root, string key, string expectedValue, 
      int keyLine, int keyChar, int valueLine, int valueChar) {

        TreeAttribute attribute = root.FindAttribute(key);
        Assert.NotNull(attribute);

        TreeNode value = attribute.Value;
        Assert.Equal(expectedValue, value.ToString());

        Assert.Equal(keyLine, attribute.LineNumber);
        Assert.Equal(keyChar, attribute.CharacterPosition);
        
        Assert.Equal(valueLine, value.LineNumber);
        Assert.Equal(valueChar, value.CharacterPosition);
    }

    // Just leaving this here as an example of how to use [Theory]
    // [Theory]
    // [InlineData(3)]
    // [InlineData(5)]
    // [InlineData(6)]
    // public void MyFirstTheory(int value) {
    //   Assert.True(IsOdd(value));
    // }
  }
}