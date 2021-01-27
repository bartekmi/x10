using System;
using System.Collections.Generic;

namespace x10.hotchoc {
  public static class RepositoryUtils {
    public static int AddOrUpdate<T>(int? dbid, T entity, Dictionary<int, T> entities, Func<T, int> ensureUniqueDbids) where T : PrimordialEntityBase {
      if (dbid != null)
        entity.Dbid = dbid.Value;
      dbid = ensureUniqueDbids(entity);

      entities[dbid.Value] = entity;
      return dbid.Value;
    }
  }
}