using System;
using System.Collections.Generic;
using System.Linq;
using x10.model;
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

    internal ClassDefX10 CompileUiDefinition(XmlElement rootNode) {
      ClassDefX10 definition = new ClassDefX10();

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

    private Instance ParseRecursively(XmlElement xmlElement) {
      Instance uiElement;
      UiAppliesTo? appliesTo;

      if (IsModelReference(xmlElement)) {  // Model Reference (starts with lower-case)
        uiElement = new InstanceModelRef();
        appliesTo = UiAppliesTo.UiModelReference;
      } else if (IsUiDefinitionUse(xmlElement)) { 
        uiElement = new InstanceClassDefUse();
        appliesTo = UiAppliesTo.UiComponentUse;

        foreach (XmlElement xmlChild in xmlElement.Children)
          ((InstanceClassDefUse)uiElement).AddChild(ParseRecursively(xmlChild));
      } else if (IsComplexAttribute(xmlElement)) {
        throw new NotImplementedException();
      } else {
        _messages.AddError(xmlElement,
          string.Format("Xml Element name '{0}' was not recognized as either a Entity *memberName* or a *UiComponentName* or as a *Complex.property*",
          xmlElement.Name));
        return null;
      }

      uiElement.XmlElement = xmlElement;
      _attrReader.ReadAttributes(xmlElement, appliesTo.Value, uiElement);

      return uiElement;
    }

    private bool IsModelReference(XmlElement element) {
      return ModelValidationUtils.IsMemberName(element.Name);
    }

    private bool IsComplexAttribute(XmlElement element) {
      string parentElementName = ((XmlElement)element.Parent)?.Name;
      if (parentElementName == null)
        return false;

      return element.Name.StartsWith(parentElementName + ".");
    }

    private bool IsUiDefinitionUse(XmlElement element) {
      return ModelValidationUtils.IsUiElementName(element.Name);
    }
  }
}
