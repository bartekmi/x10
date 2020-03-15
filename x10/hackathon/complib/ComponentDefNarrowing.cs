using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using x10.schema;

namespace x10.complib {
    // In some cases, we may want a component which is a restriction, narrowing,
    // or specialization of another component.
    // The classic example are Row/Column component which are just specializations
    // of the Group component
    public class ComponentDefNarrowing : ComponentDef {
        public string ParentComponentName { get; set; }
        public ParamValue[] NarrowedParams { get; set; }

        public override ComponentDef PhysicalComponent { get; set; }

        public override ParamDef[] Params {
            get {
                List<string> excludedNames = NarrowedParams.Select(x => x.Name).ToList();
                return PhysicalComponent.Params
                    .Where(x => !excludedNames.Contains(x.Name))
                    .ToArray();
            }
            set { throw new NotImplementedException(); }
        }
    }
}