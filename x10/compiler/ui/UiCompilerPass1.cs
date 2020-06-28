using System;
using System.Collections.Generic;
using System.Linq;
using x10.model;
using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  // The first pass does very little - creates a 'Class Def' and parses the top-level attributes
  // It also validates that there should be a single Root Xml node
  // FUTURE: Register declared UI Attribute Definitions
  public class UiCompilerPass1 {

    private readonly MessageBucket _messages;
    private readonly UiAttributeReader _attrReader;

    internal UiCompilerPass1(MessageBucket messages, UiAttributeReader attributeReader) {
      _messages = messages;
      _attrReader = attributeReader;
    }

    internal ClassDefX10 CompileUiDefinition(XmlElement rootNode) {
      ClassDefX10 definition = new ClassDefX10(rootNode) {
        InheritsFrom = ClassDefNative.Visual,
      };

      // Read top-level (entity) attributes
      _attrReader.ReadAttributesForClassDef(definition);

      // TODO: What's the point anymore?
      // Perhaps we can separate Compiler 2.1 and 2.2

      return definition;
    }
  }
}
