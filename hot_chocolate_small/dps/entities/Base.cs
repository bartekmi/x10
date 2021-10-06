using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// Provides attribute(s) common to all entities
  /// </summary>
  public abstract class Base : PrimordialEntityBase {
    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal virtual void SetNonOwnedAssociations(IRepository repository) {
    }
  }
}

