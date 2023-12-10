using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.formula;
using x10.utils;
using x10.model.metadata;
using x10.model.definition;
using x10.model;
using x10.compiler.ui;
using x10.ui.composition;
using x10.ui.libraries;
using x10.gen.react.placeholder;

namespace x10.gen.react.generate {
  public partial class ReactCodeGenerator {

    #region Import Placeholder
    internal ImportsPlaceholder ImportsPlaceholder;
    internal string GeneratedCodeSubdir;
    internal string AppContextImport;
    internal string ReactLibImport = "react_lib";   // Changing this makes it possible to mount rect_lib elsewhere
    internal void InsertImportsPlaceholder() {
      ImportsPlaceholder = new ImportsPlaceholder(GeneratedCodeSubdir, AppContextImport, ReactLibImport);
      AddPlaceholder(ImportsPlaceholder);
    }
    #endregion

    #region Source Variable Name
    private Stack<string> _sourceVariableNames = new Stack<string>();
    internal string SourceVariableName { get { return _sourceVariableNames.Peek(); } }
    internal bool AlreadyScopedToMember { get; private set;}

    internal void PushSourceVariableName(string variableName, bool alreadyScopedToMember = false) {
      if (AlreadyScopedToMember && alreadyScopedToMember)
        throw new Exception("Cannot nest 'AlreadyScopedToMember' - it should only happen at leaf Instance nodes.");
        
      _sourceVariableNames.Push(variableName);
      AlreadyScopedToMember = alreadyScopedToMember;
    }

    internal void PopSourceVariableName() {
      _sourceVariableNames.Pop();
      AlreadyScopedToMember = false;
    }
    #endregion

    #region Names of Things
    internal static string VariableName(Entity model, bool isMany = false) {
      if (model == null)
        return null;
      string name = model.Name;
      if (isMany)
        name = NameUtils.Pluralize(name);
      return NameUtils.UncapitalizeFirstLetter(name);
    }

    internal static string FunctionName(string x10FunctionName) {
      return NameUtils.UncapitalizeFirstLetter(x10FunctionName);
    }

    internal static string FunctionName(Function function) {
      return FunctionName(function.Name);
    }

    internal static string DerivedAttrFuncName(X10DerivedAttribute attribute) {
      return
        NameUtils.UncapitalizeFirstLetter(attribute.Owner.Name) +
        NameUtils.Capitalize(attribute.Name);
    }

    internal static string WhenApplicableFuncName(Member member) {
      return member.ApplicableWhen == null ?
        null :
        DerivedAttrFuncName(member.ApplicableWhen);
    }

    internal static string CreateDefaultFuncName(Entity model) {
      return "createDefault" + model.Name;
    }

    internal static string CalculateErrorsFuncName(Entity model) {
      return string.Format("{0}CalculateErrors", NameUtils.UncapitalizeFirstLetter(model.Name));
    }
    #endregion

    #region Expression/Binding Helpers
    internal string ExpressionToString(ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      JavaScriptFormulaWriter formulaWriterVisitor = new JavaScriptFormulaWriter(writer, SourceVariableName, ImportsPlaceholder);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

    internal string GetBindingPath(Instance instance) {
      IEnumerable<Member> path = UiCompilerUtils.GetBindingPath(instance);
      if (path.Count() == 0)
        return SourceVariableName;
      ExpBase expression = CodeGenUtils.PathToExpression(path);
      string expressionString = ExpressionToString(expression);
      return expressionString;
    }

    internal string GetReadOnlyBindingPath(Instance instance) {
      return GetBindingPath(instance);
    }

    // Though currently not used, this is likely to come in handy.
    internal string GeneratePathExpression(IEnumerable<Member> membersEnumerable) {
      Member[] members = membersEnumerable.ToArray();
      StringBuilder builder = new StringBuilder();

      for (int ii = 0; ii < members.Length; ii++) {
        if (ii > 0)
          builder.Append(".");
        Member member = members[ii];
        builder.Append(member.Name);
        if (!member.IsMandatory && ii < members.Length - 1)
          builder.Append("?");
      }

      return builder.ToString();
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
      return NameUtils.CamelCaseToSnakeCase(value.ToString());
    }

    #endregion

    #region Misc
    internal static string TypedLiteralToString(object literal, DataTypeEnum asEnum, bool isCodeSnippet) {
      if (literal == null)
        return "null";

      if (asEnum != null)
        return string.Format("'{0}'", ToEnumValueString(literal));

      if (literal is string str) {
        if (isCodeSnippet)
          return str;
        return string.Format("'{0}'", literal);
      } else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }
    #endregion

  }
}
