using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using x10.utils;
using x10.schema.validation;

namespace x10.schema {
    public class Property {
        public string Name { get; set; }
        public string Description { get; set; }
        public object DefualtValue { get; set; }
        public string LabelOverride { get; set; }
        public string IsRelevant { get; set; }
        public Units? Units { get; set; }
        public DataType Type { get; set; }
        public List<Validation> Validations { get; private set; }
        public Entity Owner { get; internal set; }

        // Intermediate - used only during rehydration
        internal string EnumAsString { get; set; }
        internal string DefaultValueAsString { get; set; }
        internal string UiOverrideAsString { get; set; }

        // Derived
        public string Label {
            get { return LabelOverride == null ? NameUtils.ToHuman(Name) : LabelOverride; }
        }
        public bool HasValidations { get { return Validations.Count > 0; } }
        public bool IsMandatory {
            get { return Validations.Any(x => x is ValidationMandatory); }
        }
        public bool HasDefault { get { return DefaultValueAsString != null; } }

        // These parameters are specific to particular data types.
        // Consider having an intermediate entity that marries up Property and DataType
        public int? DecimalPlaces { get; set; }
        public X10Enum Enum { get; set; }

        internal Property() {
            Validations = new List<Validation>();
        }

        public override string ToString() {
            return Name;
        }
    }
}