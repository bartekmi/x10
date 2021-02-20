using System;
using HotChocolate;

namespace x10.hotchoc {
  public class PrimordialEntityBase {
    private string _id;
    public string Id { 
      get { return _id; }
      set {
        _id = value;
      }
    }

    public int Hashcode {
      get { return GetHashCode(); }
      set {/* Do nothing */}
    }

    [GraphQLIgnore]
    public int Dbid { get; private set; }

    internal void SetDbid(int dbid) {
      Dbid = dbid;
      Id = IdUtils.ToRelayId(this, dbid);
    }

    private static int _nextUniqueDbid = 1000;
    public virtual void EnsureUniqueDbid() {
      if (Dbid == 0)
        Dbid = _nextUniqueDbid++;
    }
  }
}