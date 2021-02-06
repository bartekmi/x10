using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// Provides attribute(s) common to all entities
  /// </summary>
  public abstract class Base : PrimordialEntityBase {
    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}

