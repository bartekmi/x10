using System;
using System.Collections.Generic;
using System.IO;

namespace x10.parsing {
  public abstract class Parser {
    public abstract TreeNode Parse(string filePath);
    public abstract string GetFileExtensionWithDot();

    public MessageBucket Messages = new MessageBucket();  // Needed for testing

    public List<TreeNode> RecursivelyParseDirectory(string directoryPath) {
      List<TreeNode> parsed = new List<TreeNode>();
      Messages = new MessageBucket();

      RecursivelyParseDirectory(parsed, directoryPath);

      return parsed;
    }

    private void RecursivelyParseDirectory(List<TreeNode> parsed, string dirPath) {
      foreach (string path in Directory.EnumerateFiles(dirPath)) {
        if (!path.EndsWith(GetFileExtensionWithDot()))
          continue;
        TreeNode root = Parse(path);
        root.SetFileInfo(path);
        parsed.Add(root);
      }

      foreach (string childDirPath in Directory.EnumerateDirectories(dirPath))
        RecursivelyParseDirectory(parsed, childDirPath);
    }

    protected void AddError(string message, TreeElement treeElement) {
      Messages.Add(new CompileMessage() {
        Severity = CompileMessageSeverity.Error,
        Message = message,
        TreeElement = treeElement,
      });
    }
  }
}