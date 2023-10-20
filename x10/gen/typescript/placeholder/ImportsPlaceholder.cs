using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using x10.parsing;
using x10.model.definition;
using x10.ui.composition;
using x10.model;
using x10.gen.typescript.generate;

namespace x10.gen.typescript.placeholder {

  public enum ImportLevel {
    ThirdParty,
    Project,
    RelayGenerated,
  }

  public class ImportsPlaceholder : CodeGenerator.Output {

    #region Help Classes and Top Level
    class ImportData {
      internal string ImportName;
      internal string Path;
      internal bool IsType = false;
      internal bool IsDefault = false;
      internal ImportLevel ImportLevel;
      internal int ImportSubLevel = 0;

      // Derived
      internal int CombinedImportLevel => (int)ImportLevel * 1000000 + ImportSubLevel;

      public override bool Equals(object obj) {
        ImportData other = (ImportData)obj;

        return
          other.ImportName == ImportName &&
          other.Path == Path &&
          other.IsType == IsType &&
          other.IsDefault == IsDefault;
      }

      public override int GetHashCode() {
        return
          ImportName.GetHashCode() +
          Path.GetHashCode();
      }
    }

    private List<ImportData> _imports = new List<ImportData>();
    private string _generatedCodeSubdir;
    private string _appContextImport;
    private string _reactLibImport;

    // generatedCodeSubdir is the directory into which code is generated RELATIVE TO "x10_generated".
    // We will pre-pend this to all paths accessing generated components
    public ImportsPlaceholder(string generatedCodeSubdir, string appContextImport, string reactLibImport) {
      _generatedCodeSubdir = generatedCodeSubdir;
      _appContextImport = appContextImport;
      _reactLibImport = reactLibImport;
    }
    #endregion

    #region Import Default
    public void ImportDefault(string pathNoExtension, ImportLevel level) {
      string filename = Path.GetFileNameWithoutExtension(pathNoExtension);

      _imports.Add(new ImportData() {
        ImportName = filename,
        Path = pathNoExtension,
        ImportLevel = level,
        IsDefault = true,
      });
    }

    public void ImportDefault(IAcceptsUiAttributeValues uiObject, string appendToName = null) {
      string path = ToPathNoExtension(uiObject.XmlElement);
      if (appendToName != null)
        path += appendToName;

      ImportDefault(path, ImportLevel.Project);
    }

    public void ImportDefaultFromReactLib(string pathNoExtension) {
      string withReactLibDir = string.Format("{0}/{1}", _reactLibImport, pathNoExtension);
      ImportDefault(withReactLibDir, ImportLevel.ThirdParty);
    }
    #endregion

    #region Import (non-default)
    public void Import(string functionOrConstant, string pathNoExtension, ImportLevel level) {
      _imports.Add(new ImportData() {
        ImportName = functionOrConstant,
        Path = pathNoExtension,
        ImportLevel = level,
      });
    }

    public void ImportFromReactLib(string functionOrConstant, string path) {
      Import(functionOrConstant, string.Format("{0}/{1}", _reactLibImport, path), ImportLevel.ThirdParty);
    }

    public void Import(string functionOrConstant, IAcceptsModelAttributeValues entity) {
      string path = ToPathNoExtension(entity.TreeElement);
      Import(functionOrConstant, path, ImportLevel.Project);
    }

    public void Import(string functionOrConstant, IAcceptsUiAttributeValues uiObject) {
      string path = ToPathNoExtension(uiObject.XmlElement);
      Import(functionOrConstant, path, ImportLevel.Project);
    }
    #endregion

    #region Import Type
    public void ImportType(string type, string path, ImportLevel level) {
      _imports.Add(new ImportData() {
        ImportName = type,
        Path = path,
        ImportLevel = level,
        IsType = true,
      });
    }

    public void ImportTypeFromReactLib(string type, string path) {
      ImportType(type, string.Format("{0}/{1}", _reactLibImport, path), ImportLevel.ThirdParty);
    }

