using System;

using x10.logictree;

namespace x10.complib {
    public class ComponentDefComposite : ComponentDef {
        // TODO: This is dubious. Check uses of this
        public override ComponentDef PhysicalComponent {
            get { return this; }
            set { throw new NotImplementedException(); }
        }
        public override ParamDef[] Params { get; set; }

        public ElementDef ElementDef { get; private set; }

        internal ComponentDefComposite(ElementDef elementDef) {
            Name = elementDef.Name;
            ElementDef = elementDef;
            DefaultLabelPlacement = LabelPlacement.None;
            NullTreatment = NullTreatment.NotApplicable;        // TODO: how to deal?
        }
    }
}