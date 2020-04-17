using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;
using x10.model;

namespace x10.compiler {
  public class EntitiesAndEnumsCompiler {

    private readonly MessageBucket _messages;
    private readonly AllEnums _allEnums;

    public EntitiesAndEnumsCompiler(MessageBucket messages, AllEnums allEnums) {
      _messages = messages;
      _allEnums = allEnums;
    }

    public List<Entity> Compile(string dirPath) {
      // Recursively parse entire directory
      Parser parser = new ParserYaml(_messages);
      List<TreeNode> rootNodes = parser.RecursivelyParseDirectory(dirPath).Cast<TreeNode>().ToList();

      // Pass 1
      List<Entity> entities = new List<Entity>();
      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enums = new EnumsCompiler(_messages, _allEnums,attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(_messages, enums, attrReader);

      foreach (TreeNode rootNode in rootNodes) {
        if (IsEnumFile(rootNode.FileInfo.FilePath))
          enums.CompileEnumFile(rootNode);
        else {
          Entity entity = pass1.CompileEntity(rootNode);
          if (!string.IsNullOrWhiteSpace(entity?.Name))
            entities.Add(entity);
        }
      }

      // Pass 2
      AllEntities allEntities = new AllEntities(_messages, entities);
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(_messages, allEntities, _allEnums);
      pass2.CompileAllEntities();

      return entities;
    }

    private static bool IsEnumFile(string path) {
      return path.ToLower().EndsWith("enums.yaml");
    }
  }
}