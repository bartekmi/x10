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

      RunTest(library, "MyFunkyIntComponent does not specify Inherits-From");
    }

    [Fact]
    public void EnsureNoDuplicateAttributes() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative(new UiAttributeDefinition[] {
          new UiAttributeDefinitionAtomic() {
            Name = "myDuplicate",
          },
        }) {
          Name = "MyDerived",
          InheritsFromName = "MyBase",
        },
        new ClassDefNative(new UiAttributeDefinition[] {
          new UiAttributeDefinitionAtomic() {
            Name = "myDuplicate",
          },
          new UiAttributeDefinitionAtomic() {
            Name = "myUnique",
          },
        }) {
          Name = "MyBase",
          InheritsFrom = ClassDefNative.Object,
        },
      });

      RunTest(library, "The following attributes of MyDerived are non-unique: myDuplicate");
    }

    [Fact]
    public void EnsureMaxOnePrimaryAttribute() {
      UiLibrary library = new UiLibrary(new List<ClassDef>() {
        new ClassDefNative(new UiAttributeDefinition[] {
          new UiAttributeDefinitionAtomic() {
            Name = "myAttr1",
            IsPrimary = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "myAttr2",
            IsPrimary = true,
          },
        }) {
          Name = "MyClassDef",
          InheritsFrom = ClassDefNative.Object,
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

      RunTest(library, "A is involved in a circular inheritance dependency");
    }

    #region Utils
    private void RunTest(UiLibrary library, string expectedErrorMessage) {
      _validator.HydrateAndValidate(library);

      TestUtils.DumpMessages(_messages, _output);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);
    }
    #endregion
  }
}