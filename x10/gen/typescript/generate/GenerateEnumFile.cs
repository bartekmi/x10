using System.Collections.Generic;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.model.metadata;

namespace x10.gen.typescript.generate {
  public partial class TypeScriptCodeGenerator : CodeGenerator {

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".ts", false);

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(anEnum);

      End();
    }

    public void GenerateEnum(DataTypeEnum theEnum) {
      ImportsPlaceholder.ImportGraphqlTypeEnum(theEnum);
      GeneratePairs(theEnum);
    }

    private void GeneratePairs(DataTypeEnum theEnum) {
      WriteLine(0, "export const {0}: {", EnumToPairsConstant(theEnum));
      WriteLine(1, "value: {0},", EnumToTypeName(theEnum));
      WriteLine(1, "label: string");
      WriteLine(0, "}[] = [");

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        WriteLine(1, "{");
        WriteLine(2, "value: {0},", ToEnumValue(theEnum, enumValue));
        WriteLine(2, "label: '{0}',", enumValue.EffectiveLabel);
        if (enumValue.IconName != null)
          WriteLine(2, "icon: '{0}'", enumValue.IconName);
        WriteLine(1, "},");
      }

      WriteLine(0, "];");
      WriteLine();
    }
  }
}
