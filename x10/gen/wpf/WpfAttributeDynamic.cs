using System.IO;
using System;

using x10.formula;
using x10.ui.platform;
using x10.ui.composition;

namespace x10.gen.wpf {
  public class WpfAttributeDynamic : PlatformAttributeDynamic {

    public WpfAttributeDynamic() {}

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      throw new NotImplementedException();
    }
  }
}