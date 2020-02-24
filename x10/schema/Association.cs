using System;
using System.Linq;
using System.Text;

namespace x10.schema {
    public class Association {

        public enum TypeEnum {
            HasMany,
            HasOne,
        }

        public string Name { get; set; }
        public string ChildEntityName { get; set; }
        public Entity ChildEntity { get; set; }
        public TypeEnum Type { get; set; }
    }
}