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

namespace x10.gen.sql.primitives {
  public class ExternalDataFileTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public ExternalDataFileTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
    }

    private const string FILE_DIR = @"../../../../data";

    [Fact]
    public void ParseCnCities() {
      ExternalDataFile external = new ExternalDataFile() {
        Path = "cn_cities.csv",
      };

      external.Parse(FILE_DIR);

      Assert.Equal(9, external.ColumnNameToIndex.Count);
      Assert.Equal(0, external.ColumnNameToIndex["city"]);
      Assert.Equal(8, external.ColumnNameToIndex["population_proper"]);

      DataFileRow first = external.GetRow(0);
      Assert.Equal("Shanghai", first.GetValue("city"));
      Assert.Equal("14608512", first.GetValue("population_proper"));

      DataFileRow last = external.GetRow(2188 - 2);
      Assert.Equal("Sangri", last.GetValue("city"));
    }

    [Fact]
    public void ParseTheFiles() {
      TestRead("us_cities.csv", 19, 28890 - 1);
      TestRead("first_name.csv", 2, 6783 - 1);
      TestRead("last_name.csv", 1, 2089 - 1);
    }

    private void TestRead(string path, int expectedColumns, int expectedRows) {
      ExternalDataFile external = new ExternalDataFile() {
        Path = path,
      };

      external.Parse(FILE_DIR);

      Assert.Equal(expectedColumns, external.ColumnNameToIndex.Count);
      Assert.Equal(expectedRows, external.Count);
    }
  }
}
