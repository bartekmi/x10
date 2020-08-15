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
using x10.ui.platform;
using x10.parsing;

namespace x10.gen {
  public abstract class CodeGenerator {

    public abstract void Generate(ClassDefX10 classDef);
    public abstract void Generate(Entity entity);
    public abstract void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums);

    protected string RootGenerateDir;
    protected AllEntities AllEntities;
    protected AllEnums AllEnums;
    protected AllUiDefinitions AllUiDefinitions;
    protected MessageBucket Messages;

    private readonly IEnumerable<PlatformLibrary> _platformLibraries;

    protected CodeGenerator(MessageBucket messages, string rootGenerateDir, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions,
      IEnumerable<PlatformLibrary> platformLibraries) {
      Messages = messages;
      RootGenerateDir = rootGenerateDir;
      AllEntities = allEntities;
      AllEnums = allEnums;
      AllUiDefinitions = allUiDefinitions;
      _platformLibraries = platformLibraries;
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

    internal PlatformClassDef FindPlatformClassDef(Instance instance) {
      string logicalName = instance.RenderAs.Name;
      return _platformLibraries
        .Select(x => x.FindComponentByLogicalName(logicalName))
        .FirstOrDefault(x => x != null);
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

    public void WriteLine(int level, string text, params object[] args) {
      WriteLinePrivate(level, text, args);
      WriteLine();
    }

    // Calling this has the same effect as WriteLine(...), but if WriteLineClose() is called
    // BEFORE any other calls to WriteLine(), the contents of both WriteLineMaybe() and WriteLineClose()
    // will be printed on the same line
    private bool _writingLineMaybe = false;
    protected void WriteLineMaybe(int level, string text, params object[] args) {
      WriteLinePrivate(level, text, args);
      _writingLineMaybe = true;
    }

    private void WriteLinePrivate(int level, string text, params object[] args) {
      if (_writer == null)
        throw new Exception("Your forgot to Begin()");

      if (_writingLineMaybe)
        WriteLine();

      text = " " + text + " ";
      text = text.Replace("{ ", "{{ ").Replace(" }", " }}");
      text = text.Trim();

      _writer.Write(Spacer(level));
      _writer.Write(text, args);

      _writingLineMaybe = false;
    }

    protected void WriteLineClose(int level, string text, params object[] args) {
      if (_writingLineMaybe) {
        _writingLineMaybe = false;
        _writer.Write(text, args);
      } else
        WriteLinePrivate(level, text, args);

      WriteLine();
    }

    public void WriteLine() {
      _writer.WriteLine();
    }

    private string Spacer(int level) {
      return new string(' ', level * 2);
    }
    #endregion
  }
}
