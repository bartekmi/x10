namespace x10.hotchoc {
  public class PrimordialEntityBase {
    public string Id {
      get {
        return IdUtils.ToRelayId(this, Dbid);
      }
      set {
        // Do nothing. Needed to make Hot Chocolate happy.
      }
    }
    public int Hashcode {
      get { return GetHashCode(); }
      set {/* Do nothing */}
    }
    public int Dbid { get; set; }

    private static int _nextUniqueDbid = 1000;
    public virtual void EnsureUniqueDbid() {
      if (Dbid == 0)
        Dbid = _nextUniqueDbid++;
    }
  }
}