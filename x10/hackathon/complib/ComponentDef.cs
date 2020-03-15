using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using x10.schema;

namespace x10.complib {

    public enum LabelPlacement {
        Above,          // The most common/default one is the first
        None
    }

    public enum NullTreatment {
        NotApplicable,              // E.g. Group - not an input component
        NeverAllowsNull,            // E.g. Checkbox - no way to show null value
        ConvertNullToDefault,       // E.g. TextInput - null must be converted to ""
        AcceptsAndReturnsNull,      // E.g. SelectInput - value is of type: ?T
    }

    public abstract class ComponentDef {

        public abstract ComponentDef PhysicalComponent { get; set; }
        public abstract ParamDef[] Params { get; set; }

        public string ImportFile { get; set; }
        public string DocsUrl { get; set; }
        public string Name { get; set; }
        public LabelPlacement DefaultLabelPlacement { get; set; }
        public NullTreatment NullTreatment { get; set; }

        // Some components such as CheckBox and SelectInput simply can't be set to
        // empty. This impacts validation for presence
        public bool UserCantClear { get; set; }
        // Some Components have extra params which we can deduce from the Schema models
        // For example we can set the SelectInput.isNullable parameter based on if Property is required
        public Func<Property, IEnumerable<ParamValue>> ExtraParamsFunc { get; set; }

        public ParamDef FindParam(string paramName, out string error) {
            error = null;
            ParamDef param = Params.SingleOrDefault(x => x.Name == paramName);

            if (param == null)
                error = string.Format("Component {0} does not have parameter {1}", Name, paramName);

            return param;
        }

        public override string ToString() {
            return Name;
        }
    }
}