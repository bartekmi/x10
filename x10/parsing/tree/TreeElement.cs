namespace x10.parsing {
  public abstract class TreeElement {
    public FileInfo FileInfo { get; set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }
  }
}