using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;

using x10.complib;
using x10.error;
using x10.schema;

namespace x10.logictree {
    public abstract class ElementBase {

        internal abstract void PostProcess(ErrorBucket errors, ElementBase parent);
        public virtual void PrettyPrint(StringBuilder builder, int indent) {
            builder.AppendLine(string.Format("{0}{1}", new string(' ', indent * 4), this));
            foreach (ElementBase child in Children)
                child.PrettyPrint(builder, indent + 1);
        }

        internal string Name { get; private set; }
        internal XElement XElement { get; private set; }

        public ComponentDef Component { get; set; }
        public List<ParamValue> ParamValues { get; set; }
        public List<ElementBase> Children { get; set; }
        public Entity DataModel { get; protected set; }
        public bool IsDataModelMultiple { get; protected set; }

        // Derived
        protected IEnumerable<XAttribute> Attributes { get { return XElement.Attributes(); } }

        internal ElementBase(XElement xElement, string name) {
            XElement = xElement;
            Name = name;

            ParamValues = new List<ParamValue>();
            Children = new List<ElementBase>();
        }

        #region Helpers
        protected void AddError(ErrorBucket errors, string message) {
            IXmlLineInfo xmlLineInfo = (IXmlLineInfo)XElement;
            errors.Add(new Error() {
                Message = message,
                Line = xmlLineInfo.LineNumber,
                LinePosition = xmlLineInfo.LinePosition,
            });
        }

        protected string GetAttribute(string attributeName) {
            XAttribute attribute = Attributes.SingleOrDefault(x => x.Name.LocalName == attributeName);
            return attribute == null ? null : attribute.Value.Trim();
        }

        protected string GetMandatoryAttribute(ErrorBucket errors, string attributeName) {
            string value = GetAttribute(attributeName);
            if (value == null)
                AddError(errors, string.Format("Missing mandatory attribute {0}.{1}", Name, attributeName));
            return value;
        }

        protected Dictionary<string, string> GetAttributes(ErrorBucket errors, params string[] exclude) {
            Dictionary<string, string> namesAndValues = new Dictionary<string, string>();

            foreach (XAttribute attribute in Attributes) {
                string name = attribute.Name.LocalName;
                if (exclude.Contains(name))
                    continue;
                if (namesAndValues.ContainsKey(name))
                    AddError(errors, string.Format("Duplicate attribute {0} on Element {1}", name, Name));
                else
                    namesAndValues[name] = attribute.Value;
            }

            return namesAndValues;
        }

        public override string ToString() {
            return string.Format("{0} -  UI = {1}", Name, Component);
        }
    }
    #endregion
}