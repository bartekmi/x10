using System;
using System.Collections.Generic;
using System.IO;

namespace x10.parsing {
  public abstract class Parser {
    public abstract TreeNode Parse(string filePath);
    public abstract string GetFileExtensionWithDot();

    private readonly MessageBucket _messages;

    protected Parser(MessageBucket messages) {
      _messages = messages;
    }

    public List<TreeNode> RecursivelyParseDirectory(string directoryPath) {
      List<TreeNode> parsed = new List<TreeNode>();

      RecursivelyParseDirectory(parsed, directoryPath);

      return parsed;
    }

    private void RecursivelyParseDirectory(List<TreeNode> parsed, string dirPath) {
      foreach (string path in Directory.EnumerateFiles(dirPath)) {
        if (!path.EndsWith(GetFileExtensionWithDot()))
          continue;
        
        TreeNode root = Parse(path);
        if (root == null)
          continue;

        root.SetFileInfo(path);
        parsed.Add(root);
      }

      foreach (string childDirPath in Directory.EnumerateDirectories(dirPath))
        RecursivelyParseDirectory(parsed, childDirPath);
    }

    protected void AddError(string message, TreeElement treeElement) {
      _messages.AddError(treeElement, message);
    }

    protected void AddWarning(string message, TreeElement treeElement) {
      _messages.AddWarning(treeElement, message);
    }

    protected void AddInfo(string message, TreeElement treeElement) {
      _messages.AddInfo(treeElement, message);
    }
  }
}