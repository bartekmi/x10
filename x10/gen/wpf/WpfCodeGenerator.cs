using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.compiler;
using x10.formula;
using x10.model;
using x10.model.definition;
using x10.model.libraries;
using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;
using static x10.ui.metadata.ClassDefNative;
using x10.utils;
using x10.ui.platform;
using x10.ui;
using x10.gen.wpf.codelet;

namespace x10.gen.wpf {
  public class WpfCodeGenerator : CodeGenerator {

    private readonly string _defaultNamespace;

    // Per-Class Def Data 
    // The following members are only private (as opposed to passed down the call-stack)
    // for convenience. They are re-set with every new ClassDef generation
    private Dictionary<string, ExpBase> _viewModelMethodToExpression = new Dictionary<string, ExpBase>();

    public WpfCodeGenerator(
      MessageBucket messages,
      string rootGenerateDir,
      string defaultNamespace,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions,
      IEnumerable<PlatformLibrary> platformLibraries
      ) : base(messages, rootGenerateDir, allEntities, allEnums, allUiDefinitions, platformLibraries) {
      _defaultNamespace = defaultNamespace;
    }

    #region Generate XAML, XAML.cs, VM (View Model), Custom VM

    public override void Generate(ClassDefX10 classDef) {
      GenerateXamlFile(classDef);
      GenerateXamlCsFile(classDef);

      // Some trivial components might not have a data model - perhaps just text, etc
      if (classDef.ComponentDataModel != null) {
        GenerateViewModelFile(classDef);
        GenerateViewModelCustomFile(classDef);
      }
    }

    #region .xaml File
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

      _viewModelMethodToExpression.Clear();
      GenerateXamlRecursively(1, classDef.RootChild);

      WriteLine(0, "</UserControl>");

      End();
    }


    private readonly string[] IGNORE_ATTRIBUTES = new string[] {
      UiAttributeDefinitions.NAME,
      UiAttributeDefinitions.PATH,
    };

