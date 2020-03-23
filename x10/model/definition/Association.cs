using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class Association : Member {
    public string ReferencedEntityName { get; set; }
    public bool IsMany { get; set; }
    public bool Owns { get; set; }

    // Rehydrated
    public Entity ReferencedEntity { get; set; }
  }
}
