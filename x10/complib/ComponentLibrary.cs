using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using x10.schema;
using x10.complib;

namespace x10.complib {
    public class ComponentLibrary {
        private static ComponentLibrary _library;
        public static ComponentLibrary Singleton {
            get {
                if (_library == null) {
                    _library = new ComponentLibrary();
                    _library.InitializeComponents();
                    _library.InitializeDefaults();
                    _library.PostProcess();
                }
                return _library;
            }
        }

        private Dictionary<string, Registration> _registrations = new Dictionary<string, Registration>();
        private Dictionary<DataType, ComponentDef> _defaults = new Dictionary<DataType, ComponentDef>();

        internal void Register(ComponentDef component,
            string[] dataTypesWrite = null,
            string[] dataTypesRead = null) {
            if (_registrations.ContainsKey(component.Name))
                throw new Exception("Multiple definitons for component: " + component.Name);

            _registrations[component.Name] = (new Registration() {
                Component = component,
                DataTypesWrite = dataTypesWrite,
                DataTypesRead = dataTypesWrite,
            });
        }

        internal void RegisterDefault(string dataTypeName, string componentName) {
            DataType dataType = DataType.FindDataType(dataTypeName);
            if (dataType == null)
                throw new Exception("DataType does not exist: " + dataTypeName);

            if (_defaults.ContainsKey(dataType))
                throw new Exception("Multiple defaults for data type: " + dataTypeName);

            if (!_registrations.TryGetValue(componentName, out Registration registration))
                throw new Exception("Unknown component for default: " + componentName);

            _defaults[dataType] = registration.Component;
        }

        public ComponentDef FindComponent(Property property, string componentName, out string error) {
            error = null;

            if (componentName == null) {
                if (property.UiOverrideAsString == null)
                    return _defaults[property.Type];

                ComponentDef component = FindComponent(property.UiOverrideAsString, out error);
                if (error != null)
                    throw new Exception(string.Format("UiOverride {0} for Property {1} does not exist",
                        property.UiOverrideAsString,
                        property.Name));
                return component;
            }

            Registration registration = FindRegistration(componentName, out error);
            if (registration == null)
                return null;

            // TODO... Ignoring read-only for now

            string dataType = property.Type.Name;
            if (registration.DataTypesWrite.Contains(dataType)) {
                error = string.Format("Component {0} exists, but is not appropriate for data type {1} of property {2}",
                    componentName, dataType, property);
                return null;
            }

            return registration.Component;
        }

        public ComponentDef FindComponent(string componentName, out string error) {
            Registration registration = FindRegistration(componentName, out error);
            return registration == null ? null : registration.Component;
        }

        private Registration FindRegistration(string componentName, out string error) {
            if (!_registrations.TryGetValue(componentName, out Registration registration)) {
                error = string.Format("Component '{0}' unknown", componentName);
                return null;
            }

            error = null;
            return registration;
        }


        class Registration {
            internal ComponentDef Component;
            internal string[] DataTypesWrite;
            internal string[] DataTypesRead;
        }

