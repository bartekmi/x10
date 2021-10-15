using System.Collections.Generic;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.model.metadata;

namespace x10.gen.react.generate {
  public partial class ReactCodeGenerator : CodeGenerator {

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".js", false);

      GenerateFileHeader();
      WriteLine();

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(anEnum);

      End();
    }

    public void GenerateEnum(DataTypeEnum theEnum) {
      GeneratePairs(theEnum);
      GenerateEnumType(theEnum);
    }

    private void GeneratePairs(DataTypeEnum theEnum) {
      WriteLine(0, "export const {0} = [", EnumToPairsConstant(theEnum));

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        WriteLine(1, "{");
        WriteLine(2, "value: '{0}',", ToEnumValueString(enumValue.Value));
        WriteLine(2, "label: '{0}',", enumValue.EffectiveLabel);
        if (enumValue.IconName != null)
          WriteLine(1, "icon: '{0}'", enumValue.IconName);
        WriteLine(1, "},");
      }

      WriteLine(0, "];");
      WriteLine();
    }

    private void GenerateEnumType(DataTypeEnum theEnum) {
      IEnumerable<string> enumStrings =
        theEnum.AvailableValuesAsStrings.Select(x => string.Format("'{0}'", ToEnumValueString(x)));

      WriteLine(0, "export type {0} = {1};",
        EnumToName(theEnum),
        string.Join(" | ", enumStrings));

      WriteLine();
    }

  }
}