    private void GenerateXamlRecursively(int level, Instance instance) {
      if (instance == null)
        return;

      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null) {
        Messages.AddError(instance.XmlElement, "No platform-specific Class Definition for Logical Class {0}",
          instance.ClassDef.Name);
        return;
      }

      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName);
      if (platClassDef.StyleInfo != null)
        WriteLine(level + 1, "Style=\"{ StaticResource {0} }\"", platClassDef.StyleInfo);

      foreach (PlatformAttributeStatic staticAttr in platClassDef.StaticPlatformAttributes)
        WriteLine(level + 1, "{0}=\"{1}\"", staticAttr.PlatformName, staticAttr.ValueWithSubstitutions(instance));

      foreach (PlatformAttributeByFunc byFuncAttr in platClassDef.ByFuncPlatformAttributes)
        WriteLine(level + 1, "{0}=\"{1}\"", byFuncAttr.PlatformName, byFuncAttr.Function(instance));

      UiAttributeValue primaryValue = instance.PrimaryValue;
      PlatformAttributeDataBind dataBind = platClassDef.DataBindAttribute;
      bool dataBindAlreadyRendered = false;

      foreach (UiAttributeValue attrValue in instance.AttributeValues) {
        string attrName = attrValue.Definition.Name;

        if (attrValue == primaryValue || IGNORE_ATTRIBUTES.Contains(attrValue.Definition.Name))
          continue;

        PlatformAttributeDynamic dynamicAttr = platClassDef.FindDyamicAttribute(attrName);
        if (dynamicAttr == dataBind)
          dataBindAlreadyRendered = true;

        // Write the attribute
        if (attrValue is UiAttributeValueAtomic atomicValue) {
          string name = dynamicAttr == null ? NameUtils.Capitalize(attrName) : dynamicAttr.PlatformName;
          string value;
          if (atomicValue.Expression != null)
            value = GenerateBindingForFormula(instance, dynamicAttr, name, atomicValue.Expression);
          else if (atomicValue.Value != null)
            value = GenerateAttributeForValue(dynamicAttr, atomicValue.Value);
          else
            continue;

          WriteLine(level + 1, "{0}=\"{1}\"", name, value);
        } else {
          // TODO - e.g. Action from submit button
        }
      }

      Member member = instance.ModelMember;
      if (dataBind != null && !dataBindAlreadyRendered)
        WriteLine(level + 1, "{0}=\"{ Binding {1}{2}{3} }\"",
          dataBind.PlatformName,
          WpfGenUtils.MODEL_PROPERTY_PREFIX,
          WpfGenUtils.GetBindingPath(instance),
          member.IsReadOnly ? ", Mode=OneWay" : null);

      // Close the XAML element
      if (primaryValue == null)
        WriteLineClose(level, "/>");
      else {
        WriteLineClose(level, ">");
        WriteChildren(level + 1, primaryValue);
        WriteLine(level, "</{0}>", platClassDef.EffectivePlatformName);
      }
    }

    private string GenerateAttributeForValue(PlatformAttributeDynamic dynamicAttr, object value) {
      if (dynamicAttr != null && dynamicAttr.TranslationFunc != null)
        return dynamicAttr.TranslationFunc(value)?.ToString();
      else
        return value.ToString();
    }

    private string GenerateBindingForFormula(Instance context, PlatformAttributeDynamic dynamicAttr, string name, ExpBase expression) {
      string path;
      ExpIdentifier pathStart = expression.FirstMemberOfPath();
      if (pathStart != null) {
        // In this case, we know we have a simple path, so we can just inject it as the WPF path
        string expressionAsPath = ExpressionToString(expression, false);
        if (pathStart.IsOtherVariable)
          path = expressionAsPath;
        else
          // TODO: Deal with situation where expression is relative not to UI Component root,
          // but to some nested Enity
          path = WpfGenUtils.MODEL_PROPERTY_PREFIX + expressionAsPath;
      } else {
        // In this case, we have some sort of a more complex expression - we will render the expression in the
        // View Model, and reference it here
        // TODO: There is opportunity for duplicate names here, but we'll live with it for now
        path = string.Format("{0}_{1}",
          context.RenderAs.Name, name);
        _viewModelMethodToExpression[path] = expression;
      }

      string converter = null;
      if (dynamicAttr != null && dynamicAttr.Converter != null)
        converter = string.Format(", Converter={{StaticResource {0}}}", dynamicAttr.Converter);
      return string.Format("{{Binding Path={0}{1}}}", path, converter);
    }

    private void WriteChildren(int level, UiAttributeValue attrValue) {
      if (attrValue is UiAttributeValueComplex complexAttr) {
        foreach (Instance childInstance in complexAttr.Instances)
          GenerateXamlRecursively(level, childInstance);
      } else
        throw new NotImplementedException();
    }
    #endregion

    #region .xaml.cs File
    private void GenerateXamlCsFile(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, ".xaml.cs");

      WriteLine(0, "using System.Windows;");
      WriteLine(0, "using System.Windows.Controls;");
      WriteLine();

      WriteLine(0, "namespace {0} {", GetNamespace(classDef.XmlElement));
      WriteLine(1, "public partial class {0} : UserControl {", classDef.Name);

      WriteLine();
      WriteLine(2, "private {0}VM ViewModel { get { return ({0}VM)DataContext; } }", classDef.Name);
      WriteLine();

      GenerateConstructor(classDef);
      GenerateMethods(classDef);
      CodeletGenerator.Generate(this, CodeletTarget.XamlCs, classDef.RootChild);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateConstructor(ClassDefX10 classDef) {
      WriteLine(2, "public {0}() {", classDef.Name);
      WriteLine(3, "InitializeComponent();");
      WriteLine(3, "DataContext = new {0}VM(this);", classDef.Name);
      WriteLine(2, "}");
      WriteLine();
    }

    private void GenerateMethods(ClassDefX10 classDef) {
    }
    #endregion

    #region View Model (VM) File
    private void GenerateViewModelFile(ClassDefX10 classDef) {
      Entity dataModel = classDef.ComponentDataModel;

      Begin(classDef.XmlElement.FileInfo, "VM.cs");

      WriteLine(0, "using System.Collections.Generic;");
      WriteLine(0, "using System.Windows.Controls;");
      WriteLine();
      WriteLine(0, "using wpf_lib.lib;");   // TODO: Eventually, this should be dynamic
      WriteLine(0, "using wpf_lib.lib.utils;");

      WriteLine();
      WriteLine(0, "using {0};", GetNamespace(dataModel.TreeElement));
      WriteLine();

      WriteLine(0, "namespace {0} {", GetNamespace(classDef.XmlElement));
      WriteLine(1, "public partial class {0}VM : ViewModelBase<{1}> {", classDef.Name, dataModel.Name);

      GenerateState(classDef);
      GenerateDataSources(classDef);
      GenerateExpressions(classDef);

      // Constructor
      WriteLine(2, "public {0}VM(UserControl userControl) : base(userControl) {", classDef.Name);
      WriteLine(3, "{0} = {1}.Create(null);", WpfGenUtils.MODEL_PROPERTY, dataModel.Name);
      WriteLine(2, "}");

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateState(ClassDefX10 classDef) {
      IEnumerable<StateClass> states = classDef.GetStateVariables(AllEntities, AllEnums);
      if (states == null)
        return;

      WriteLine();
      WriteLine(2, "// State");

      foreach (StateClass state in states)
        GenerateProperty(state.ToX10DataType(), state.Variable, null);
    }

    private void GenerateExpressions(ClassDefX10 classDef) {
      if (_viewModelMethodToExpression.Count > 0) {
        WriteLine(2, "// Properties used in XAML bindings");
        foreach (var pair in _viewModelMethodToExpression)
          // TODO: This will not handle formulas which return non-atomic types
          GeneratePropertyForFormula(pair.Value.DataType.DataType, pair.Key, pair.Value, true);

        WriteLine();

        WriteLine(2, "// Ensures all properties above are refreshed every time anything changes");
        WriteLine(2, "public override void FireCustomPropertyNotification() {");
        foreach (string propertyName in _viewModelMethodToExpression.Keys)
          WriteLine(3, "RaisePropertyChanged(nameof({0}));", propertyName);
        WriteLine(2, "}");

        WriteLine();
      }
    }

    private void GenerateDataSources(ClassDefX10 classDef) {
      WriteLine(2, "// Data Sources");

      IEnumerable<DataTypeEnum> enums = UiUtils.ListSelfAndDescendants(classDef.RootChild)
        .Select(x => x.ModelMember)
        .OfType<X10Attribute>()
        .Select(x => x.DataType)
        .OfType<DataTypeEnum>()
        .Distinct();

      foreach (DataTypeEnum anEnum in enums) {
        string name = anEnum.Name;
        WriteLine(2, "public IEnumerable<EnumValueRepresentation> {0} { get; }",
          NameUtils.Pluralize(name));
        WriteLine(3, "= EnumValueRepresentation.GetForEnumType(typeof({0}));",
          WpfGenUtils.EnumToName(anEnum));
      }

      WriteLine();
    }

    #endregion

    private void GenerateViewModelCustomFile(ClassDefX10 classDef) {
    }
    #endregion

    #region Generate Models
    public override void Generate(Entity entity) {
      Begin(entity.TreeElement.FileInfo, ".cs");

      WriteLine(0, "using System;");
      WriteLine(0, "using System.Collections.Generic;");
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

      DerivedAttributeDependencyMap dependencyMap = DerivedAttributeDependencyMap.BuildMap(entity);

      GenerateRegularAttributes(dependencyMap, entity.RegularAttributes);
      GenerateDerivedAttributes(entity.DerivedAttributes);
      GenerateAssociations(entity.Associations);
      GenerateValidations(entity);
      GenerateToString(entity);
      GenerateCreateFunction(entity);

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private Dictionary<ExpBase, IEnumerable<Member>> CalculateFormulaDependencies(Entity entity) {
      throw new NotImplementedException();
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
    private void GenerateRegularAttributes(DerivedAttributeDependencyMap dependencyMap, IEnumerable<X10RegularAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in attributes) {
        HashSet<X10DerivedAttribute> dependentDerivedAttributes = dependencyMap.ChildDependencies(attribute);
        GenerateProperty(attribute.DataType, attribute, dependentDerivedAttributes);
      }
    }
    #endregion

    #region Derived Attributes
    private void GenerateDerivedAttributes(IEnumerable<X10DerivedAttribute> attributes) {
      WriteLine();
      WriteLine(2, "// Derived Attributes");

      foreach (X10DerivedAttribute attribute in attributes)
        GeneratePropertyForFormula(attribute.DataType,
          WpfGenUtils.MemberToName(attribute),
          attribute.Expression,
          false);
    }
    #endregion

    #region Associations
    private void GenerateAssociations(IEnumerable<Association> associations) {
      WriteLine();
      WriteLine(2, "// Associations");

      foreach (Association association in associations) {
        string dataType = AssociationDataType(association);
        string propName = WpfGenUtils.MemberToName(association);
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

    private static string BindablePropName(Association association) {
      return WpfGenUtils.MemberToName(association) + "Bindable";
    }

    private static string AssociationDataType(Association association) {
      string associationTarget = association.ReferencedEntity.Name;
      if (association.IsMany)
        return string.Format("List<{0}>", associationTarget);
      else
        return associationTarget;
    }
    #endregion

    #region Validations
    private void GenerateValidations(Entity entity) {
      WriteLine();
      WriteLine(2, "// Validations");
      WriteLine(2, "public override void CalculateErrors(string prefix, EntityErrors errors) {");

      // Validation of mandatory members
      foreach (Member member in entity.Members) {
        string propName = WpfGenUtils.MemberToName(member);
        string humanName = NameUtils.CamelCaseToHumanReadable(member.Name);
        bool canBeEmpty = member is X10Attribute attr && CanBeEmpty(attr.DataType);

        if (!member.IsReadOnly && canBeEmpty && member.IsMandatory) {
          WriteLine(3, "if (string.IsNullOrWhiteSpace({0}?.ToString()))", propName);
          WriteLine(4, "errors.Add(\"{0} is required\", prefix, nameof({1}));", humanName, propName);
        }
      }

      // Forward validation to owned associations
      IEnumerable<Association> ownedAssociations = entity.Associations.Where(x => x.Owns);
      if (ownedAssociations.Any()) {
        WriteLine();
        foreach (Association association in ownedAssociations) {
          if (association.IsMany) {
            // TODO
          } else {
            bool applicableWhen = GenerateApplicableWhen(association);
            string name = WpfGenUtils.MemberToName(association);
            WriteLine(3 + (applicableWhen ? 1 : 0), "{0}.CalculateErrors(\"{0}.\", errors);", name);
          }
        }
      }

      WriteLine(2, "}");
    }

    private bool GenerateApplicableWhen(Member member) {
      if (member.HasAttribute(BaseLibrary.APPLICABLE_WHEN)) {
        WriteLine(3, "if ({0})", PostCompileTransformations.ApplicableWhenPropertyName(member));
        return true;
      }
      return false;
    }
    #endregion

    #region Create ToString()
    private void GenerateToString(Entity entity) {
      ModelAttributeValue value = entity.FindAttribute(BaseLibrary.DEFAULT_STRING_REPRESENTATION);
      if (value == null)
        return;

      WriteLine();
      WriteLine(2, "public override string ToString() {");
      WriteLine(3, "return {0}?.ToString();", AttributeValueToString(value));
      WriteLine(2, "}");
    }
    #endregion

    #region Create Default Entity
    private void GenerateCreateFunction(Entity entity) {
      WriteLine();
      WriteLine(2, "public static {0} Create(EntityBase owner) {", entity.Name);
      WriteLine(3, "{0} newEntity = new {0} {", entity.Name);
      WriteLine(4, "Owner = owner,");

      foreach (X10RegularAttribute attribute in entity.RegularAttributes) {
        ModelAttributeValue defaultValue = attribute.FindAttribute(BaseLibrary.DEFAULT);
        if (defaultValue != null) {
          WriteLine(4, "{0} = {1},",
            WpfGenUtils.MemberToName(attribute),
            AttributeValueToString(defaultValue));
        }
      }
      WriteLine(3, "};");
      WriteLine();

      IEnumerable<Association> associations = entity.Associations.Where(x => x.Owns);
      if (associations.Count() > 0) {
        foreach (Association association in associations) {
          string associationTarget = association.ReferencedEntity.Name;
          string initializer;
          if (association.IsMany)
            initializer = string.Format("new List<{0}>()", associationTarget);
          else
            initializer = string.Format("{0}.Create(newEntity)", associationTarget);

          WriteLine(3, "newEntity.{0} = {1};", WpfGenUtils.MemberToName(association), initializer);
        }
        WriteLine();
      }

      WriteLine(3, "return newEntity;");
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
      WriteLine(level, "public enum {0} {", WpfGenUtils.EnumToName(theEnum));

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
        return WpfGenUtils.TypedLiteralToString(value.Value, value.EnumType);
      else if (value.Formula != null)
        return ExpressionToString(value.Expression, false);
      else
        return "BLANK VALUE";
    }

    private string ExpressionToString(ExpBase expression, bool prefixWithModel) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      WpfFormulaWriter formulaWriterVisitor = new WpfFormulaWriter(writer, prefixWithModel);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

    private string GetDataType(X10DataType dataType) {
      if (dataType.IsPrimitive)
        return GetDataType(dataType.DataType);
      else if (dataType.IsEntity)
        if (dataType.IsMany)
          return string.Format("IEnumerable<{0}>", dataType.Entity.Name);
        else
          return dataType.Entity.Name;
      else
        return dataType.ToString();
    }

    private string GetDataType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "bool";
      if (dataType == DataTypes.Singleton.Date) return "DateTime?";
      if (dataType == DataTypes.Singleton.Float) return "double?";
      if (dataType == DataTypes.Singleton.Integer) return "int?";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime?";
      if (dataType == DataTypes.Singleton.Money) return "double?";
      if (dataType is DataTypeEnum enumType) return WpfGenUtils.EnumToName(enumType) + "?";

      throw new Exception("Unknown data type: " + dataType.Name);
    }

    private bool CanBeEmpty(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return false;
      return true;
    }

    private void GenerateProperty(DataType type, Member member, IEnumerable<Member> extraRaiseChange) {
      GenerateProperty(new X10DataType(type), member, extraRaiseChange);
    }

    private void GenerateProperty(X10DataType type, Member member, IEnumerable<Member> extraRaiseChange) {
      string name = WpfGenUtils.MemberToName(member);
      GenerateProperty(type, name, extraRaiseChange.Select(x => WpfGenUtils.MemberToName(x)));
    }

    private void GenerateProperty(X10DataType type, string name, IEnumerable<string> extraRaiseChange) {
      string dataType = GetDataType(type);
      string varName = "_" + NameUtils.UncapitalizeFirstLetter(name);
      string propName = NameUtils.Capitalize(name);

      WriteLine(2, "private {0} {1};", dataType, varName);
      WriteLine(2, "public {0} {1} {", dataType, propName);
      WriteLine(3, "get { return {0}; }", varName);
      WriteLine(3, "set {");
      WriteLine(4, "{0} = value;", varName);
      WriteLine(4, "RaisePropertyChanged(nameof({0}));", propName);

      if (extraRaiseChange?.Count() > 0) {
        WriteLine();
        foreach (string extraPropName in extraRaiseChange)
          WriteLine(4, "RaisePropertyChanged(nameof({0}));", extraPropName);
      }

      WriteLine(3, "}");
      WriteLine(2, "}");
    }

    private void GeneratePropertyForFormula(DataType dataTypeObj, string propertyName, ExpBase expression, bool prefixWithModel) {
      string dataType = GetDataType(dataTypeObj);

      WriteLine(2, "public {0} {1} {", dataType, propertyName);
      WriteLine(3, "get {");
      WriteLine(4, "return {0};", ExpressionToString(expression, prefixWithModel));
      WriteLine(3, "}");
      WriteLine(2, "}");
    }

    #endregion
  }
}
