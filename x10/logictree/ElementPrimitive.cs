using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

using x10.error;
using x10.schema;
using x10.complib;

namespace x10.logictree {
    public class ElementPrimitive : ElementBase {
        public Property Property { get; private set; }

        private const string PARAM_UI = "ui";


        internal ElementPrimitive(XElement xElement, string name) : base(xElement, name) {
            // Do nothing
        }

        internal override void PostProcess(ErrorBucket errors, ElementBase parent) {
            DataModel = parent.DataModel;
            if (DataModel == null)      // Error earlier on
                return;

            // Find the property
            Property = DataModel.FindProperty(Name);
            if (Property == null) {
                AddError(errors, string.Format("Property {0} does not exist on Model {1}",
                    Name, DataModel.Name));
                return;
            }

            // TODO: Associations

            // Usually this is null, indicating to just choose the default UI for the data type
            string uiOverride = GetAttribute(PARAM_UI);
            Component = ComponentLibrary.Singleton.FindComponent(Property, uiOverride, out string error);
            if (error != null) {
                AddError(errors, error);
                return;
            }

            // Guard against things like trying to use a Chackbox to display a nullable boolean
            if (!Property.IsMandatory && Component.NullTreatment == NullTreatment.NeverAllowsNull) {
                AddError(errors, string.Format("Nullable Property {0} is tied to component {1} which can never accept null values",
                    Property.Name, Component.Name));
            }

            foreach (KeyValuePair<string, string> attr in GetAttributes(errors, PARAM_UI)) {
                ParamDef paramDef = Component.FindParam(attr.Key, out error);
                if (paramDef == null) {
                    AddError(errors, error);
                    continue;
                }
                ParamValues.Add(new ParamValue() {
                    Param = paramDef,
                    Value = attr.Value,
                });
            }
        }

    }
}