namespace x10.parsing {
  public class FileInfo {
    public string FilePath { get; private set; }

    public FileInfo(string filePath) {
      FilePath = filePath;
    }
  }
}