using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.compiler;
using x10.model.definition;

namespace x10.gen.sql {
  public class FakeDataGenerationTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public FakeDataGenerationTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
    }

    [Fact]
    public void GenerateLargeData() {
      string INPUT_DIR = "../../../../x10/examples/flexport";
      string OUTPUT_FILE = @"C:\TEMP\x10_flexport_data.sql";

      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.Compile(INPUT_DIR);

      Assert.False(_messages.HasErrors);

      FakeDataGenerator.Generate(entities, new Random(0), OUTPUT_FILE);
    }

    [Fact]
    public void GenerateSingleEntityData() {
      string yaml = @"
name: Entity
datagen_quantity: 10

attributes:
  - name: integer
    dataType: Integer
    datagen_min: 7
    datagen_max: 8

  - name: float
    dataType: Float
    datagen_min: 10.5
    datagen_max: 10.6

  - name: charReplacement
    dataType: String
    datagen_pattern: ~ZZ.DD.LLL~

  - name: percentagePattern
    dataType: String
    datagen_pattern: (50% = <adjective> <noun> | 50% = <noun>)
";

      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.CompileFromYamlStrings(yaml);

      Assert.False(_messages.HasErrors);

      TestUtils.DumpMessages(_messages, _output);
      _output.WriteLine("");

      string sql = FakeDataGenerator.GenerateIntoString(entities, new Random(0));
      _output.WriteLine(sql);

      Assert.Equal(@"TODO", sql);
    }
  }
}
