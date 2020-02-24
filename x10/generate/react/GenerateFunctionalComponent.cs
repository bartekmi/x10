using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using x10.logictree;
using x10.complib;
using x10.schema;
using x10.schema.validation;
using x10.utils;

namespace x10.generate.react {

    public static class GenerateFunctionalComponent {

        #region Top Level
        public static void Generate(ElementDef elementDef, bool doTranslation) {
            string noExtension = Path.GetFileNameWithoutExtension(elementDef.Path);
            string outFile = noExtension + ".jsx";

            using (TextWriter writer = GenerateUtils.CreateWriter(outFile)) {
                GenerateUtils.GenerateHeader(writer, elementDef, Flow.SrictLocal);
                GenerateImports(writer, elementDef, doTranslation);
                GenerateProps(writer, elementDef);
                GenerateFunction(writer, elementDef, doTranslation);
                GenerateItemDisplayFunctionIfNeeded(writer, elementDef, doTranslation);
            }
        }
        #endregion

        #region Imports
        private static void GenerateImports(TextWriter writer, ElementDef elementDef, bool doTranslation) {
            // 3rd-party imports
            writer.WriteLine("import * as React from \"react\";");
            writer.WriteLine();

            // Latitude / Component imports
            // Note that not all components have Labels and InputError's
            writer.WriteLine("// eslint-disable-next-line autofix/no-unused-vars");
            writer.WriteLine("import Label from \"latitude/Label\";");
            writer.WriteLine("// eslint-disable-next-line autofix/no-unused-vars");
            writer.WriteLine("import InputError from \"latitude/InputError\";");

            IEnumerable<ComponentDef> components = ElementUtils.Collect<ElementBase>(elementDef)
                .Where(x => x.Component != null && x != elementDef)
                .Select(x => x.Component.PhysicalComponent)
                .Distinct();

            foreach (ComponentDef component in components)
                writer.WriteLine("import {0} from \"{1}\";",
                    component.Name,
                    component.ImportFile);
            writer.WriteLine();

            // Internationalization
            if (doTranslation)
                writer.WriteLine(string.Format("import {{ {0}T as t}} from \"config/I18n\"",
                    elementDef.I18n));

            // Validations
            IEnumerable<string> validationFunctions = ElementUtils.Collect<ElementPrimitive>(elementDef)
                .SelectMany(x => x.Property.Validations)
                .Select(x => x.Function)
                .Distinct();

            if (validationFunctions.Count() > 0)
                writer.WriteLine(string.Format("import {{ {0} }} from \"tools/faml/famlValidations\"",
                    string.Join(", ", validationFunctions)));

            // Local 
            // DataModel Type 
            writer.WriteLine(string.Format("import type {{ {0} }} from \"{1}\"",
                GenerateUtils.GetFlowTypeName(elementDef.DataModel),
                GenerateUtils.GetLocalImportPath(GenerateTypes.TYPES_FILE_NAME)));

            // Initial Data function(s) for onAddItem() for multi-components
            IEnumerable<Entity> multiEntities = ElementUtils.Collect<ElementUse>(elementDef)
                .Where(x => x.Component.Params.Any(p => p.Type == ParamType.VarOnAddItem))
                .Select(x => x.DataModel)
                .Distinct();
            if (multiEntities.Count() > 0) {
                string[] multiInitialFunctions = multiEntities
                    .Select(x => GenerateInitialData.GetFunctionName(x))
                    .ToArray();

                writer.WriteLine(string.Format("import {{ {0} }} from \"{1}\"",
                    string.Join(", ", multiInitialFunctions),
                    GenerateUtils.GetLocalImportPath(GenerateInitialData.INITIAL_DATA_FILE_NAME)));
            }
        }
        #endregion

