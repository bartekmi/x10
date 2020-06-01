using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Core;

using x10.utils;

namespace x10.parsing {
  public class ParserYaml : Parser {

    public ParserYaml(MessageBucket messages, string rootDir) : base(messages, rootDir) {
      // Do nothing
    }

    public TreeNode ParseFromString(string yaml) {
      return ParseFromStrings(yaml).Single();
    }

    public IEnumerable<TreeNode> ParseFromStrings(params string[] yamls) {
      List<TreeNode> parsed = new List<TreeNode>();

      foreach (string yaml in yamls)
        using (TextReader reader = new StringReader(yaml)) {
          TreeNode node = ParsePrivate(() => YamlUtils.ReadYamlFromString(yaml), null);
          if (node == null)
            throw new Exception("Could not parse YAML: " + yaml);
          string entityName = (node as TreeHash)?.FindValue("name")?.ToString();
          node.SetFileInfo(FileInfo.FromFilename(entityName == null ? "Tmp.yaml" : entityName + ".yaml"));
          parsed.Add(node);
        }

      return parsed;
    }

    public IParseElement Parse(string filePath) {
      return Parse(FileInfo.FromFilename(filePath));
    }

    public override IParseElement Parse(FileInfo fileInfo) {
      return ParsePrivate(() => YamlUtils.ReadYaml(fileInfo.FilePath), fileInfo);
    }

    private TreeNode ParsePrivate(Func<YamlDocument> yamlReaderFunc, FileInfo fileInfo) {
      try {
        YamlDocument document = yamlReaderFunc();
        if (document == null) {
          AddInfo("No YAML document read - empty file?", new TreeFileError(fileInfo));
          return null;
        }

        TreeNode treeRoot = ParseRecursive(document.RootNode);
        return treeRoot;
      } catch (YamlException e) {
        AddError("Can't parse YAML file. Error: " + ExceptionUtils.GetMessageRecursively(e),
          new TreeFileError(fileInfo) {
            Start = ToMark(e.Start),
            End = ToMark(e.End),
          });
        return null;
      }
    }

    private TreeNode ParseRecursive(YamlNode yamlNode) {
      TreeNode treeNode;

      if (yamlNode is YamlSequenceNode) {
        treeNode = new TreeSequence();
        foreach (YamlNode child in (yamlNode as YamlSequenceNode).Children)
          ((TreeSequence)treeNode).AddChild(ParseRecursive(child));

      } else if (yamlNode is YamlMappingNode) {
        treeNode = new TreeHash();
        foreach (var yamlChild in (yamlNode as YamlMappingNode)) {
          TreeNode treeChild = ParseRecursive(yamlChild.Value);
          TreeAttribute attribute = new TreeAttribute(yamlChild.Key.ToString(), treeChild);
          SetLocation(attribute, yamlChild.Key);
          ((TreeHash)treeNode).AddAttribute(attribute);
        }

      } else if (yamlNode is YamlScalarNode) {
        treeNode = new TreeScalar((yamlNode as YamlScalarNode).Value);

      } else
        throw new Exception("I din't know there is a fourth option: " + yamlNode.ToString());

      SetLocation(treeNode, yamlNode);

      return treeNode;
    }

    public override string GetFileExtensionWithDot() {
      return ".yaml";
    }

    private void SetLocation(TreeElement treeNode, YamlNode yaml) {
      treeNode.Start = ToMark(yaml.Start);
      treeNode.End = ToMark(yaml.Start);
    }

    private PositionMark ToMark(Mark mark) {
      return new PositionMark() {
        Index = mark.Index,
        LineNumber = mark.Line,
        CharacterPosition = mark.Column,
      };
    }
  }
}