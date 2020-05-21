using System;
using System.Collections.Generic;
using System.Text;
using x10.compiler;
using x10.model;

namespace x10.gen {
  public abstract class CodeGenerator {
    public abstract void Generate(string outputDir, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions);
  }
}
