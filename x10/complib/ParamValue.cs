using System;
using System.Linq;
using System.Text;

using x10.complib;

namespace x10.complib {
    public class ParamValue {
        internal string Name { get; set; }
        public object Value { get; set; }
        public ParamDef Param { get; set; }

        public override string ToString() {
            return string.Format("{0} = {1}", Param.Name, Value);
        }
    }
}