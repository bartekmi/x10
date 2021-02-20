using System;
using System.Collections.Generic;

namespace x10.hotchoc {
  public static class RepositoryUtils {
    public static int AddOrUpdate<T>(int? dbid, T entity, Dictionary<int, T> entities) where T : PrimordialEntityBase {
      if (dbid != null)
        entity.SetDbid(dbid.Value);
      entity.EnsureUniqueDbid();

      entities[entity.Dbid] = entity;
      return entity.Dbid;
    }
  }
}