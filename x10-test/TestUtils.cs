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
using x10.ui.composition;
using x10.ui.metadata;
using FileInfo = x10.parsing.FileInfo;

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

    #region Model Compilation
    public static Entity EntityCompilePass1(MessageBucket _messages, AllEnums _allEnums, string yaml, string fileName = null) {
      // Parse
      ParserYaml parser = new ParserYaml(_messages, null);
      TreeNode rootNode = parser.ParseFromString(yaml);
      if (rootNode == null)
        throw new Exception("Unable to parse yaml from: " + yaml);

      rootNode.SetFileInfo(ExtractFileInfo(rootNode, fileName));

      // Pass 1
      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enums = new EnumsCompiler(_messages, _allEnums, attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(_messages, enums, attrReader);
      Entity entity = pass1.CompileEntity(rootNode);

      return entity;
    }

    public static FileInfo ExtractFileInfo(TreeNode rootNode, string overrideFileName) {
      string filename;

      if (overrideFileName == null) {
        string entityName = (rootNode as TreeHash)?.FindValue("name")?.ToString();
        if (entityName != null)
          filename = entityName + ".yaml";
        else
          filename = "Tmp.yaml";
      } else
        filename = overrideFileName;

      return FileInfo.FromFilename(filename);
    }


    public static void EntityCompilePass2(MessageBucket messages, AllEnums allEnums, params Entity[] entities) {
      AllEntities allEntities = new AllEntities(messages, entities);
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(messages, allEntities, allEnums, null);
      pass2.CompileAllEntities();
    }

    public static AllEntities EntityCompile(MessageBucket messages, AllEnums allEnums, params string[] yamls) {
      IEnumerable<Entity> entities = yamls.Select(x => EntityCompilePass1(messages, allEnums, x));
      Entity[] entitiesArray = entities.ToArray();
      EntityCompilePass2(messages, allEnums, entitiesArray);

      return new AllEntities(messages, entitiesArray);
    }
    #endregion

    #region UI Compilation
    public static ClassDefX10 UiCompilePass1(string xml, 
      MessageBucket messages,
      UiCompilerPass1 compiler,
      ITestOutputHelper output,
      string fileName = null
      ) {

      ParserXml parser = new ParserXml(messages, null);

      XmlElement rootNode = parser.ParseFromString(xml);

      if (rootNode == null) {
        DumpMessages(messages, output);
        Assert.NotNull(rootNode);
      }

      rootNode.SetFileInfo(ExtractFileInfo(rootNode, fileName));

      ClassDefX10 definition = compiler.CompileUiDefinition(rootNode);
      return definition;
    }

    private static FileInfo ExtractFileInfo(XmlElement rootNode, string overrideFileName) {
      string filename;
      if (overrideFileName == null)
        filename = rootNode.Name + ".xml";
      else
        filename = overrideFileName;

      return FileInfo.FromFilename(filename);
    }

    public static void UiCompilePass2(MessageBucket messages,
      AllEntities allEntities,
      AllEnums allEnums,
      UiLibrary uiLibrary,
      params ClassDefX10[] uiDefinitions
      ) {
      AllUiDefinitions allUiDefinitions = new AllUiDefinitions(messages, uiDefinitions, new UiLibrary[] { uiLibrary });

      UiAttributeReader attrReader = new UiAttributeReader(messages, allEntities, allEnums, allUiDefinitions);
      UiCompilerPass2 pass2 = new UiCompilerPass2(messages, attrReader, allEntities, allEnums, allUiDefinitions, new AllFunctions(messages));
      pass2.CompileAllUiDefinitions();
    }

    #endregion
  }
}
