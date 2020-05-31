using System.IO;

namespace x10.parsing {
  public class FileInfo {
    public string FilePath { get; private set; }

    // Derived
    public string[] RelativePathComponents {
      get {
        // TODO: Must have a way to make the path relative 
        string dirName = Path.GetDirectoryName(FilePath);
        return dirName.Split("/");
      }
    }


    public FileInfo(string filePath) {
      FilePath = filePath;
    }

  }
}