using System;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model;

namespace x10 {
  internal class TestBasicEntities {

    internal AllEntities AllEntities { get; private set; }
    internal AllEnums AllEnums { get; private set; }

    internal Entity Entity { get; private set; }
    internal Entity NestedEntity { get; private set; }

    private readonly MessageBucket _messsages = new MessageBucket();

    internal TestBasicEntities(ITestOutputHelper output) {
      string entityYaml = @"
name: Entity
attributes:
  - name: myInteger1
    dataType: Integer
  - name: myInteger2
    dataType: Integer
  - name: myFloat
    dataType: Float
  - name: myBoolean
    dataType: Boolean
  - name: myString
    dataType: String
  - name: myDate
    dataType: Date
  - name: myTimestamp
    dataType: Timestamp
  - name: myEnumValue
    dataType: MyEnum
derivedAttributes:
  - name: derivedSimple
    dataType: Integer
    formula: =myInteger1 + 1
  - name: derivedIndirect
    dataType: Integer
    formula: =derivedSimple + myInteger2
associations:
  - name: nested
    dataType: NestedEntity
  - name: many
    dataType: NestedEntity
    many: true
enums:
  - name: MyEnum
    values: one, two, three
";
      string nestedYaml = @"
name: NestedEntity
attributes:
  - name: myNestedInteger1
    dataType: Integer
  - name: myNestedInteger2
    dataType: Integer
derivedAttributes:
  - name: myNestedDerived
    dataType: Integer
    formula: =myNestedInteger1 + myNestedInteger2
";

      AllEnums = new AllEnums(_messsages);
      AllEntities = TestUtils.EntityCompile(_messsages, AllEnums, entityYaml, nestedYaml);

      Entity = AllEntities.FindEntityByName("Entity");
      NestedEntity = AllEntities.FindEntityByName("NestedEntity");

      TestUtils.DumpErrors(_messsages, output);
      Assert.False(_messsages.HasErrors);
    }

  }
}