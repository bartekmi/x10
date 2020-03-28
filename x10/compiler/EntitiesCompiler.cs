using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EntitiesCompiler {

    public MessageBucket Messages { get; private set; }

    public EntitiesCompiler() {
      Messages = new MessageBucket();   // Testing can by-pass Compile()
    }

    public List<Entity> Compile(string dirPath) {
      Messages.Clear();   // Always empty the bucket

      // Recursively parse entire directory
      Parser parser = new ParserYaml();
      List<TreeNode> rootNodes = parser.RecursivelyParseDirectory(dirPath);

      // Pass 1
      List<Entity> entities = new List<Entity>();
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(Messages);

      foreach (TreeNode rootNode in rootNodes) {
        Entity entity = pass1.CompileEntity(rootNode);
        if (!string.IsNullOrWhiteSpace(entity?.Name))
          entities.Add(entity);
      }

      // TODO: Check uniqueness of Entity names
      // TODO: Check uniqueness of Enum names (pending collecting enums from enum def files)

      // Pass 2
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(Messages, entities);
      pass2.CompileAllEntities();

      return entities;
    }
  }
}