using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.utils;

namespace x10.gen {
  public abstract class CodeGenerator {

    public abstract void Generate(ClassDefX10 classDef);
    public abstract void Generate(Entity entity);
    public abstract void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums);

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
      foreach (Entity entity in AllEntities.All.Where(x => !x.IsAbstract))
        Generate(entity);

      foreach (ClassDefX10 classDef in AllUiDefinitions.All)
        Generate(classDef);

      GenerateEnumFiles();
    }

    private void GenerateEnumFiles() {
      var enumFileGroups = AllEnums.All.Where(x => x.IsDefinedInEnumsFile)
        .GroupBy(x => x.TreeElement.FileInfo);

      foreach (var enumFileGroup in enumFileGroups)
        GenerateEnumFile(enumFileGroup.Key, enumFileGroup);
    }


    #region Write Helpers
    private TextWriter _writer;

    protected void Begin(FileInfo fileInfo, string extension) {
      string relativePath = fileInfo.RelativePath;
      string relativeDir = Path.GetDirectoryName(relativePath);
      string filenameNoExt = Path.GetFileNameWithoutExtension(relativePath);
      filenameNoExt = NameUtils.CapitalizeFirstLetter(filenameNoExt);

      string absolutePath = Path.Combine(RootGenerateDir, relativeDir, filenameNoExt + extension);
      Begin(absolutePath);
    }

    protected void Begin(string absolutePath) {
      if (_writer != null)
        throw new Exception("Someone before me did not End() after Being()");
      string dir = Path.GetDirectoryName(absolutePath);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);
      _writer = new StreamWriter(absolutePath);
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
      text = text.Trim();

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
