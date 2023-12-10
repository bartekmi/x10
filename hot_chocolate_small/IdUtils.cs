using System;
using System.Linq;

namespace x10.hotchoc {
  public static class IdUtils {
    public static bool IsUuid(string? id) {
      if (id == null)
        return false;
      return id.Length == 36 && id.Count(x => x == '-') >= 4;
    }

    public static string ToFrontEndId(PrimordialEntityBase entity, int dbid) {
      return string.Format("{0}_{1}", entity.GetType().Name, dbid);
    }

    public static string ToFrontEndId<T>(int dbid) {
      return string.Format("{0}_{1}", typeof(T).Name, dbid);
    }

    public static int? FromFrontEndId(string? frontEndId) {
      if (frontEndId == null)
        return null;
      return FromFrontEndId(frontEndId, out string dummy);
    }

    public static int FromFrontEndIdMandatory(string frontEndId) {
      int? dbid = FromFrontEndId(frontEndId, out string dummy);
      if (dbid == null)
        throw new Exception("Could not convert from Front-End Id: " + frontEndId);
      return dbid.Value;
    }

    public static int? FromFrontEndId(string frontEndId, out string entityName) {
      if (IsUuid(frontEndId)) {
        entityName = "";
        return null;
      }

      string[] pieces = frontEndId.Split('_');
      if (pieces.Length != 2)
        throw new Exception("Invalid Front-End Id: " + frontEndId);

      entityName = pieces[0];
      return int.Parse(pieces[1]);
    }
  }
}