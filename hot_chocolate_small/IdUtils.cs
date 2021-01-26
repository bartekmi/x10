using System.Linq;

using x10.hotchoc.Entities;

namespace x10.hotchoc {
  public static class IdUtils {
    public static bool IsUuid(string id) {
      return id.Length == 36 && id.Count(x => x == '-') >= 4;
    }

    public static string ToRelayId(PrimordialEntityBase entity, int dbid) {
      return string.Format("{0}_{1}", entity.GetType().Name, dbid);
    }

    public static int FromRelayId(string relayId) {
      return FromRelayId(relayId, out string dummy);
    }

    public static int FromRelayId(string relayId, out string entityName) {
      string[] pieces = relayId.Split('_');
      entityName = pieces[0];
      return int.Parse(pieces[1]);
    }
  }
}