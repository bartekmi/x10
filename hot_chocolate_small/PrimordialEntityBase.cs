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
    public int Dbid { get; set; }

    private static int _nextUniqueDbid = 1000;
    public void EnsureUniqueDbid() {
      if (Dbid == 0)
        Dbid = _nextUniqueDbid++;
    }
  }
}