using System;
using System.Linq;
using System.Text;

namespace x10.complib {

    public enum ParamType {
        Bool,
        String,
        Int,
        Float,
        Enum,
        Color,

        // Parameters with special meanings
        VarInputData,
        VarShowErrors,
        VarCallback,
        VarOnAddItem,
        VarSubstitute,
        VarChildAsCreateFunc,
    }

    public class ParamDef {
        public string Name { get; set; }
        public string Description { get; set; }
        public ParamType Type { get; set; }
        public bool IsRequired { get; set; }
        public bool NeedsTranslation { get; set; }
        public string Template { get; set; }

        // Specific to Type
        public string[] EnumValues { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}