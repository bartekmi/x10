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

    public string IntermediateFilePath { get; set; }

    public abstract void Generate(ClassDefX10 classDef);
    public abstract void Generate(Entity entity);
    public abstract void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums);

    public string RootGenerateDir;
    public AllEntities AllEntities;
    public AllEnums AllEnums;
    public AllUiDefinitions AllUiDefinitions;
    public MessageBucket Messages;

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

      if (Directory.Exists(rootGenerateDir))
        Directory.Delete(rootGenerateDir, true);

      foreach (Entity entity in AllEntities.All.Where(x => !x.IsAbstract))
        Generate(entity);

      foreach (ClassDefX10 classDef in AllUiDefinitions.All) {
        PrintIntermediateFile(classDef);
        Generate(classDef);
      }

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

      PlatformClassDef platformClassDef = _platformLibraries
        .Select(x => x.FindComponentByLogicalName(logicalName))
        .FirstOrDefault(x => x != null);

      if (platformClassDef == null) {
        Messages.AddError(instance.XmlElement, "No platform-specific Class Definition for Logical Class {0}",
          instance.ClassDef.Name);
      }

      return platformClassDef;
    }

    #region Write Helpers
    private TextWriter _writer;
    private List<Output> _outputs;

    protected void Begin(FileInfo fileInfo, string extension, bool capitalize = true) {
      string relativePath = AssembleRelativePath(fileInfo, extension, capitalize);
      string absolutePath = Path.Combine(RootGenerateDir, relativePath);
      Begin(absolutePath);
    }

    protected void Begin(string absolutePath) {
      if (_writer != null)
        throw new Exception("Someone before me did not End() after Being()");
      _writer = CreateIntermediateDirs(absolutePath);
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

    protected void Write(string text, bool includeLeadingSpace, params object[] args) {
      text = EscapeSpaceProtectedBraces(text);
      if (includeLeadingSpace)
        text = " " + text;

      _outputs.Add(new OutputWrite(text, args));
    }

    protected void WriteRaw(int level, Action<TextWriter, int> writeFunc) {
      using (TextWriter writer = new StringWriter()) {
        writeFunc(writer, level);
        _outputs.Add(new OutputWrite(writer.ToString()));
      }
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

    public void AddPlaceholder(Output placeholder) {
      _outputs.Add(placeholder);
    }

    internal static string Spacer(int level) {
      return new string(' ', level * 2);
    }
    #endregion

    #region Output
    public abstract class Output {
      public abstract void Write(TextWriter writer);
    }

    class OutputWrite : Output {
      private string _text;
      private object[] _args;
      internal OutputWrite(string text, object[] args = null) {
        _text = text;
        _args = args;
      }
      public override void Write(TextWriter writer) {
        if (_args == null)
          writer.Write(_text);
        else
          writer.Write(_text, _args);
      }
    }

    class OutputWriteLine : Output {
      public override void Write(TextWriter writer) {
        writer.WriteLine();
      }
    }

    public class OutputPlaceholder : Output {
      private int _indent;
      private List<string> _lines = new List<string>();

      internal OutputPlaceholder(int indent) {
        _indent = indent;
      }

      public override void Write(TextWriter writer) {
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

    #region Utilities
    protected static string AssembleRelativePath(FileInfo fileInfo, string extension, bool capitalize) {
      string relativePath = fileInfo.RelativePath;
      string relativeDir = Path.GetDirectoryName(relativePath);
      string filenameNoExt = Path.GetFileNameWithoutExtension(relativePath);
      if (capitalize)
        filenameNoExt = NameUtils.CapitalizeFirstLetter(filenameNoExt);

      return Path.Combine(relativeDir, filenameNoExt + extension ?? "");
    }

    protected IEnumerable<DataTypeEnum> FindLocalEnums(Entity entity) {
      FileInfo fileInfo = entity.TreeElement.FileInfo;
      return AllEnums.All
        .Where(x => x.TreeElement.FileInfo.FilePath == fileInfo.FilePath);
    }

    private StreamWriter CreateIntermediateDirs(string absolutePath) {
      string dir = Path.GetDirectoryName(absolutePath);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);
      return new StreamWriter(absolutePath);
    }
    #endregion

    #region Debugging
    private void PrintIntermediateFile(ClassDefX10 classDef) {
      if (IntermediateFilePath == null)
        return;

      string relativePath = classDef.XmlElement.FileInfo.RelativePath;
      string absolutePath = Path.Combine(IntermediateFilePath, relativePath);

      using (TextWriter writer = CreateIntermediateDirs(absolutePath)) 
        classDef.Print(writer, 0, new PrintConfig() {
          PrintModelMember = true,
        });
    }
    #endregion
  }
}
