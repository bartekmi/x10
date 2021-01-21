using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  public class UiComponentDataCalculatorTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public UiComponentDataCalculatorTest(ITestOutputHelper output) {
      _output = output;
    }

    // *** Use-cases to test ***
    //
    // For all nested cases above - also when limited by "Reduces many to one"
    // Ditto for formulas 

    [Fact]
    public void TopField() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1/>
    <myInteger2/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
myInteger2
";

      RunTest(xml, gql);
    }

    [Fact]
    public void TopDerived() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <derivedSimple/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
";

      RunTest(xml, gql);
    }

    [Fact]
    public void TopDerivedIndirect() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <derivedIndirect/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
myInteger2
";

      RunTest(xml, gql);
    }

    [Fact]
    public void NestedField() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1/>
    <nested.myNestedInteger1/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
nested {
  myNestedInteger1
}
";

      RunTest(xml, gql);
    }

    [Fact]
    public void NestedDerived() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1/>
    <nested.myNestedDerived/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
nested {
  myNestedInteger1
  myNestedInteger2
}
";

      RunTest(xml, gql);
    }

    [Fact]
    public void DataFromFormula() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1 visible='=myInteger2 > 7'/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
myInteger2
";

      RunTest(xml, gql);
    }

    [Fact]
    public void DataFromFormulaNested() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1 visible='=nested.myNestedDerived > 7'/>
  </Group>
</MyClassDef>
";

      string gql = @"
myInteger1
nested {
  myNestedInteger1
  myNestedInteger2
}
";

      RunTest(xml, gql);
    }

    private void RunTest(string xml, string expectedGql) {
      TestBasicUiLibrary basicLib = new TestBasicUiLibrary(_output);
      ClassDefX10 classDef = basicLib.CompileClassDef(xml);
      MemberWrapper wrapper = UiComponentDataCalculator.ExtractData(classDef);

      string actualGql = wrapper.PrintGraphQL(0);

      Assert.Equal(expectedGql.Trim(), actualGql.Trim());
    }
  }
}