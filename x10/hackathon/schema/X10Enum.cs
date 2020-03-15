using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace x10.schema {
    public class X10Enum {
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumValue[] Values { get; set; }

        public EnumValue Parse(string name, out string error) {
            EnumValue value = Values.SingleOrDefault(x => x.Name == name);
            if (value == null) {
                error = string.Format("Invalid value {0} for enum {1}. Valid values are: {2}",
                    name, Name, string.Join(", ", (IEnumerable<object>)Values));
                return null;
            }

            error = null;
            return value;
        }
    }
}