        #region Props
        private static void GenerateProps(TextWriter writer, ElementDef elementDef) {
            string entityType = GenerateUtils.GetFlowTypeName(elementDef.DataModel);
            string dataType = elementDef.IsDataModelMultiple ?
                string.Format("$ReadOnlyArray<{0}>", entityType) : entityType;

            writer.WriteLine(@"
type Props = {{|
  +data: {0},
  +showErrors: boolean,
  +onDataChanged: (newData: {0}) => void,
|}};", dataType);
            writer.WriteLine();
        }
        #endregion

        #region Function 
        #region Function - Top Level
        private static void GenerateFunction(TextWriter writer, ElementDef elementDef, bool doTranslation) {
            writer.WriteLine(@"
export default function {0}({{
    data,
    showErrors,
    onDataChanged
}}: Props) {{", elementDef.Name);

            GenerateExtractFieldsFromData(writer, elementDef);
            GenerateReturnStatement(writer, elementDef, doTranslation);

            writer.WriteLine("}");
        }
        #endregion

        #region Function - const {...} = <data>
        private static void GenerateExtractFieldsFromData(TextWriter writer, ElementBase element) {
            // Pull out all fields which have Primitive Elements in this Form
            IEnumerable<ElementPrimitive> fields =
                ElementUtils.Collect<ElementPrimitive>(element,
                    e => e.Component.Params.Any(x => x.Type == ParamType.VarChildAsCreateFunc))
                .Distinct();

            // Pull out all fields which have Use Elements with a Path
            IEnumerable<ElementUse> usesWithPath =
                ElementUtils.Collect<ElementUse>(element)
                .Where(x => x.Path != null)
                .Distinct();

            if (fields.Count() > 0 || usesWithPath.Count() > 0) {
                writer.WriteLine("  const {");

                foreach (ElementPrimitive field in fields)
                    writer.WriteLine("    {0},", field.Name);

                foreach (ElementUse field in usesWithPath)
                    writer.WriteLine("    {0},", field.Path);

                writer.WriteLine("  } = data;");
                writer.WriteLine();
            }
        }
        #endregion

        #region Return Statement - Top Level
        private static void GenerateReturnStatement(TextWriter writer, ElementDef elementDef, bool doTranslation) {
            writer.WriteLine("  return (");
            GenerateElement(writer, elementDef, 2, doTranslation);
            writer.WriteLine("  );");
        }

        private static void GenerateElement(TextWriter writer, ElementBase element, int indent, bool doTranslation) {
            if (element is ElementDef)
                GenerateElementDef(writer, element as ElementDef, indent, doTranslation);
            else if (element is ElementUse)
                GenerateElementUse(writer, element as ElementUse, indent, doTranslation);
            else if (element is ElementPrimitive)
                GenerateElementPrimitive(writer, element as ElementPrimitive, indent, doTranslation);
            else
                throw new Exception("Update this code!");
        }
        #endregion

        #region Return Statement - ElementDef
        private static void GenerateElementDef(TextWriter writer, ElementDef element, int indent, bool doTranslation) {
            // TODO... If styling needed, make this a div instead of a <>
            OpenTag(writer, "", indent, element);
            foreach (ElementBase child in element.Children)
                GenerateElement(writer, child, indent + 1, doTranslation);
            CloseTag(writer, "", indent);
        }
        #endregion

        #region Return Statement - ElementUse
        private const string ITEM_DISPLAY_FUNCTION = "itemDisplayFunction";

        private static void GenerateElementUse(TextWriter writer, ElementUse element, int indent, bool doTranslation) {
            string tag = element.Component.PhysicalComponent.Name;

            OpenTag(writer, tag, indent, element, CreateSpecialAttributes(element, indent + 1));

            bool childIsTemplate = element.Component.Params.Any(x => x.Type == ParamType.VarChildAsCreateFunc);
            if (!childIsTemplate)
                foreach (ElementBase child in element.Children)
                    GenerateElement(writer, child, indent + 1, doTranslation);

            CloseTag(writer, tag, indent);
        }

        private static Attr[] CreateSpecialAttributes(ElementUse element, int indent) {
            List<Attr> specialAttributes = new List<Attr>();

            foreach (ParamDef paramDef in element.Component.Params) {
                string value = null;
                switch (paramDef.Type) {
                    case ParamType.VarShowErrors:
                        value = "showErrors";
                        break;
                    case ParamType.VarInputData:
                        value = element.Path == null ? "data" : element.Path;
                        break;
                    case ParamType.VarCallback:
                        if (element.Path == null)
                            value = "onDataChanged";
                        else
                            value = String.Format(@"newData => {{
          onDataChanged({{
            ...data,
            {0}: newData,
          }});
        }}", element.Path);


                        break;
                    case ParamType.VarOnAddItem:
                        value = GenerateInitialData.GetFunctionName(element.DataModel);
                        break;
                    case ParamType.VarChildAsCreateFunc:
                        value = ITEM_DISPLAY_FUNCTION;
                        break;
                    case ParamType.VarSubstitute:
                        ParamValue paramValue = element.ParamValues.SingleOrDefault(x => x.Param == paramDef);
                        if (paramValue != null)
                            value = paramDef.Template.Replace("PROPTEXT", paramValue.Value.ToString());
                        break;
                }

                if (value != null)
                    specialAttributes.Add(new AttrExp(paramDef.Name, value));
            }

            return specialAttributes.ToArray();
        }

        #region Item Display Function
        private static void GenerateItemDisplayFunctionIfNeeded(TextWriter writer, ElementDef elementDef, bool doTranslation) {
            ElementUse multiElement = ElementUtils.Collect<ElementUse>(elementDef)
                .SingleOrDefault(x => x.Component.Params.Any(p => p.Type == ParamType.VarChildAsCreateFunc));

            if (multiElement != null) {
                writer.WriteLine();
                ElementUse templateElement = (ElementUse)multiElement.Children.Single();
                GenerateItemDisplayFunction(writer, templateElement, doTranslation);
            }
        }

        private static void GenerateItemDisplayFunction(TextWriter writer, ElementUse element, bool doTranslation) {
            string entityType = GenerateUtils.GetFlowTypeName(element.DataModel);

            writer.WriteLine(
@"function {0}(props: {{
  +data: {1},
  +showErrors: boolean,
  +onDataChanged: (item: {1}) => void,
}}) {{
  const {{
    data,
    showErrors,
    onDataChanged
  }} = props;", ITEM_DISPLAY_FUNCTION, entityType);

            writer.WriteLine();
            GenerateExtractFieldsFromData(writer, element);

            writer.WriteLine("    return (");
            string tag = element.Component.PhysicalComponent.Name;

            OpenTag(writer, tag, 2, element, CreateSpecialAttributes(element, 3));

            foreach (ElementBase child in element.Children)
                GenerateElement(writer, child, 3, doTranslation);

            CloseTag(writer, tag, 2);

            writer.WriteLine(
@"  );
}");
        }
        #endregion
        #endregion

        #region Return Statement - Element Primitive
        private static void GenerateElementPrimitive(
            TextWriter writer,
            ElementPrimitive element,
            int indent,
            bool doTranslation) {

            AttrLit requiredOptionalAttr = null;
            if (element.Component.UserCantClear) {
                // Since the UI component always has a value, indicating required/optional status is meaningless
            } else {
                requiredOptionalAttr = new AttrLit(
                    element.Property.IsMandatory ? "indicateRequired" : "indicateOptional",
                    true
                );
            }

            bool doLabel = element.Component.DefaultLabelPlacement != LabelPlacement.None;
            bool isConditional = element.Property.IsRelevant != null;

            if (isConditional)
                writer.WriteLine("{0}{{({1}) ? (", Indent(indent++), element.Property.IsRelevant);

            if (doLabel)
                OpenTag(writer, "Label", indent,
                    new AttrLit("value", element.Property.Label, doTranslation),
                    requiredOptionalAttr);

            if (element.Property.HasValidations && !CanSkipValidation(element))
                GenerateInputError(writer, element, indent + 1, doTranslation);
            else
                GenerateInputElement(writer, element, indent + 1);

            if (doLabel)
                CloseTag(writer, "Label", indent);

            if (isConditional)
                writer.WriteLine("{0}) : null}}", Indent(--indent));
        }

        // If the only validation is for a mandatory element, but the physical UI component
        // can't even have an empty value, we can skip validation
        private static bool CanSkipValidation(ElementPrimitive element) {
            IEnumerable<Validation> validations = element.Property.Validations;
            if (validations.Count() > 1)
                return false;

            Validation validation = validations.Single();
            if (validation is ValidationMandatory)
                if (element.Component.UserCantClear &&
                    element.Property.HasDefault)
                    return true;

            return false;
        }

        private static void GenerateInputError(TextWriter writer, ElementPrimitive element, int indent, bool doTranslation) {
            // TODO... Need more robust validation
            Validation validation = element.Property.Validations.First();
            string showError = string.Format("showErrors && !{0}({1})",
                validation.Function,
                element.Property.Name);

            OpenTag(writer, "InputError", indent,
                new AttrLit("errorText", validation.ErrorMessage, doTranslation),
                new AttrExp("showError", showError));

            GenerateInputElement(writer, element, indent + 1);
            CloseTag(writer, "InputError", indent);
        }

        private static void GenerateInputElement(TextWriter writer, ElementPrimitive element, int indent) {
            List<Attr> attributes = new List<Attr>() {
                new AttrExp("value", ValueExpression(element)),
                new AttrExp("onChange", OnChangeFunction(element)),
            };

            // Some components will set some of their attributes based on the property from the schema
            // For example, SelectInput will set its isNullable property based on whether the 
            // Schema property is mandatory
            var extraParamsFunc = element.Component.ExtraParamsFunc;
            if (extraParamsFunc != null) {
                foreach (ParamValue paramValue in extraParamsFunc(element.Property)) {
                    string expression = null;
                    if (paramValue.Value is X10Enum)
                        expression = BuildOptions(paramValue.Value as X10Enum);
                    else
                        expression = paramValue.Value.ToString();

                    attributes.Add(new AttrExp(paramValue.Name, expression));
                }
            }

            Tag(writer, element.Component.Name, indent, element, attributes.ToArray());
        }

        private static string ValueExpression(ElementPrimitive element) {
            Property property = element.Property;
            ComponentDef component = element.Component;
            string suffix = "";

            switch (component.NullTreatment) {
                case NullTreatment.NotApplicable:
                    throw new Exception("Should never happen because this means a non-UI component: " + component.Name);
                case NullTreatment.NeverAllowsNull:
                    if (!property.IsMandatory)
                        throw new Exception(string.Format("Should never happen because this generates compile error. Property: {0}, Component: {1}",
                            property, component));
                    break;
                case NullTreatment.ConvertNullToDefault:
                    if (!property.IsMandatory) {
                        if (property.Type.NullValueAlternative == null) {
                            throw new Exception(string.Format("DataType {0} does not allow a null value alternative. Property: {1}, Component: {2}",
                                property.Type, property, component));
                        }
                        suffix = " || " + property.Type.NullValueAlternative;
                    }
                    break;
                case NullTreatment.AcceptsAndReturnsNull:
                    break;
                default:
                    throw new Exception("Fix your code");
            }

            return property.Name + suffix;
        }

        private static string OnChangeFunction(ElementPrimitive element) {
            return String.Format(
                @"newValue =>
                  onDataChanged({{
                    ...data,
                    {0}: newValue,
                  }})",
                  element.Property.Name);
        }
        #endregion

        #region Return Statement - Utils

        private static void OpenTag(TextWriter writer, string tag, int indent, ElementBase element) {
            WriteTag(writer, tag, indent, false, element);
        }

        private static void OpenTag(TextWriter writer, string tag, int indent, params Attr[] attributes) {
            WriteTag(writer, tag, indent, false, attributes);
        }

        private static void OpenTag(TextWriter writer, string tag, int indent, ElementBase element, params Attr[] attributes) {
            WriteTag(writer, tag, indent, false, element, attributes);
        }

        private static void Tag(TextWriter writer, string tag, int indent, ElementBase element, params Attr[] attributes) {
            WriteTag(writer, tag, indent, true, element, attributes);
        }

        private static void WriteTag(TextWriter writer, string tag, int indent, bool isComplete, ElementBase element, params Attr[] extraAttributes) {
            Attr[] attributes = element.ParamValues
                .Where(x => x.Param.Type != ParamType.VarSubstitute)
                .Select(x => new AttrLit(x.Param.Name, x.Value, x.Param.NeedsTranslation))
                .Concat(extraAttributes)
                .ToArray();

            WriteTag(writer, tag, indent, isComplete, attributes);
        }

        private static void WriteTag(TextWriter writer, string tag, int indent, bool isComplete, params Attr[] attributes) {
            string singleLine = WriteTag(tag, indent, isComplete, false, attributes);
            if (singleLine.Length <= 80) {
                writer.Write(singleLine);
            } else {
                string multiLine = WriteTag(tag, indent, isComplete, true, attributes);
                writer.Write(multiLine);
            }
        }

        private static string WriteTag(string tag, int indent, bool isComplete, bool isMultiLine, params Attr[] attributes) {
            using (StringWriter writer = new StringWriter()) {
                if (isMultiLine) {                                          // Multi-Line output
                    writer.WriteLine("{0}<{1}", Indent(indent), tag);
                    foreach (Attr attr in attributes)
                        writer.WriteLine("{0}{1}={2}",
                            Indent(indent + 1),
                            attr.Name,
                            attr.Value);
                    writer.WriteLine("{0}{1}>", Indent(indent), isComplete ? "/" : "");
                } else {                                                    // Single-Line output
                    writer.Write("{0}<{1}{2}", Indent(indent), tag, attributes.Length == 0 ? "" : " ");
                    foreach (Attr attr in attributes.Where(x => x != null))
                        writer.Write("{0}={1} ",
                            attr.Name,
                            attr.Value);
                    writer.WriteLine("{0}>", isComplete ? "/" : "");
                }

                return writer.ToString();
            }
        }

        private static void CloseTag(TextWriter writer, string tag, int indent) {
            writer.WriteLine("{0}</{1}>", Indent(indent), tag);
        }

        // TODO: Most likely, this code does not belong here, but in ComponentLibrary.
        // It is only a coincidence that the SelectInput and RadioButtonGroup have
        // identical data formats for options.
        private static string BuildOptions(X10Enum anEnum) {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");
            foreach (EnumValue value in anEnum.Values) {
                builder.Append("{");
                builder.Append(string.Format("label: \"{0}\", ", value.Label));        // TODO: Translate
                builder.Append(string.Format("value: \"{0}\"", value.Name));
                builder.Append("}, ");
            }
            builder.Append("]");

            return builder.ToString();
        }

        #endregion
        #endregion

        #region Global Utils
        private static string Indent(int indent) {
            return new string(' ', indent * 2);
        }
        #endregion
    }
}