using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using x10.compiler;
using x10.formula;
using x10.model;
using x10.model.definition;
using x10.model.libraries;
using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;
using x10.utils;
using FileInfo = x10.parsing.FileInfo;

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

      // Some trivial components might not have a data model - perhaps just text, etc
      if (classDef.ComponentDataModel != null) {
        GenerateViewModelFile(classDef);
        GenerateViewModelCustomFile(classDef);
      }
    }

    private void GenerateXamlFile(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, ".xaml");

      WriteLine(0,
@"<UserControl x:Class=""{0}.{1}""
             xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
             xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml""
             xmlns:mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006""
             xmlns:d = ""http://schemas.microsoft.com/expression/blend/2008""
             xmlns:local = ""{0}""
             xmlns:lib = ""clr-namespace:wpf_lib.lib;assembly=wpf_lib""
             mc:Ignorable = ""d""> ", GetNamespace(classDef.XmlElement), classDef.Name);

      WriteLine(0, "</UserControl>");

      End();
    }

    private void GenerateXamlCsFile(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, ".xaml.cs");

      WriteLine(0, "using System.Windows;");
      WriteLine(0, "using System.Windows.Controls;");
      WriteLine();

      WriteLine(0, "namespace {0} {", GetNamespace(classDef.XmlElement));
      WriteLine(1, "public partial class {0} : UserControl {", classDef.Name);

      GenerateConstructor(classDef);
      GenerateMethods(classDef);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateConstructor(ClassDefX10 classDef) {
      WriteLine(2, "public {0}() {", classDef.Name);
      WriteLine(3, "InitializeComponent();");
      WriteLine(3, "DataContext = new {0}VM(this);", classDef.Name);
      WriteLine(2, "}");
    }

    private void GenerateMethods(ClassDefX10 classDef) {
    }

    private void GenerateViewModelFile(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, "VM.cs");

      WriteLine(0, "using System.Collections.Generic;");
      WriteLine(0, "using System.Windows.Controls;");
      WriteLine();
      WriteLine(0, "using wpf_lib.lib;");   // TODO: Eventually, this should be dynamic

      WriteLine(0, "namespace {0} {", GetNamespace(classDef.XmlElement));
      WriteLine(1, "public partial class {0}VM : ViewModelBase<{1}> {", classDef.Name, classDef.ComponentDataModel.Name);

      GenerateState(classDef);
      GenerateExpression(classDef);
      GenerateValidations(classDef);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateState(ClassDefX10 classDef) {
    }

    private void GenerateExpression(ClassDefX10 classDef) {
    }

    private void GenerateValidations(ClassDefX10 classDef) {
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
      WriteLine(0, "using {0}.functions;", _defaultNamespace);
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
      GenerateToString(entity);
      GenerateCreateFunction(entity);

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
        WriteLine(4, "return {0};", ExpressionToString(attribute.Expression));
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
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

    #region Create Default Entity
    private void GenerateCreateFunction(Entity entity) {
      WriteLine();
      WriteLine(2, "public static {0} Create() {", entity.Name);
      WriteLine(3, "return new {0} {", entity.Name);

      foreach (Member member in entity.Members) {
        ModelAttributeValue value = member.FindAttribute(BaseLibrary.DEFAULT);
        if (value != null) {
          WriteLine(4, "{0} = {1},", member.NameUpperCased, AttributeValueToString(value));
        }
      }

      WriteLine(3, "};");
      WriteLine(2, "}");
    }
    #endregion

    #region Create ToString()
    private void GenerateToString(Entity entity) {
      ModelAttributeValue value = entity.FindAttribute(BaseLibrary.DEFAULT_STRING_REPRESENTATION);
      if (value == null)
        return;

      WriteLine();
      WriteLine(2, "public override string ToString() {");
      WriteLine(3, "return {0};", AttributeValueToString(value));
      WriteLine(2, "}");
    }
    #endregion

    #endregion

    #region Enums

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

      WriteLine(0, "using wpf_lib.lib.attributes;");
      WriteLine();

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

    private string AttributeValueToString(ModelAttributeValue value) {
      if (value.Value != null)
        return WpfGenUtils.TypedLiteralToString(value.Value);
      else if (value.Formula != null)
        return ExpressionToString(value.Expression);
      else
        return "BLANK VALUE";
    }

    private string ExpressionToString(ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      WpfFormulaWriter formulaWriterVisitor = new WpfFormulaWriter(writer);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

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
  }
}
