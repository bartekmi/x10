using System;
using System.IO;
using System.Collections.Generic;

using x10.formula;
using x10.utils;
using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.gen.react {
  public partial class ReactCodeGenerator {

    private void GenerateFileHeader() {
      WriteLine(0, "// This file was auto-generated on {0}. Do not modify by hand.", DateTime.Now);
      WriteLine(0, "// @flow");
      WriteLine();
    }

    private static string ImportPath(ModelComponent modelComponent) {
      x10.parsing.FileInfo fileInfo = modelComponent.TreeElement.FileInfo;
      return AssembleRelativePath(fileInfo, null, true);  // TODO: Single source of truth for capitalization
    }

    #region Import Placeholder
    internal ImportsPlaceholder ImportsPlaceholder;
    internal void InsertImportsPlaceholder() {
      ImportsPlaceholder = new ImportsPlaceholder();
      AddPlaceholder(ImportsPlaceholder);
    }
    #endregion

    #region Source Variable Name
    private Stack<string> _sourceVariableNames = new Stack<string>();
    internal string SourceVariableName { get { return _sourceVariableNames.Peek(); } }

    internal void PushSourceVariableName(string variableName) {
      _sourceVariableNames.Push(variableName);
    }

    internal void PopSourceVariableName() {
      _sourceVariableNames.Pop();
    }
    #endregion

    #region Names of Things and Code Snippet Generation
    internal static string VariableName(Entity model, bool isMany) {
      if (model == null)
        return null;
      string name = model.Name;
      if (isMany)
        name = NameUtils.Pluralize(name);
      return NameUtils.UncapitalizeFirstLetter(name);
    }

    internal static string DerivedAttrFuncName(X10DerivedAttribute attribute) {
      return
        NameUtils.UncapitalizeFirstLetter(attribute.Owner.Name) +
        NameUtils.Capitalize(attribute.Name);
    }

    internal static string TypedLiteralToString(object literal, DataTypeEnum asEnum) {
      if (literal == null)
        return "null";

      if (asEnum != null)
        return string.Format("'{0}'", ToEnumValueString(literal));

      if (literal is string)
        return string.Format("'{0}'", literal);
      else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }
    #endregion

    #region Expression Helpers
    internal string ExpressionToString(ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      JavaScriptFormulaWriter formulaWriterVisitor = new JavaScriptFormulaWriter(writer, SourceVariableName, ImportsPlaceholder);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }
    #endregion

    #region Enum-Related Helpers

    internal static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }

    internal static string EnumToPairsConstant(DataTypeEnum enumType) {
      return enumType.Name + "EnumPairs";
    }

    internal static string ToEnumValueString(object value) {
      return NameUtils.CamelCaseToSnakeCaseAllCaps(value.ToString());
    }

    #endregion
  }
}
