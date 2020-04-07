using System;
using System.Collections.Generic;
using System.Linq;
using x10.parsing;
using x10.ui.composition;
using x10.ui.metadata;

namespace x10.compiler {
  public class UiCompilerPass1 {

    private readonly MessageBucket _messages;
    private readonly UiAttributeReader _attrReader;

    internal UiCompilerPass1(MessageBucket messages, UiAttributeReader attributeReader) {
      _messages = messages;
      _attrReader = attributeReader;
    }

    internal UiDefinitionX10 CompileUiDefinition(XmlElement rootNode) {
      UiDefinitionX10 definition = new UiDefinitionX10();

      // Read top-level (entity) attributes
      _attrReader.ReadAttributes(rootNode, UiAppliesTo.UiDefinition, definition);

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
      definition.RootChild = ParseRecursively(rootChild);

      return definition;
    }

    private UiChild ParseRecursively(XmlElement xmlElement) {
      if (IsUiModelReference(xmlElement)) {  // Model Reference (starts with lower-case)
        UiChildModelReference uiElement = new UiChildModelReference();
        _attrReader.ReadAttributes(xmlElement, UiAppliesTo.UiModelReference, uiElement);

        return uiElement;
      } else {  // Component Use (starts with upper-case)
        UiChildComponentUse uiElement = new UiChildComponentUse();
        _attrReader.ReadAttributes(xmlElement, UiAppliesTo.UiComponentUse, uiElement);

        foreach (XmlElement xmlChild in xmlElement.Children) {
          uiElement.AddChild(ParseRecursively(xmlChild));
        }

        return uiElement;
      }
    }

    private bool IsUiModelReference(XmlElement element) {
      return char.IsLower(element.Name[0]);
    }
  }
}
