using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;

namespace x10.gen.wpf {
  public class WpfCodeGenerator : CodeGenerator {

    private readonly string _defaultNamespace;

    public WpfCodeGenerator(string rootGenerateDir, string defaultNamespace, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions)
      : base(rootGenerateDir, allEntities, allEnums, allUiDefinitions) {
      _defaultNamespace = defaultNamespace;
    }

    #region Generate XAML, etc
    public override void Generate(ClassDefX10 classDef) {
      GenerateXamlFile(classDef);
      GenerateXamlCsFile(classDef);
      GenerateViewModelFile(classDef);
      GenerateViewModelCustomFile(classDef);
    }

    private void GenerateXamlFile(ClassDefX10 classDef) {
    }

    private void GenerateXamlCsFile(ClassDefX10 classDef) {
    }

    private void GenerateViewModelFile(ClassDefX10 classDef) {
    }

    private void GenerateViewModelCustomFile(ClassDefX10 classDef) {
    }
    #endregion

    #region Generate Models
    public override void Generate(Entity entity) {
      Begin(entity.TreeElement.FileInfo, ".cs");

      WriteLine(0, "using System;");
      WriteLine();

      WriteLine(0, "using wpf_lib.lib;");
      WriteLine(0, "using wpf_lib.lib.attributes;");
      WriteLine();

      GenerateExtraUsings(entity);

      WriteLine(0, "namespace {0} {", GetNamespace(entity.TreeElement));
      WriteLine();
      GenerateLocalEnums(entity.TreeElement.FileInfo);
      WriteLine();
      WriteLine(1, "public class {0} : EntityBase {", entity.Name);

      GenerateRegularAttributes(entity.RegularAttributes);
      GenerateDerivedAttributes(entity.DerivedAttributes);
      GenerateAssociations(entity.Associations);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateExtraUsings(Entity entity) {
      IEnumerable<string> imports = entity.Associations
        .Select(x => x.ReferencedEntity)
        .Select(x => GetNamespace(x.TreeElement))
        .Distinct();

      string localNamespace = GetNamespace(entity.TreeElement);

      foreach (string aNamespace in imports)
        if (aNamespace != localNamespace)
          WriteLine(0, "using {0};", aNamespace);

      if (imports.Count() > 0)
        WriteLine();
    }

    private void GenerateLocalEnums(FileInfo fileInfo) {
      IEnumerable<DataTypeEnum> localEnums = AllEnums.All
        .Where(x => x.TreeElement.FileInfo.FilePath == fileInfo.FilePath);

      foreach (DataTypeEnum theEnum in localEnums)
        GenerateEnum(1, theEnum);
    }

    #region Regular Attributes
    private void GenerateRegularAttributes(IEnumerable<X10RegularAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in attributes) {
        string dataType = GetDataType(attribute.DataType);
        string varName = "_" + attribute.NameLowerCased;
        string propName = attribute.NameUpperCased;

        WriteLine(2, "private {0} {1};", dataType, varName);
        WriteLine(2, "public {0} {1} {", dataType, propName);
        WriteLine(3, "get { return {0}; }", varName);
        WriteLine(3, "set {");
        WriteLine(4, "{0} = value;", varName);
        WriteLine(4, "RaisePropertyChanged(nameof({0}));", propName);
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }
    #endregion

    #region Derived Attributes
    private void GenerateDerivedAttributes(IEnumerable<X10DerivedAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Derived Attributes");

      foreach (X10DerivedAttribute attribute in attributes) {
        string dataType = GetDataType(attribute.DataType);

        WriteLine(2, "public {0} {1} {", dataType, attribute.NameUpperCased);
        WriteLine(3, "get {");
        WriteLine(4, "return {0};", TransformFormula(attribute.Formula));
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }

    private string TransformFormula(string formula) {
      formula = formula.Trim();
      if (formula.StartsWith("="))
        formula = formula.Substring(1);

      // Transofrmations:
      // 1. Capitalize all names
      // 2. Replace '__Context__' with 'AppStatics.Singleton.Context' 
      // 3. Replace single quotes with double

      bool previousWasNameChar = false;
      StringBuilder builder = new StringBuilder();
      foreach (char c in formula) {
        bool isNameChar = char.IsLetterOrDigit(c) || c == '_';
        if (isNameChar && !previousWasNameChar)
          builder.Append(char.ToUpper(c));
        else
          builder.Append(c);
        previousWasNameChar = isNameChar;
      }

      formula = builder.ToString();
      formula = formula.Replace("__Context__", "AppStatics.Singleton.Context");
      formula = formula.Replace("'", "\"");

      return formula;
    }
    #endregion

    #region Associations
    private void GenerateAssociations(IEnumerable<Association> associations) {
      WriteLine();
      WriteLine(2, "// Associations");

      foreach (Association association in associations) {
        string dataType = association.ReferencedEntity.Name;
        string propName = association.NameUpperCased;
        string bindablePropName = BindablePropName(association);

        WriteLine(2, "public virtual {0} {1} { get; set; }", dataType, propName);
        WriteLine(2, "public {0} {1} {", dataType, bindablePropName);
        WriteLine(3, "get { return {0}; }", propName);
        WriteLine(3, "set {");
        WriteLine(4, "{0} = value;", propName);
        WriteLine(4, "RaisePropertyChanged(nameof({0}));", bindablePropName);
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }

    private string BindablePropName(Association association) {
      return association.Name + "Bindable";
    }
    #endregion

    private string GetDataType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "bool";
      if (dataType == DataTypes.Singleton.Date) return "DateTime?";
      if (dataType == DataTypes.Singleton.Float) return "double";
      if (dataType == DataTypes.Singleton.Integer) return "int";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime?";
      if (dataType == DataTypes.Singleton.Money) return "double";
      if (dataType is DataTypeEnum) return dataType.Name;

      throw new Exception("Unknown data type: " + dataType.Name);
    }
    #endregion

    #region Enums

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(0, anEnum);

      End();
    }

    private void GenerateEnum(int level, DataTypeEnum theEnum) {
      WriteLine(level, "public enum {0} {", theEnum.Name);

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        if (enumValue.Label != null)
          WriteLine(level + 1, "[Label(\"{0}\")]", enumValue.Label);
        if (enumValue.IconName != null)
          WriteLine(level + 1, "[Icon(\"{0}\")]", enumValue.IconName);
        WriteLine(level + 1, "{0},", enumValue.ValueUpperCased);
      }

      WriteLine(level, "}");
      WriteLine();
    }
    #endregion

    #region Utilities
    private string GetNamespace(IParseElement element) {
      string theNamespace = string.Join('.', element.FileInfo.RelativeDirComponents);
      theNamespace = _defaultNamespace + "." + theNamespace;
      return theNamespace;
    }
    #endregion
  }
}
