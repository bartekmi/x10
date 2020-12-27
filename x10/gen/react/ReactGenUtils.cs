using System;

using x10.model.metadata;
using x10.utils;
using x10.model.definition;
using x10.parsing;

namespace x10.gen.react {
  public partial class ReactCodeGenerator {

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

    private void GenerateFileHeader() {
      WriteLine(0, "// This file was auto-generated on {0}. Do not modify by hand.", DateTime.Now);
      WriteLine(0, "// @flow");
      WriteLine();
    }

    private string ImportPath(ModelComponent modelComponent) {
      FileInfo fileInfo = modelComponent.TreeElement.FileInfo;
      return AssembleRelativePath(fileInfo, null, true);  // TODO: Single source of truth for capitalization
    }

    internal string VariableName(Entity model, bool isMany) {
      if (model == null)
        return null;
      string name = model.Name;
      if (isMany)
        name = NameUtils.Pluralize(name);
      return NameUtils.UncapitalizeFirstLetter(name);
    }

    #region Enum-Related Helpers

    private static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }

    private static string ToEnumValueString(object value) {
      return NameUtils.CamelCaseToSnakeCaseAllCaps(value.ToString());
    }

    #endregion
  }
}
