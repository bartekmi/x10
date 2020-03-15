using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

using x10.error;
using x10.schema;

namespace x10.logictree {
    public class ElementDef : ElementBase {
        public string Path { get; set; }
        public string Team { get; set; }
        public string I18n { get; set; }

        internal ElementDef(XElement xElement, string name) : base(xElement, name) {
            // Do nothing
        }

        internal override void PostProcess(ErrorBucket errors, ElementBase parent) {
            Team = GetMandatoryAttribute(errors, "team");
            I18n = GetAttribute("i18n");

            string modelName = GetMandatoryAttribute(errors, "model");
            if (modelName != null) {
                if (modelName.StartsWith("[") && modelName.EndsWith("]")) {
                    IsDataModelMultiple = true;
                    modelName = modelName.Substring(1, modelName.Length - 2).Trim();
                }

                DataModel = Schema.Singleton.FindEntityByName(modelName);
                if (DataModel == null)
                    AddError(errors, "Unknown model: " + modelName);
            }

            // TODO: Should Components should be able to define their own parameters?
        }
    }
}