using System;
using System.Collections.Generic;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.ui.composition;
using System.Linq;
using x10.ui.metadata;

namespace x10.compiler {
  public class TopLevelCompiler {

    private readonly MessageBucket _messages;
    private readonly IEnumerable<UiLibrary> _libraries;

    public TopLevelCompiler(MessageBucket messages, IEnumerable<UiLibrary> libraries) {
      _messages = messages;
      _libraries = libraries;
    }

    internal void Compile(string rootDir, out AllEntities allEntities, out AllEnums allEnums, out AllUiDefinitions allUiDefinitions) {
      // Parse Entities
      allEnums = new AllEnums(_messages);
      EntitiesAndEnumsCompiler entityCompiler = new EntitiesAndEnumsCompiler(_messages, allEnums);
      List<Entity> entities = entityCompiler.Compile(rootDir);
      allEntities = new AllEntities(entities, _messages);

      // Parse Xml Files
      ParserXml parser = new ParserXml(_messages);
      IEnumerable<XmlElement> rootXmlElements = parser.RecursivelyParseDirectory(rootDir)
        .Cast<XmlElement>();

      // UI Compile - Pass 1
      UiCompilerPass1 uiPass1 = new UiCompilerPass1(_messages,
        new UiAttributeReader(_messages, allEntities, allEnums));

      List<ClassDefX10> classDefs = new List<ClassDefX10>();
      foreach (var rootXmlElement in rootXmlElements) {
        ClassDefX10 classDef = uiPass1.CompileUiDefinition(rootXmlElement);
        classDefs.Add(classDef);
      }
      allUiDefinitions = new AllUiDefinitions(_messages, classDefs, _libraries);

      // Entities - Pass 3
      EntityCompilerPass3 entitiesPass3 = new EntityCompilerPass3(allUiDefinitions);
      entitiesPass3.CompileAllEntities(allEntities);

      // UI Compile - Pass 2
      UiAttributeReader attrReader = new UiAttributeReader(_messages, allEntities, allEnums);
      UiCompilerPass2 uiPass2 = new UiCompilerPass2(_messages, attrReader, allEntities, allEnums, allUiDefinitions);
      uiPass2.CompileAllUiDefinitions();
    }
  }
}
