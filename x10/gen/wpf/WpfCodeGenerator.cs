using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.parsing;
using x10.ui.composition;

namespace x10.gen.wpf {
  public class WpfCodeGenerator : CodeGenerator {

    public WpfCodeGenerator(string rootGenerateDir, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions) 
      : base(rootGenerateDir, allEntities, allEnums, allUiDefinitions) {
    }

    #region Generate XAML, etc
    public override void Generate(ClassDefX10 classDef) {
      GenerateXamlFile(classDef);
      GenerateXamlCsFile(classDef);
      GenerateViewModelFile(classDef);
      GenerateViewModelCustomFile(classDef);
    }

    private void GenerateXamlFile(ClassDefX10 classDef) {
      throw new NotImplementedException();
    }

    private void GenerateXamlCsFile(ClassDefX10 classDef) {
      throw new NotImplementedException();
    }

    private void GenerateViewModelFile(ClassDefX10 classDef) {
      throw new NotImplementedException();
    }

    private void GenerateViewModelCustomFile(ClassDefX10 classDef) {
      throw new NotImplementedException();
    }
    #endregion

    #region Generate Models
    public override void Generate(Entity entity) {
      using (TextWriter writer = CreateTextWriter(entity.TreeElement, ".cs")) {
        writer.WriteLine("using System;");
        writer.WriteLine();
      }
    }
    #endregion
  }
}
