using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

using x10.error;
using x10.complib;
using x10.schema;

namespace x10.logictree {
    public class ElementUse : ElementBase {

        private const string PARAM_PATH = "path";

        public string Path { get; private set; }

        internal ElementUse(XElement xElement, string name) : base(xElement, name) {
            // Do nothing
        }

        internal override void PostProcess(ErrorBucket errors, ElementBase parent) {
            Component = ComponentLibrary.Singleton.FindComponent(Name, out string error);
            if (error != null) {
                AddError(errors, error);
                return;
            }

            // If this element uses a "Narrowing Component" - transfer its narrowing Params
            if (Component is ComponentDefNarrowing) {
                foreach (ParamValue narrowing in (Component as ComponentDefNarrowing).NarrowedParams)
                    ParamValues.Add(new ParamValue() {
                        Name = narrowing.Name,
                        Param = narrowing.Param,
                        Value = narrowing.Value,
                    });
            }

            // Possibly descend down the path to a lower Data Model Entity
            DataModel = parent.DataModel;
            Path = GetAttribute(PARAM_PATH);
            if (Path == null || parent.DataModel == null)
                DataModel = parent.DataModel;
            else {
                Association association = parent.DataModel.FindAssociation(Path);
                if (association == null)
                    AddError(errors, string.Format("Association {0} does not exist on Entity {1}", Path, parent.DataModel.Name));
                else
                    DataModel = association.ChildEntity;
            }

            if (error != null) {
                AddError(errors, error);
                return;
            }



            foreach (KeyValuePair<string, string> attr in GetAttributes(errors, PARAM_PATH)) {
                string paramName = attr.Key;
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