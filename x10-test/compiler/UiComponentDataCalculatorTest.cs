using System;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.ui.composition;

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
    public void DataFromFormulaManyCount() {
      string xml = @"
<MyClassDef model='Entity'>
  <Text text='=many.count'/>
</MyClassDef>
";

      string gql = @"
many {
}
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

    [Fact]
    public void IgnoreContextFields() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <Text text='=__Context__.today'/>
    <ageInYears/>
  </Group>
</MyClassDef>
";

      string gql = @"
myDate
";

      RunTest(xml, gql);
    }

    [Fact(Skip = "See Issue https://github.com/bartekmi/x10/issues/40")]
    public void DerivedAttrWithChildAttr() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <fromChild/>
  </Group>
</MyClassDef>
";

      string gql = @"

";

      RunTest(xml, gql);
    }

    [Fact]
    public void RecursivelyContainsMember() {
      string xml = @"
<MyClassDef model='Entity'>
  <Group>
    <myInteger1/>
    <nested.myNestedInteger1/>
  </Group>
</MyClassDef>
";

      MemberWrapper wrapper = ExtractWrapper(xml, out TestBasicEntities basicEntities);
      AllEntities allEntities = basicEntities.AllEntities;

      Member myInteger1 = allEntities.FindMemberByPath("Entity.myInteger1");
      Member nested = allEntities.FindMemberByPath("Entity.nested");
      Member myNestedInteger1 = allEntities.FindMemberByPath("Entity.nested.myNestedInteger1");
      Member many = allEntities.FindMemberByPath("Entity.many");

      Assert.True(wrapper.RecursivelyContainsMember(myInteger1));
      Assert.True(wrapper.RecursivelyContainsMember(nested));
      Assert.True(wrapper.RecursivelyContainsMember(myNestedInteger1));
      Assert.False(wrapper.RecursivelyContainsMember(many));
    }

    private MemberWrapper ExtractWrapper(string xml, out TestBasicEntities basicEntities) {
      TestBasicUiLibrary basicLib = new TestBasicUiLibrary(_output);
      ClassDefX10 classDef = basicLib.CompileClassDef(xml, out basicEntities);
      MemberWrapper wrapper = UiComponentDataCalculator.ExtractData(classDef);

      return wrapper;
    }

    private void RunTest(string xml, string expectedGql) {
      MemberWrapper wrapper = ExtractWrapper(xml, out TestBasicEntities dummy);
      string actualGql = wrapper.PrintGraphQL(0);

      Assert.Equal(expectedGql.Trim(), actualGql.Trim());
    }
  }
}