﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using x10.parsing;
using x10.model.metadata;
using x10.model.definition;

namespace x10.compiler {
  public class EntitiesAndEnumsCompiler {

    public MessageBucket Messages { get; private set; }

    public EntitiesAndEnumsCompiler() {
      Messages = new MessageBucket();   // Testing can by-pass Compile()
    }

    public List<Entity> Compile(string dirPath) {
      Messages.Clear();   // Always empty the bucket

      // Recursively parse entire directory
      Parser parser = new ParserYaml();
      List<TreeNode> rootNodes = parser.RecursivelyParseDirectory(dirPath);

      // Pass 1
      List<Entity> entities = new List<Entity>();
      AttributeReader attrReader = new AttributeReader(Messages);
      EnumsCompiler enums = new EnumsCompiler(Messages, attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(Messages, enums, attrReader);

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
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(Messages, entities);
      pass2.CompileAllEntities();

      return entities;
    }

    private static bool IsEnumFile(string path) {
      return path.ToLower().EndsWith("enums.yaml");
    }
  }
}