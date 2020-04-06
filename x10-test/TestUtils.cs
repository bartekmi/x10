using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using x10.model;
using x10.compiler;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace x10 {
  public class TestUtils {
    public static void DumpMessages(MessageBucket messages, ITestOutputHelper output, CompileMessageSeverity? severities = null) {
      if (messages.IsEmpty)
        output.WriteLine("No Errors");
      else
        foreach (CompileMessage message in messages.FilteredMessages(severities))
          output.WriteLine(message.ToString());
    }

    public static Entity EntityCompilePass1(MessageBucket _messages, AllEnums _allEnums, string yaml, string fileName = null) {
      // Parse
      ParserYaml parser = new ParserYaml(_messages);
      TreeNode rootNode = parser.ParseFromString(yaml);
      if (rootNode == null)
        throw new Exception("Unalbe to parse yaml from: " + yaml);

      rootNode.SetFileInfo(ExtractFileName(rootNode, fileName));

      // Pass 1
      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enums = new EnumsCompiler(_messages, _allEnums, attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(_messages, enums, attrReader);
      Entity entity = pass1.CompileEntity(rootNode);

      return entity;
    }

    private static string ExtractFileName(TreeNode rootNode, string overrideFileName) {
      if (overrideFileName != null)
        return overrideFileName;

      string entityName = (rootNode as TreeHash)?.FindValue("name")?.ToString();
      if (entityName == null)
        return "Tmp.yaml";

      return entityName + ".yaml";
    }


    public static void EntityCompilePass2(MessageBucket _messages, AllEnums _allEnums, params Entity[] entities) {
      AllEntities allEntities = new AllEntities(entities, _messages);
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(_messages, allEntities, _allEnums);
      pass2.CompileAllEntities();
    }

    public static AllEntities EntityCompile(MessageBucket messages, params string[] yamls) {
      AllEnums allEnums = new AllEnums(messages);

      IEnumerable<Entity> entities = yamls.Select(x => EntityCompilePass1(messages, allEnums, x));
      Entity[] entitiesArray = entities.ToArray();
      EntityCompilePass2(messages, allEnums, entitiesArray);

      return new AllEntities(entitiesArray, messages);
    }
  }
}
