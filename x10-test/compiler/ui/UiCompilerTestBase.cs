using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;
using x10.ui.composition;

namespace x10.compiler {
  public class UiCompilerTestBase {
    protected readonly ITestOutputHelper _output;
    protected readonly MessageBucket _messages = new MessageBucket();
    protected readonly AllEntities _allEntities;
    protected readonly AllEnums _allEnums;
    protected readonly UiCompilerPass1 _compilerPass1;

    protected UiCompilerTestBase(ITestOutputHelper output) {
      _output = output;

      _allEnums = new AllEnums(_messages);
      _allEntities = CreateEntities();

      UiAttributeReader attrReader = new UiAttributeReader(_messages, _allEntities, _allEnums);
      _compilerPass1 = new UiCompilerPass1(_messages, attrReader);
    }


    private AllEntities CreateEntities() {
      // At this point, allEnums is unused
      AllEntities allEntities = TestUtils.EntityCompile(_messages, _allEnums, new string[] {
        @"
name: Building
description: dummy
attributes:
  - name: name
    description: dummy
    dataType: String
  - name: apartmentCount
    description: dummy
    dataType: Integer
  - name: ageInYears
    description: dummy
    dataType: Integer
    ui: MyAverageIntComponent
associations:
  - name: apartments
    description: dummy
    dataType: Apartment
    many: true
",
        @"
name: Apartment
description: dummy
attributes:
  - name: number
    description: dummy
    dataType: Integer
  - name: squreFootage
    description: dummy
    dataType: Float
associations:
  - name: rooms
    description: dummy
    dataType: Room
    many: true
",
        @"
name: Room
description: dummy
attributes:
  - name: name
    description: dummy
    dataType: String
  - name: paintColor
    description: dummy
    dataType: String
",
      });

      if (_messages.HasErrors) {
        TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);
        throw new Exception("Entities did not load cleanly - see output");
      }

      return allEntities;
    }
  }
}
