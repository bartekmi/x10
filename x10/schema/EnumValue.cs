using System;
using System.Linq;
using System.Text;

namespace x10.schema {
    public class EnumValue {
        public int IntValue { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public override string ToString() {
            // Do not change this as it connects to code generation
            return Name;
        }
    }
}