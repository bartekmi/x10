using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using x10.ui.composition;

namespace x10.gen.wpf.codelet {
  public class CodeletRecipee {
    // Comment to be placed above this codelet section
    public string Comment { get; set; }

    public Action<WpfCodeGenerator, Instance> GenerateInXamlCs { get; set; }
    public Action<WpfCodeGenerator, Instance> GenerateInVM { get; set; }
  }
}
