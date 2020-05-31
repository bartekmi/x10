using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
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
      Begin(entity.TreeElement, ".cs");

      WriteLine(0, "using System;");
      WriteLine();
      WriteLine(0, "namespace {0} {", GetNamespace(entity.TreeElement));
      WriteLine(1, "public class {1} : EntityBase {");

      GenerateRegularAttributes(entity.RegularAttributes);
      GenerateDerivedAttributes(entity.DerivedAttributes);
      GenerateAssociations(entity.Associations);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateRegularAttributes(IEnumerable<X10RegularAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in attributes) {
        string dataType = GetDataType(attribute.DataType);
        string varName = "_" + attribute.NameLowerCased;
        string propName = attribute.Name;

        WriteLine(2, "private {0} {1};", dataType, varName);
        WriteLine(2, "public {0} {1} {", dataType, propName);
        WriteLine(3, "get { return {0}; }", varName);
        WriteLine(3, "set {");
        WriteLine(4, "{0} = value;", varName);
        WriteLine(4, "RaisePropertyChange(nameof({0}));", propName);
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }

    private void GenerateDerivedAttributes(IEnumerable<X10DerivedAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Derived Attributes");

      foreach (X10DerivedAttribute attribute in attributes) {
        string dataType = GetDataType(attribute.DataType);

        WriteLine(2, "public {0} {1} {", dataType, attribute.Name);
        WriteLine(3, "get {");
        WriteLine(4, "return {0};", attribute.Formula);
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }

    private void GenerateAssociations(IEnumerable<Association> associations) {
      WriteLine();
      WriteLine(2, "// Associations");

      foreach (Association association in associations) {
        string dataType = association.ReferencedEntity.Name;
        string propName = association.Name;
        string bindablePropName = BindablePropName(association);

        WriteLine(2, "public virtual {0} {1} { get; set; }", dataType, propName);
        WriteLine(2, "public {0} {1} {", dataType, bindablePropName);
        WriteLine(3, "get { return {0}; }", propName);
        WriteLine(3, "set {");
        WriteLine(4, "{0} = value;", propName);
        WriteLine(4, "RaisePropertyChange(nameof({0}));", bindablePropName);
        WriteLine(3, "}");
        WriteLine(2, "}");
      }
    }

    private string BindablePropName(Association association) {
      return association.Name + "Bindable";
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

    #region Utilities
    private string GetNamespace(IParseElement element) {
      return string.Join('.', element.FileInfo.RelativePathComponents);
    }
    #endregion
  }
}
