using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.parsing;
using x10.ui.composition;

namespace x10.gen {
  public abstract class CodeGenerator {

    public abstract void Generate(ClassDefX10 classDef);
    public abstract void Generate(Entity entity);

    protected String RootGenerateDir;
    protected AllEntities AllEntities;
    protected AllEnums AllEnums;
    protected AllUiDefinitions AllUiDefinitions;

    protected CodeGenerator(string rootGenerateDir, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions) {
      RootGenerateDir = rootGenerateDir;
      AllEntities = allEntities;
      AllEnums = allEnums;
      AllUiDefinitions = allUiDefinitions;
    }

    public void Generate() {
      foreach (Entity entity in AllEntities.All)
        Generate(entity);

      foreach (ClassDefX10 classDef in AllUiDefinitions.All)
        Generate(classDef);
    }

    #region Write Helpers
    private TextWriter _writer;

    protected void Begin(IParseElement element, string extension) {
      string relativePath = element.FileInfo.RelativePath;
      string relativeDir = Path.GetDirectoryName(relativePath);
      string filenameNoExt = Path.GetFileNameWithoutExtension(relativePath);

      string absolutePath = Path.Combine(RootGenerateDir, relativeDir, filenameNoExt + extension);
      Begin(absolutePath);
    }

    protected void Begin(string filename) {
      if (_writer != null)
        throw new Exception("Someone before me did not End() after Being()");
      string dir = Path.GetDirectoryName(filename);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);
      _writer = new StreamWriter(filename);
    }

    protected void End() {
      _writer.Dispose();
      _writer = null;
    }

    protected void WriteLine(int level, string text, params object[] args) {
      if (_writer == null)
        throw new Exception("Your forgot to Begin()");

      text = " " + text + " ";
      text = text.Replace("{ ", "{{ ").Replace(" }", " }}");
      text.Trim();

      _writer.Write(Spacer(level));
      _writer.WriteLine(text, args);
    }

    protected void WriteLine() {
      _writer.WriteLine();
    }

    private string Spacer(int level) {
      return new string(' ', level * 2);
    }
    #endregion
  }
}
