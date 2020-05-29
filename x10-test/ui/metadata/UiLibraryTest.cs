using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;
using x10.ui.libraries;
using System.IO;

namespace x10.ui.metadata {
  public class UiLibraryTest {
    [Fact]
    public void GenerateBaseLibraryDocs() {
      string filename = @"C:\TEMP\base_ui_library.md";
      using (TextWriter writer = new StreamWriter(filename))
        BaseLibrary.Singleton().GenerateMarkdown(writer);
    }
  }
}
