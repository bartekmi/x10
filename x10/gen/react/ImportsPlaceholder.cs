using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using x10.model.definition;
using x10.ui.composition;
using x10.model;

namespace x10.gen.react {
  public class ImportsPlaceholder : CodeGenerator.Output {

    class ImportData {
      internal string ImportName;
      internal string Path;
      internal bool IsType = false;
      internal bool IsDefault = false;

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

    public void Import(string functionOrConstant, string pathNoExtension) {
      _imports.Add(new ImportData() {
        ImportName = functionOrConstant,
        Path = pathNoExtension
      });
    }

    public void ImportDerivedAttributeFunction(X10DerivedAttribute derivedAttribute) {
      Import(ReactCodeGenerator.DerivedAttrFuncName(derivedAttribute), derivedAttribute.Owner);
    }

    public void Import(string functionOrConstant, IAcceptsModelAttributeValues entity) {
      Import(functionOrConstant, entity.TreeElement.FileInfo.RelativePathNoExtension);
    }

    public void ImportDefault(string pathNoExtension) {
      string filename = Path.GetFileNameWithoutExtension(pathNoExtension);

      _imports.Add(new ImportData() {
        ImportName = filename,
        Path = pathNoExtension,
        IsDefault = true,
      });
    }

    public void ImportDefault(IAcceptsUiAttributeValues entity) {
      ImportDefault(entity.XmlElement.FileInfo.RelativePathNoExtension);
    }

    public void ImportType(string type, string path) {
      _imports.Add(new ImportData() {
        ImportName = type,
        Path = path,
        IsType = true,
      });
    }

    public void ImportType(Entity model) {
      ImportType(model.Name, model.TreeElement.FileInfo.RelativePathNoExtension);
    }

    public void ImportCreateDefaultFunc(Entity model) {
      Import(ReactCodeGenerator.CreateDefaultFuncName(model), 
             model.TreeElement.FileInfo.RelativePathNoExtension);
    }

    // Some special cases
    public void ImportAppContext() {
      ImportReact();
      Import("AppContext", "AppContext");
    }

    public void ImportReact() {
      _imports.Add(new ImportData() {
        ImportName = "* as React",
        Path = "react",
        IsDefault = true,
      });
    }

    public void ImportType(string type, IAcceptsModelAttributeValues entity) {
      ImportType(type, entity.TreeElement.FileInfo.RelativePathNoExtension);
    }

    public void ImportFunction(Function function) {
      string functionName = ReactCodeGenerator.FunctionName(function);

      _imports.Add(new ImportData() {
        ImportName = functionName,
        // TODO: In the yaml function definitions, make it possible to specify location of function
        Path = "react_lib/utils/" + functionName,
        IsDefault = true,
      });
    }

    public override void Write(TextWriter writer)  {
      IEnumerable<IGrouping<string, ImportData>> orderedImportGroups = _imports
        .Distinct()
        .GroupBy(x => x.Path)
        .OrderBy(x => x.Key);

      foreach (IGrouping<string, ImportData> group in orderedImportGroups) {
        ImportData defaultImport = group.SingleOrDefault(x => x.IsDefault);
        IEnumerable<ImportData> nonDefaultImports= group.Where(x => !x.IsDefault);
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

      writer.WriteLine();
    }
  }
}