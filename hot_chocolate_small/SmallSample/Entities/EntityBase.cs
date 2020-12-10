namespace Small.Entities {
  public class EntityBase {
    public string Id {
      get {
        return string.Format("{0}/{1}", this.GetType().Name, Dbid);
      }
      set {
        // Do nothing. Needed to make Hot Chocolate happy.
      }
    }
    public int Dbid { get; set; }
  }
}