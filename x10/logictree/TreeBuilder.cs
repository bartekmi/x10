using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using x10.schema;
using x10.complib;
using x10.error;

namespace x10.logictree {
    public static class TreeBuilder {
        public static ElementDef BuildPass1(string path, out ErrorBucket errors) {
            errors = new ErrorBucket();
            ElementDef elementDef = null;

            try {
                // Top-level of XML document
                XDocument doc = XDocument.Load(path, LoadOptions.SetLineInfo);
                XElement root = doc.Root;

                elementDef = new ElementDef(root, root.Name.LocalName) {
                    Path = path
                };
                elementDef.PostProcess(errors, null);
            } catch (XmlException e) {
                errors.Add(new Error() {
                    Message = e.Message,
                    Line = e.LineNumber,
                    LinePosition = e.LinePosition,
                });
            }

            // Error housekeeping
            foreach (Error error in errors.Errors)
                error.Path = path;

            return elementDef;
        }

        public static void BuildPass2(ElementDef elementDef, out ErrorBucket errors) {
            errors = new ErrorBucket();

            // Build recursively in two passes
            BuildRecursive(elementDef, elementDef.XElement);
            PostProcessRecursive(errors, elementDef, null);

            // Error housekeeping
            foreach (Error error in errors.Errors)
                error.Path = elementDef.Path;
        }

        private static void BuildRecursive(ElementBase element, XElement node) {
            // Process children
            foreach (XElement xmlChild in node.Elements()) {
                ElementBase child = CreateElementFromXml(xmlChild);
                element.Children.Add(child);
                BuildRecursive(child, xmlChild);
            }
        }

        private static ElementBase CreateElementFromXml(XElement xml) {
            string name = xml.Name.LocalName;
            if (char.IsLower(name[0]))
                return new ElementPrimitive(xml, name);
            else
                return new ElementUse(xml, name);
        }

        private static void PostProcessRecursive(ErrorBucket errors, ElementBase element, ElementBase parent) {
            element.PostProcess(errors, parent);
            foreach (ElementBase child in element.Children)
                PostProcessRecursive(errors, child, element);
        }
    }
}