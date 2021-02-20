using System;
using System.Collections.Generic;

namespace x10.hotchoc {
  public abstract class RepositoryBase {
    public abstract void Add(PrimordialEntityBase instance);
    public abstract IEnumerable<Type> Types();
  }
}