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

      UiAttributeReader attrReader = new UiAttributeReader(_messages, _allEntities, _allEnums, null);
      _compilerPass1 = new UiCompilerPass1(_messages, attrReader);
    }


    private AllEntities CreateEntities() {
      model.libraries.BaseLibrary.ICON_DATA_TYPE.EnumValueValues = new string[] { "open", "covered" }; 

      // At this point, allEnums is unused
      AllEntities allEntities = TestUtils.EntityCompile(_messages, _allEnums, new string[] {
        @"
name: Building
description: dummy
attributes:
  - name: id
    description: dummy
    dataType: String
    readOnly: true
  - name: name
    description: dummy
    dataType: String
    label: Building Name
  - name: apartmentCount
    description: dummy
    dataType: Integer
  - name: ageInYears
    description: dummy
    dataType: Integer
    ui: MyAverageIntComponent
  - name: hasUndergroundParking
    description: dummy
    dataType: Boolean

derivedAttributes:
  - name: derived
    description: dummy
    dataType: Integer
    formula: =apartmentCount + 1

associations:
  - name: apartments
    description: dummy
    dataType: Apartment
    many: true
  - name: demoApartment
    description: dummy
    dataType: Apartment
    many: false
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
  - name: balconyType
    description: dummy
    dataType: BalconyType
associations:
  - name: rooms
    description: dummy
    dataType: Room
    many: true

enums:
  - name: BalconyType
    description: dummy
    values: 
      - value: open
        icon: open
      - value: covered
        icon: covered
      - value: none
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
        @"
name: FancyRoom
description: Fancy dummy
inheritsFrom: Room
attributes:
  - name: fancinessQuotient
    description: dummy
    dataType: Integer
",
        @"
name: __Context__
description: Top level context
associations:
  - name: currentUser
    description: Current logged-in user
    dataType: User
",
        @"
name: User
description: A user of the system with log-in credentials
attributes:
  - name: firstName
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
