using System;
using System.Collections.Generic;
using System.IO;

namespace x10.parsing {
  public abstract class Parser {
    public abstract IParseElement Parse(string filePath);
    public abstract string GetFileExtensionWithDot();

    private readonly MessageBucket _messages;

    protected Parser(MessageBucket messages) {
      _messages = messages;
    }

    public List<IParseElement> RecursivelyParseDirectory(string directoryPath) {
      List<IParseElement> parsed = new List<IParseElement>();

      RecursivelyParseDirectory(parsed, directoryPath);

      return parsed;
    }

    private void RecursivelyParseDirectory(List<IParseElement> parsed, string dirPath) {
      foreach (string path in Directory.EnumerateFiles(dirPath)) {
        if (!path.EndsWith(GetFileExtensionWithDot()))
          continue;

        IParseElement root = Parse(path);
        if (root == null)
          continue;

        root.SetFileInfo(path);
        parsed.Add(root);
      }

      foreach (string childDirPath in Directory.EnumerateDirectories(dirPath))
        RecursivelyParseDirectory(parsed, childDirPath);
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