    public void ImportType(Entity model) {
      ImportType(model.Name, model);
    }

    public void ImportType(string type, IAcceptsModelAttributeValues entity) {
      string path = ToPathNoExtension(entity.TreeElement);
      ImportType(type, path, ImportLevel.Project);
    }
    #endregion

    #region Special Imports
    public void ImportDerivedAttributeFunction(X10DerivedAttribute derivedAttribute) {
      Import(TypeScriptCodeGenerator.DerivedAttrFuncName(derivedAttribute), derivedAttribute.Owner);
    }

    public void ImportCreateDefaultFunc(Entity model) {
      Import(TypeScriptCodeGenerator.CreateDefaultFuncName(model), model);
    }

    public void ImportCalculateErrorsFunc(Entity model) {
      Import(TypeScriptCodeGenerator.CalculateErrorsFuncName(model), model);
    }

    public void ImportAppContext() {
      ImportReact();
      Import("AppContext", _appContextImport, ImportLevel.Project);
    }

    public void ImportReact() {
      _imports.Add(new ImportData() {
        ImportName = "* as React",
        Path = "react",
        ImportLevel = ImportLevel.ThirdParty,
        IsDefault = true,
      });
    }

    // If function.ImportDir is null, this is assumed to be in react_lib
    public void ImportFunction(Function function) {
      string functionName = TypeScriptCodeGenerator.FunctionName(function);
      if (function.ImportDir == null)
        ImportDefaultFromReactLib("utils/" + functionName);
      else {
        string path = string.Format("{0}/{1}", function.ImportDir, functionName);
        ImportDefault(path, ImportLevel.Project);
      }
    }
    #endregion

    #region Write Implementation
    public override void Write(TextWriter writer) {
      foreach (ImportData import in _imports) {
        if (import.Path.StartsWith("latitude"))
          import.ImportSubLevel = 1;
        if (import.Path.StartsWith(_reactLibImport))
          import.ImportSubLevel = 2;
      }

      IEnumerable<IGrouping<int, ImportData>> orderedImportGroups = _imports
        .Distinct()
        .GroupBy(x => x.CombinedImportLevel)
        .OrderBy(x => x.Key);

      foreach (IGrouping<int, ImportData> group in orderedImportGroups) {
        WriteGroup(writer, group);
        writer.WriteLine();
      }

      writer.WriteLine();
    }

    private static void WriteGroup(TextWriter writer, IEnumerable<ImportData> importSubLevelGroup) {
      IEnumerable<IGrouping<string, ImportData>> orderedImportGroups = importSubLevelGroup
        .Distinct()
        .GroupBy(x => x.Path)
        .OrderBy(x => x.Key);

      foreach (IGrouping<string, ImportData> group in orderedImportGroups) {
        ImportData defaultImport = group.SingleOrDefault(x => x.IsDefault);
        IEnumerable<ImportData> nonDefaultImports = group.Where(x => !x.IsDefault);
        bool hasNonDefaults = nonDefaultImports.Count() > 0;

        writer.Write("import ");
        if (defaultImport != null) {
          writer.Write(defaultImport.ImportName);
          if (hasNonDefaults)
            writer.Write(", ");
        }

        if (hasNonDefaults) {
          writer.Write("{ ");
          bool isFirst = true;

          foreach (ImportData import in nonDefaultImports.OrderBy(x => x.IsType).ThenBy(x => x.ImportName)) {
            if (!isFirst) writer.Write(", ");
            if (import.IsType) writer.Write("type ");
            writer.Write(import.ImportName);
            isFirst = false;
          }

          writer.Write(" }");
        }

        writer.WriteLine(" from '{0}';", group.Key);
      }
    }
    #endregion

    #region Utils

    private string ToPathNoExtension(IParseElement parseElement) {
      string pathNoExtension = parseElement.FileInfo.RelativePathNoExtension;
      if (_generatedCodeSubdir == null)
        return pathNoExtension;
      return Path.Combine(_generatedCodeSubdir, pathNoExtension);
    }
    #endregion
  }
}