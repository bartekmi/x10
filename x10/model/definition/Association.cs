using System;
using System.Collections.Generic;
using System.Text;
using x10.formula;

namespace x10.model.definition {
  public class Association : Member {
    public string ReferencedEntityName { get; set; }
    public bool IsMany { get; set; }
    public bool Owns { get; set; }

    // Rehydrated
    public Entity ReferencedEntity { get; set; }

    public override ExpDataType GetExpressionDataType() {
      return new ExpDataType(ReferencedEntity);
    }
  }
}