        private void InitializeComponents() {
            //-----------------------------------------------------------------
            // Latitude Components
            // For Latitude, I assume there is a way to extract this info, since the 
            // go/components page seems to do it

            // Data Input Components
            Register(new ComponentDefBasic() {
                Name = "Text",
                ImportFile = "latitude/Text",
                NullTreatment = NullTreatment.NotApplicable,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "color", Type = ParamType.Color },
                    new ParamDef() {
                        Name = "scale",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"display", "headline", "title"},
                    },
                    new ParamDef() {
                        Name = "weight",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"bold", "regular"},
                    },
                    new ParamDef() {
                        Name = "emptyRetainsLineHeight",
                        Type = ParamType.Bool,
                    },
                },
            },
            new string[] { },
            new string[] { "boolean", "int", "float", "string", "text", "enum", "datatime" });

            Register(new ComponentDefBasic() {
                Name = "TextInput",
                ImportFile = "latitude/TextInput",
                NullTreatment = NullTreatment.ConvertNullToDefault,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "value", Type = ParamType.String },
                    new ParamDef() {
                        Name = "placeholder",
                        Type = ParamType.String,
                        NeedsTranslation = true,
                    },
                    new ParamDef() { Name = "disabled", Type = ParamType.Bool },
                    new ParamDef() {
                        Name = "textAlign",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"left", "right", "center"}
                    },
                    new ParamDef() {
                        Name = "suffix",
                        Type = ParamType.String,
                    },
                },
            },
            new string[] { "string" });

            Register(new ComponentDefBasic() {
                Name = "FloatInput",
                ImportFile = "latitude/FloatInput",
                NullTreatment = NullTreatment.AcceptsAndReturnsNull,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "value", Type = ParamType.String },
                    new ParamDef() { Name = "placeholder", Type = ParamType.String },
                    new ParamDef() { Name = "disabled", Type = ParamType.Bool },
                    new ParamDef() {
                        Name = "textAlign",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"left", "right", "center"}
                    },
                    new ParamDef() { Name = "suffix", Type = ParamType.String },
                    new ParamDef() { Name = "decimalPrecision", Type = ParamType.Int },
                },
            },
            new string[] { "int", "float" });

            Register(new ComponentDefBasic() {
                Name = "TextareaInput",
                ImportFile = "latitude/TextareaInput",
                NullTreatment = NullTreatment.ConvertNullToDefault,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "value", Type = ParamType.String },
                },
            },
            new string[] { "text" });

            Register(new ComponentDefBasic() {
                Name = "SelectInput",
                ImportFile = "latitude/select/SelectInput",
                NullTreatment = NullTreatment.AcceptsAndReturnsNull,
                UserCantClear = true,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "value", Type = ParamType.String },
                    new ParamDef() { Name = "isNullable", Type = ParamType.Bool },
                },
                ExtraParamsFunc = (property) => {
                    return new ParamValue[] {
                        new ParamValue() {
                            Name = "isNullable",
                            Value = (!property.IsMandatory).ToString().ToLower(),
                        },
                        new ParamValue() {
                            Name = "options",
                            Value = property.Enum,
                        },
                    };
                },
            },
            new string[] { "enum" });

            Register(new ComponentDefBasic() {
                Name = "Checkbox",
                ImportFile = "latitude/Checkbox",
                NullTreatment = NullTreatment.NeverAllowsNull,
                UserCantClear = true,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "value", Type = ParamType.Bool },
                    new ParamDef() { Name = "isNullable", Type = ParamType.Bool },
                },
            },
            new string[] { "enum" });

            // Container Components
            Register(new ComponentDefNarrowing() {
                Name = "Row",
                ParentComponentName = "Group",
                NarrowedParams = new ParamValue[] {
                    new ParamValue() {Name = "flexDirection", Value = "row"},
                    new ParamValue() {Name = "gap", Value = 24},
                    new ParamValue() {Name = "alignItems", Value = "flex-start"},
                },
            });

            Register(new ComponentDefNarrowing() {
                Name = "Column",
                ParentComponentName = "Group",
                NarrowedParams = new ParamValue[] {
                    new ParamValue() {Name = "flexDirection", Value = "column"},
                    new ParamValue() {Name = "gap", Value = 24},
                },
            });

            Register(new ComponentDefBasic() {
                Name = "Group",
                ImportFile = "latitude/Group",
                UserCantClear = true,
                Params = new ParamDef[] {
                    new ParamDef() { Name = "gap", Type = ParamType.Int },
                    new ParamDef() {
                        Name = "alignItems",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"flex-start" },
                    },
                    new ParamDef() {
                        Name = "flexDirection",
                        Type = ParamType.Enum,
                        EnumValues = new string[] {"row", "row-reverse", "column", "column-reverse" },
                    }
                },
            });


            // --------------------------------------------------------------------
            // Candidate Components
            Register(new ComponentDefBasic() {
                Name = "RadioButtonGroup",
                ImportFile = "components/base_candidate/radio/RadioButtonGroup",
                NullTreatment = NullTreatment.AcceptsAndReturnsNull,
                DefaultLabelPlacement = LabelPlacement.None,
                UserCantClear = true,
                Params = new ParamDef[] {
                },
                ExtraParamsFunc = (property) => {
                    return new ParamValue[] {
                        new ParamValue() {
                            Name = "options",
                            Value = property.Enum,
                        },
                    };
                },
            },
            new string[] { "boolean", "enum" });

            // --------------------------------------------------------------------
            // Higher Level Components
            Register(new ComponentDefBasic() {
                Name = "MultiStacker",
                ImportFile = "components/hackathon/MultiStacker",
                Params = new ParamDef[] {
                    new ParamDef() { Name = "items", Type = ParamType.VarInputData },
                    new ParamDef() { Name = "itemDisplayFunc", Type = ParamType.VarChildAsCreateFunc },
                    new ParamDef() { Name = "onItemsChanged", Type = ParamType.VarCallback },
                    new ParamDef() { Name = "onAddItem", Type = ParamType.VarOnAddItem },
                    new ParamDef() { Name = "addItemLabel", Type = ParamType.String },
                    new ParamDef() { Name = "showErrors", Type = ParamType.VarShowErrors },
                    new ParamDef() { Name = "deleteIcon", Type = ParamType.String },
                },
            });

            Register(new ComponentDefBasic() {
                Name = "Switcher",
                ImportFile = "components/hackathon/Switcher",
                Params = new ParamDef[] {
                    new ParamDef() { Name = "switchToSecondChildLabel", Type = ParamType.String },
                    new ParamDef() { Name = "switchToFirstChildLabel", Type = ParamType.String },
                    new ParamDef() {
                        Name = "showFirstChild",
                        Type = ParamType.VarSubstitute,
                        Template = "data.PROPTEXT || false",
                    },
                    new ParamDef() {
                        Name = "onSwitchChanged",
                        Type = ParamType.VarSubstitute,
                        Template = @"showFirstChild => {
          onDataChanged({
            ...data,
            PROPTEXT: showFirstChild,
          });
        }",
                    },
                },
            });
        }

        private void InitializeDefaults() {
            RegisterDefault("boolean", "Checkbox");
            RegisterDefault("int", "FloatInput");
            RegisterDefault("float", "FloatInput");
            RegisterDefault("string", "TextInput");
            RegisterDefault("text", "TextareaInput");
            RegisterDefault("enum", "SelectInput");
        }

        #region Post-Process
        private void PostProcess() {
            PostProcessNarrowingComponents();
            PostProcessNarrowingComponentParameters();
            PostProcessSchemaUiOverrides();
        }

        private void PostProcessNarrowingComponents() {
            foreach (ComponentDefNarrowing component in FindNarrowingComponents()) {
                if (_registrations.TryGetValue(component.ParentComponentName, out Registration registration)) {
                    component.PhysicalComponent = (ComponentDefBasic)registration.Component;
                } else
                    throw new Exception(string.Format("Component {0} not found for Narrowing Component {1}",
                        component.ParentComponentName,
                        component.Name));
            }
        }

        private void PostProcessNarrowingComponentParameters() {
            foreach (ComponentDefNarrowing component in FindNarrowingComponents()) {
                foreach (ParamValue paramNarrowing in component.NarrowedParams) {
                    ParamDef param = component.PhysicalComponent.Params.SingleOrDefault(x => x.Name == paramNarrowing.Name);
                    if (param == null) {
                        throw new Exception(string.Format("Narrowing parameter {0} of component {1} does not exist in parent {2}",
                            paramNarrowing.Name,
                            component.Name,
                            component.ParentComponentName));
                    }
                    paramNarrowing.Param = param;
                }
            }
        }

        private IEnumerable<ComponentDefNarrowing> FindNarrowingComponents() {
            return _registrations.Values
                .Select(x => x.Component)
                .OfType<ComponentDefNarrowing>();
        }

        private void PostProcessSchemaUiOverrides() {
            IEnumerable<Property> propsWithOverrides = Schema.Singleton.Entities
                .SelectMany(x => x.Properties)
                .Where(x => x.UiOverrideAsString != null);

            foreach (Property property in propsWithOverrides)
                if (!_registrations.ContainsKey(property.UiOverrideAsString))
                    throw new Exception(string.Format("Unknown UI Override {0} in Property {1}",
                        property.UiOverrideAsString,
                        property.Name));
        }
        #endregion
    }
}