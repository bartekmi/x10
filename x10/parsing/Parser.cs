using System;
using System.Collections.Generic;
using System.IO;

namespace x10.parsing {
  public abstract class Parser {
    public abstract TreeNode Parse(string filePath);
    public abstract string GetFileExtensionWithDot();

    private MessageBucket _messages;

    public List<TreeNode> RecursivelyParseDirectory(string directoryPath) {
      List<TreeNode> parsed = new List<TreeNode>();
      _messages = new MessageBucket();

      RecursivelyParseDirectory(parsed, directoryPath);

      return parsed;
    }

    private void RecursivelyParseDirectory(List<TreeNode> parsed, string dirPath) {
      foreach (string path in Directory.EnumerateFiles(dirPath)) {
        if (!path.EndsWith(GetFileExtensionWithDot()))
          continue;
        TreeNode root = Parse(path);
        parsed.Add(root);
      }

      foreach (string childDirPath in Directory.EnumerateDirectories(dirPath))
        RecursivelyParseDirectory(parsed, childDirPath);
    }
  }
}