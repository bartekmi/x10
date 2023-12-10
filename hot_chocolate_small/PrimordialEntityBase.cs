using System;
using HotChocolate;

namespace x10.hotchoc {
  public abstract class PrimordialEntityBase {
    private string _id;
    public string Id { 
      get { return _id; }
      set {
        _id = value;
      }
    }

    [GraphQLIgnore]
    public int DbidHotChoc { get; private set; }

    internal void SetDbid(int dbid) {
      DbidHotChoc = dbid;
      Id = IdUtils.ToFrontEndId(this, dbid);
    }

    private static int _nextUniqueDbid = 1000;
    public virtual void EnsureUniqueDbid() {
      if (DbidHotChoc == 0)
        SetDbid(_nextUniqueDbid++);
    }
  }
}