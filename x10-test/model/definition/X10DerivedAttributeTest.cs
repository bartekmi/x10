using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;
using x10.parsing;

namespace x10.model.definition {
  public class X10DerivedAttributeTest {

    private readonly ITestOutputHelper _output;

    public X10DerivedAttributeTest(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void DerivedAttributeDependencyMapTest() {
      string yaml = @"
name: Sample
attributes:
  - name: a
    dataType: Integer
  - name: b
    dataType: Integer
  - name: c
    dataType: Integer
derivedAttributes:
  - name: der1
    dataType: Integer
    formula: =a + b
  - name: der2
    dataType: Integer
    formula: =b + c
  - name: der3
    dataType: Integer
    formula: =b + der2
";
      MessageBucket errors = new MessageBucket();
      AllEntities entities = TestUtils.EntityCompile(errors, new AllEnums(errors), yaml);
      Entity entity = entities.All.Single();

      DerivedAttributeDependencyMap map = DerivedAttributeDependencyMap.BuildMap(entity);

      Member a = entity.FindMemberByName("a");
      Member b = entity.FindMemberByName("b");
      Member c = entity.FindMemberByName("c");

      Assert.Empty(errors.Errors);
      Assert.Equal(new string[] { "der1" }, map.ChildDependencies(a).Select(x => x.Name));
      Assert.Equal(new string[] { "der1", "der2", "der3" }, map.ChildDependencies(b).Select(x => x.Name));
      Assert.Equal(new string[] { "der2", "der3" }, map.ChildDependencies(c).Select(x => x.Name));
    }
  }
}