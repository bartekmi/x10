using System;
using System.Collections.Generic;
using System.IO;

namespace x10.parsing {
  public abstract class Parser {
    public abstract IParseElement Parse(FileInfo file);
    public abstract string GetFileExtensionWithDot();

    private readonly MessageBucket _messages;
    private readonly string _rootDir;

    protected Parser(MessageBucket messages, string rootDir) {
      _messages = messages;
      _rootDir = rootDir;
    }

    public List<IParseElement> RecursivelyParseDirectory() {
      List<IParseElement> parsed = new List<IParseElement>();

      RecursivelyParseDirectory(parsed, new FileInfo(_rootDir, new string[0], null));

      return parsed;
    }

    private void RecursivelyParseDirectory(List<IParseElement> parsed, FileInfo dirInfo) {
      string dirPath = dirInfo.FilePath;

      foreach (string path in Directory.EnumerateFiles(dirPath)) {
        if (!path.EndsWith(GetFileExtensionWithDot()))
          continue;

        FileInfo pathInfo = dirInfo.CreateFileFileInfo(Path.GetFileName(path));
        IParseElement root = Parse(pathInfo);
        if (root == null)
          continue;

        root.SetFileInfo(pathInfo);
        parsed.Add(root);
      }

      foreach (string childDirPath in Directory.EnumerateDirectories(dirPath)) {
        FileInfo childDirInfo = dirInfo.CreateDirFileInfo(Path.GetFileName(childDirPath));
        RecursivelyParseDirectory(parsed, childDirInfo);
      }
    }

    protected void AddError(string message, IParseElement treeElement) {
      _messages.AddError(treeElement, message);
    }

    protected void AddWarning(string message, IParseElement treeElement) {
      _messages.AddWarning(treeElement, message);
    }

    protected void AddInfo(string message, IParseElement treeElement) {
      _messages.AddInfo(treeElement, message);
    }
  }
}