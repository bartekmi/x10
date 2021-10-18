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
    private readonly AllFunctions _allFunctions;

    public EntitiesAndEnumsCompiler(MessageBucket messages, AllEnums allEnums, AllFunctions allFunctions) {
      _messages = messages;
      _allEnums = allEnums;
      _allFunctions = allFunctions;
    }

    public List<Entity> CompileFromYamlStrings(params string[] yamls) {
      // Recursively parse entire directory
      ParserYaml parser = new ParserYaml(_messages, null);
      IEnumerable<TreeNode> rootNodes = parser.ParseFromStrings(yamls);
      return CompilePrivate(rootNodes);
    }

    public List<Entity> Compile(string dirPath) {
      // Recursively parse entire directory
      Parser parser = new ParserYaml(_messages, dirPath);
      List<TreeNode> rootNodes = parser.RecursivelyParseDirectory().Cast<TreeNode>().ToList();
      return CompilePrivate(rootNodes);
    }

    private List<Entity> CompilePrivate(IEnumerable<TreeNode> rootNodes) { 
      // Pass 1 -------------------------------
      List<Entity> entities = new List<Entity>();
      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enums = new EnumsCompiler(_messages, _allEnums, attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(_messages, enums, attrReader);

      foreach (TreeNode rootNode in rootNodes) {
        if (IsEnumFile(rootNode.FileInfo.FilePath))
          enums.CompileEnumFile(rootNode);
        else if (IsFunctionFile(rootNode.FileInfo.FilePath)) {
          // Do nothing
        } else {
          Entity entity = pass1.CompileEntity(rootNode);
          if (!string.IsNullOrWhiteSpace(entity?.Name))
            entities.Add(entity);
        }
      }

      // Pass 2 ---------------------------------
      AllEntities allEntities = new AllEntities(_messages, entities);

      // Functions
      FunctionsCompiler functions = new FunctionsCompiler(_messages, allEntities, _allEnums, _allFunctions, attrReader);
      foreach (TreeNode rootNode in rootNodes) 
        if (IsFunctionFile(rootNode.FileInfo.FilePath))
          functions.CompileFunctionsFile(rootNode);

      EntityCompilerPass2 pass2 = new EntityCompilerPass2(_messages, allEntities, _allEnums, _allFunctions);
      pass2.CompileAllEntities();

      return entities;
    }

    private static bool IsEnumFile(string path) {
      return path.ToLower().EndsWith("enums.yaml");
    }

    private static bool IsFunctionFile(string path) {
      return path.ToLower().EndsWith("functions.yaml");
    }
  }
}