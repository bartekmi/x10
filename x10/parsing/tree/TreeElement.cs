namespace x10.parsing {
  public abstract class TreeElement {
    private FileInfo _fileInfo;
    public TreeElement Parent { get; internal set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }

    // Derived
    public TreeElement Root {
      get { return Parent == null ? this : Parent.Root; }
    }
    public FileInfo FileInfo {
      get { return Root._fileInfo; }
    }

    internal void SetFileInfo(string path) {
      _fileInfo = new FileInfo(path);
    }
  }
}