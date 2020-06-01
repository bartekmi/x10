namespace x10.parsing {
  public abstract class TreeElement : IParseElement {
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

    public void CopyPropertiesFrom(TreeElement source) {
      _fileInfo = source._fileInfo;
      Parent = source.Parent;
      Start = source.Start;
      End = source.End;
    }

    public void SetFileInfo(FileInfo fileInfo) {
      _fileInfo = fileInfo;
    }
  }
}