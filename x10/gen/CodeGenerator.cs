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

    private IEnumerable<PlatformLibrary> _platformLibraries;

    public void Generate(
      MessageBucket messages,
      string rootGenerateDir,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions,
      IEnumerable<PlatformLibrary> platformLibraries) {

      Messages = messages;
      RootGenerateDir = rootGenerateDir;
      AllEntities = allEntities;
      AllEnums = allEnums;
      AllUiDefinitions = allUiDefinitions;
      _platformLibraries = platformLibraries;


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
    private List<Output> _outputs;

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
      _outputs = new List<Output>();
    }

    protected void End() {
      foreach (Output output in _outputs)
        output.Write(_writer);

      _writer.Dispose();
      _writer = null;
      _outputs = null;
    }

    public void WriteLine(int level, string text, params object[] args) {
      WritePrivate(level, text, args);
      WriteLine();
    }

    // Calling this has the same effect as WriteLine(...), but if WriteLineClose() is called
    // BEFORE any other calls to WriteLine(), the contents of both WriteLineMaybe() and WriteLineClose()
    // will be printed on the same line
    private bool _writingLineMaybe = false;
    protected void WriteLineMaybe(int level, string text, params object[] args) {
      WritePrivate(level, text, args);
      _writingLineMaybe = true;
    }

    private void WritePrivate(int level, string text, params object[] args) {
      if (_writer == null)
        throw new Exception("Your forgot to Begin()");

      if (_writingLineMaybe)
        WriteLine();

      text = EscapeSpaceProtectedBraces(text);

      _outputs.Add(new OutputWrite(Spacer(level)));
      _outputs.Add(new OutputWrite(text, args));

      _writingLineMaybe = false;
    }

    internal static string EscapeSpaceProtectedBraces(string text) {
      text = " " + text + " ";
      text = text.Replace("{ ", "{{ ").Replace(" }", " }}");
      return text.Trim();
    }

    protected void WriteLineClose(int level, string text, params object[] args) {
      if (_writingLineMaybe) {
        _writingLineMaybe = false;
        _outputs.Add(new OutputWrite(text, args));
      } else
        WritePrivate(level, text, args);

      WriteLine();
    }

    public void WriteLine() {
      _outputs.Add(new OutputWriteLine());
    }

    public OutputPlaceholder CreatePlaceholder(int indent) {
      OutputPlaceholder placeholder = new OutputPlaceholder(indent);
      _outputs.Add(placeholder);
      return placeholder;
    }

    internal static string Spacer(int level) {
      return new string(' ', level * 2);
    }
    #endregion

    #region Output
    public abstract class Output {
      internal abstract void Write(TextWriter writer);
    }

    class OutputWrite : Output {
      private string _text;
      private object[] _args;
      internal OutputWrite(string text, object[] args = null) {
        _text = text;
        _args = args;
      }
      internal override void Write(TextWriter writer) {
        if (_args == null)
          writer.Write(_text);
        else
          writer.Write(_text, _args);
      }
    }

    class OutputWriteLine : Output {
      private string _text;
      internal override void Write(TextWriter writer) {
        writer.WriteLine();
      }
    }

    public class OutputPlaceholder : Output {
      private int _indent;
      private List<string> _lines = new List<string>();

      internal OutputPlaceholder(int indent) {
        _indent = indent;
      }

      internal override void Write(TextWriter writer) {
        string spacer = CodeGenerator.Spacer(_indent);
        foreach (string line in _lines.Distinct().OrderBy(x => x)) {
          writer.Write(spacer);
          writer.WriteLine(line);
        }
      }

      public void WriteLine(string text, params object[] args) {
        text = CodeGenerator.EscapeSpaceProtectedBraces(text);
        _lines.Add(string.Format(text, args));
      }
    }
    #endregion

  }
}
