using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace x10.complib {
    public class ComponentDefBasic : ComponentDef {

        public override ComponentDef PhysicalComponent {
            get { return this; }
            set { throw new NotImplementedException(); }
        }
        public override ParamDef[] Params { get; set; }
    }
}