using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace x10.parsing {
  public class FileInfo {
    public string RootPath { get; private set; }
    public string[] RelativeDirComponents;
    public string FileName { get; private set; }

    public string FilePath {
      get {
        IEnumerable<string> components = new string[] { RootPath }
          .Concat(RelativeDirComponents)
          .Concat(FileName == null ? new string[0] : new string[] { FileName });
        return Path.Combine(components.ToArray());
      }
    }

    public FileInfo(string rootPath, string[] relativeDirComponents, string fileName) {
      RootPath = rootPath;
      RelativeDirComponents = relativeDirComponents;
      FileName = fileName;
    }

    internal FileInfo CreateDirFileInfo(string dirComponentName) {
      return new FileInfo(
        RootPath,
        RelativeDirComponents.Concat(new string[] { dirComponentName }).ToArray(),
        null);
    }

    internal FileInfo CreateFileFileInfo(string fileName) {
      return new FileInfo(
        RootPath,
        RelativeDirComponents,
        fileName);
    }

    public static FileInfo FromFilename(string filePath) {
      return new FileInfo(
        Path.GetDirectoryName(filePath),
        new string[0],
        Path.GetFileName(filePath));
    }
  }
}