﻿using System;
using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.ui.metadata;
using x10.compiler.ui;

namespace x10.compiler {
  public class TopLevelCompiler {

    private readonly MessageBucket _messages;
    private readonly IEnumerable<UiLibrary> _libraries;

    public TopLevelCompiler(MessageBucket messages, IEnumerable<UiLibrary> libraries) {
      _messages = messages;
      _libraries = libraries;
    }

    internal void Compile(string rootDir, out AllEntities allEntities, out AllEnums allEnums, out AllFunctions allFunctions, out AllUiDefinitions allUiDefinitions) {
      // Parse Entities
      allEnums = new AllEnums(_messages);
      allFunctions = new AllFunctions(_messages);

      EntitiesAndEnumsCompiler entityCompiler = new EntitiesAndEnumsCompiler(_messages, allEnums, allFunctions);
      List<Entity> entities = entityCompiler.Compile(rootDir);
      allEntities = new AllEntities(_messages, entities);

      // Parse Xml Files
      ParserXml parser = new ParserXml(_messages, rootDir);
      IEnumerable<XmlElement> rootXmlElements = parser.RecursivelyParseDirectory()
        .Cast<XmlElement>();

      // UI Compile - Pass 1
      UiAttributeReader attrReaderForPass1 = new UiAttributeReader(_messages, allEntities, allEnums, null);
      UiCompilerPass1 uiPass1 = new UiCompilerPass1(_messages, attrReaderForPass1);

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
      UiAttributeReader attrReaderForPass2 = new UiAttributeReader(_messages, allEntities, allEnums, allUiDefinitions);
      UiCompilerPass2 uiPass2 = new UiCompilerPass2(_messages, attrReaderForPass2, allEntities, allEnums, allUiDefinitions, allFunctions);
      uiPass2.CompileAllUiDefinitions();

      // Post-Compile Transformations
      PostCompileTransformations.PostCompile(allEntities, allEnums, allUiDefinitions, allFunctions);
    }
  }
}
