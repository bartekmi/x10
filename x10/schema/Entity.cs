using System;
using System.Linq;
using System.Text;

namespace x10.schema {
    public class Entity {
        public string Name { get; set; }
        public string Description { get; set; }
        public Property[] Properties { get; set; }
        public Association[] Associations { get; set; }
        public X10Enum[] Enums { get; set; }

        public Property FindProperty(string name) {
            return Properties.SingleOrDefault(x => x.Name == name);
        }

        public Association FindAssociation(string name) {
            return Associations.SingleOrDefault(x => x.Name == name);
        }
    }
}