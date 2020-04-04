namespace x10.parsing {
  public abstract class XmlBase : IParseRoot {
    private FileInfo _fileInfo;
    public XmlBase Parent { get; internal set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }

    // Derived
    public XmlElement Root {
      get { return Parent == null ? (XmlElement)this : Parent.Root; }
    }
    public FileInfo FileInfo {
      get { return Root._fileInfo; }
    }

    public void CopyPropertiesFrom(XmlBase source) {
      _fileInfo = source._fileInfo;
      Parent = source.Parent;
      Start = source.Start;
      End = source.End;
    }

    public void SetFileInfo(string path) {
      _fileInfo = new FileInfo(path);
    }
  }
}