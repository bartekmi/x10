using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;

namespace x10.ui.metadata {
  public class UiLibraryValidatorTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly UiLibraryValidator _validator;

    public UiLibraryValidatorTest(ITestOutputHelper output) {
      _output = output;
      _validator = new UiLibraryValidator(_messages);
    }

    [Fact]
    public void EnsureBaseClassPresent() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative() {
          Name = "MyFunkyIntComponent",
        },
      });

      RunTest(library, "Inherits-From parent of Class Definition MyFunkyIntComponent is not defined");
    }

    [Fact]
    public void EnsureNoDuplicateAttributes() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative() {
          Name = "MyDerived",
          InheritsFromName = "MyBase",
          LocalAttributeDefinitions = new UiAttributeDefinition[] {
            new UiAttributeDefinitionAtomic() {
              Name = "myDuplicate",
            },
          },
        },
        new ClassDefNative() {
          Name = "MyBase",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions =new UiAttributeDefinition[] {
            new UiAttributeDefinitionAtomic() {
              Name = "myDuplicate",
            },
            new UiAttributeDefinitionAtomic() {
              Name = "myUnique",
            },
          },
        },
      });

      RunTest(library, "The following attributes of MyDerived are non-unique: myDuplicate");
    }

    [Fact]
    public void EnsureMaxOnePrimaryAttribute() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative() {
          Name = "MyClassDef",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions =new UiAttributeDefinition[] {
            new UiAttributeDefinitionAtomic() {
              Name = "myAttr1",
              IsPrimary = true,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "myAttr2",
              IsPrimary = true,
            },
          },
        },
      });

      RunTest(library, "MyClassDef contains more than one Primary Attribute: myAttr1, myAttr2");
    }

    [Fact]
    public void EnsureNoCircularInheritance() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative() {
          Name = "A",
          InheritsFromName = "C",
        },
        new ClassDefNative() {
          Name = "B",
          InheritsFromName = "A",
        },
        new ClassDefNative() {
          Name = "C",
          InheritsFromName = "B",
        },
      });

      RunTest(library,
        "A is involved in a circular inheritance dependency",
        "B is involved in a circular inheritance dependency",
        "C is involved in a circular inheritance dependency");
    }

    [Fact]
    public void EnsureNoCircularInheritancePontsToSelf() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative() {
          Name = "A",
          InheritsFromName = "A",
        },
      });

      RunTest(library,
        "A is involved in a circular inheritance dependency");
    }

    #region Utils
    private void RunTest(UiLibrary library, params string[] expectedErrorMessages) {
      _validator.HydrateAndValidate(library);

      TestUtils.DumpMessages(_messages, _output);

      foreach (string expectedErrorMessage in expectedErrorMessages) {
        CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
        if (message == null)
          _output.WriteLine("Missing error message: " + expectedErrorMessage);
        Assert.NotNull(message);
      }
    }
    #endregion
  }
}