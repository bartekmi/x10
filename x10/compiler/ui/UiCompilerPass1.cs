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
      ClassDefX10 definition = new ClassDefX10(rootNode);

      // Read top-level (entity) attributes
      _attrReader.ReadAttributesForClassDef(definition);

      // Top-level definition node generally should have exactly one child node
      // We allow for a non-node placeholder, but with a warning.
      int topLevelChildCount = rootNode.Children.Count;
      if (topLevelChildCount == 0) {
        _messages.AddWarning(rootNode,
          String.Format("UI Component definition '{0}' contains no children. It will not be rendered as a visual component",
          rootNode.Name));
        return definition;
      }
      if (topLevelChildCount > 1) {
        _messages.AddError(rootNode,
          String.Format("UI Component definition '{0}' has multiple children.",
          rootNode.Name));
        return null;
      }

      XmlElement rootChild = rootNode.Children.Single();

      return definition;
    }
  }
}
