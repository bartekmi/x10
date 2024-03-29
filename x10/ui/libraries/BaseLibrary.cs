﻿using System;
using System.Linq;
using System.Collections.Generic;

using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;
using x10.ui.composition;

namespace x10.ui.libraries {
  public class BaseLibrary {

    public const string CLASS_DEF_FORM = "Form";
    public const string CLASS_DEF_LIST = "List";
    public const string ASSOCIATION_DISPLAY = "AssociationDisplay";

    private const string MANDATORY_INDICATOR = "mandatoryIndicator";

    private readonly static List<ClassDef> definitions = new List<ClassDef>() {

      #region No-Data Formatting Components
      new ClassDefNative() {
        Name = "Heading1",
        Description = "A top-level heading - typically used for the main name of a page",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            IsPrimary = true, 
            Name = "text",
            Description = "The text of the heading",
            DataType = DataTypes.Singleton.String,
          },
        }
      },
      new ClassDefNative() {
        Name = "Heading2",
        Description = "Same idea as <Heading1>, but smaller font - second-level heading on a page",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            IsPrimary = true,
            Name = "text",
            Description = "The text of the heading",
            DataType = DataTypes.Singleton.String,
          },
        }
      },
      new ClassDefNative() {
        Name = "Heading3",
        Description = "Same idea as <Heading2>, but even smaller font - third-level heading on a page",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            IsPrimary = true,
            Name = "text",
            Description = "The text of the heading",
            DataType = DataTypes.Singleton.String,
          },
        }
      },
      new ClassDefNative() {
        Name = "HorizontalDivider",
        Description = "A horizontal divider line running the entire width of its container - typically, the entire page",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            IsPrimary = true,
            Name = "label",
            Description = "Optional label embedded in divider",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "color",
            Description = "Optional color for the divider",
            DataType = DataTypes.Singleton.Color,
          },
        }
      },
      new ClassDefNative() {
        Name = "VerticalDivider",
        Description = "A vertical divider line running the entire height of its container",
        InheritsFrom = ClassDefNative.Visual,
      },
      new ClassDefNative() {
        Name = "Bullet",
        Description = "A black dot - a 'bullet' - used to break up text",
        InheritsFrom = ClassDefNative.Visual,
      },
      new ClassDefNative() {
        Name = "Label",
        Description = "A label around a data entry or data display field/content. Normally, you get this 'for free' around model fields, but it's useful if you want to have a label around a group of fields",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Content",
            Description = "Embedded UI components that are 'labelled' (i.e. that live within the label)",
            IsPrimary = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "The text of the label. Normally rendered as bold text.",
            DataType = DataTypes.Singleton.String,
            TakeValueFromModelAttrName = model.libraries.BaseLibrary.LABEL,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "toolTip",
            Description = "If present, an icon (?) will be placed after the label - the Tool Tip message will be displayed to the user when they hover over the icon",
            DataType = DataTypes.Singleton.String,
            TakeValueFromModelAttrName = model.libraries.BaseLibrary.TOOL_TIP,
          },
        }
      },
      #endregion

      #region Atomic Display Components
      new ClassDefNative() {
        Name = "TextualDisplay",
        Description = "Base class for all components which display text",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "weight",
            Description = "You can optionally make the text bold using this attribute",
            DataType = new DataTypeEnum() {
              Name = "FontWeight",
              EnumValueValues = new string[] { "normal", "bold" },
            },
            IsAttached = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "textColor",
            Description = "The color of the text",
            DataType = DataTypes.Singleton.Color,
            IsAttached = true,
          },
        }
      },
      new ClassDefNative() {
        Name = "Text",
        Description = "Display text on the User Interface.",
        InheritsFromName = "TextualDisplay",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "text",
            Description = "The text to display",
            DataType = DataTypes.Singleton.String,
            IsPrimary = true,
            // Not mandatory - it may come from binding. 
            // TODO: Ideally, we should make it mandatory, but also indicate that this is the binding attribute
            // and act accordingly - i.e. only raise error if Text is used in a non-binding context
          },
        }
      },
      new ClassDefNative() {
        Name = "IntDisplay",
        Description = "Display an Integer on the User Interface.",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Integer,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "value",
            Description = "The value to display",
            DataType = DataTypes.Singleton.Integer,
            IsPrimary = true,
          },
        }
      },
      new ClassDefNative() {
        Name = "FloatDisplay",
        Description = "Display a Float on the User Interface.",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Float,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "value",
            Description = "The value to display",
            DataType = DataTypes.Singleton.Float,
            IsPrimary = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "decimalPlaces",
            Description = "Optionally, specify to how many decimal place to round the value",
            DataType = DataTypes.Singleton.Integer,
            TakeValueFromModelAttrName = model.libraries.BaseLibrary.DECIMAL_PLACES,
          },
        }
      },
      new ClassDefNative() {
        Name = "DateDisplay",
        Description = "Display an Date on the User Interface.",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Date,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "value",
            Description = "The value to display",
            DataType = DataTypes.Singleton.Date,
            IsPrimary = true,
          },
        }
      },
      new ClassDefNative() {
        Name = "TimeDisplay",
        Description = "Display an Time on the User Interface.",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Time,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "value",
            Description = "The value to display",
            DataType = DataTypes.Singleton.Time,
            IsPrimary = true,
          },
        }
      },
      new ClassDefNative() {
        Name = "TimestampDisplay",
        Description = "Display an Timestamp on the User Interface.",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Timestamp,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "value",
            Description = "The value to display",
            DataType = DataTypes.Singleton.Timestamp,
            IsPrimary = true,
          },
        }
      },
      new ClassDefNative() {
        Name = "EnumDisplay",
        Description = "Display for a choice from a fixed list of choices - a.k.a. 'Enumeration'",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = new DataTypeEnum(),
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "hideLabelIfIconPresent",
            Description = "If true, hide the enum label if an icon is present",
            DataType = DataTypes.Singleton.Boolean,
          },
        }
      },
      new ClassDefNative() {
        Name = "BooleanBanner",
        Description = "If the bound value is true, show the 'label' text in a box",
        InheritsFromName = "TextualDisplay",
        AtomicDataModel = DataTypes.Singleton.Boolean,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "icon",
            DataType = x10.model.libraries.BaseLibrary.ICON_DATA_TYPE,
          },
          // TODO: Add optional color
        }
      },

      // Specialized Components
      new ClassDefNative() {
        Name = "Pill",
        Description = "Like the <Text> component, but the text will be displayed within a colored oval background. Useful for displaying small but important information - e.g. number of returned results",
        InheritsFromName = "Text",
      },
      new ClassDefNative() {
        Name = "Icon",
        InheritsFrom = ClassDefNative.Visual,
        AtomicDataModel = DataTypes.Singleton.String,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "icon",
            Description = "Type of icon",
            DataType = x10.model.libraries.BaseLibrary.ICON_DATA_TYPE,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "color",
            Description = "The color of the icon",
            DataType = DataTypes.Singleton.Color,
          },
        }
      },
      #endregion

      #region Edit Components

      #region Atomic Edit Components
      new ClassDefNative() {
        Name = "TextEdit",
        Description = "One-line editor for text",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.String,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "prefix",
            Description = "Prefix string to display before the edited text - e.g. '$'",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "suffix",
            Description = "Suffix string to display after the edited text - e.g. 'kg'",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "TextArea",
        Description = "Multi-line editor for text. Suitable for longer descriptions.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.String,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "text",
            Description = "The text to display",
            DataType = DataTypes.Singleton.String,
            IsPrimary = true,
            // See comments on "Text"
          },
        }
      },
      new ClassDefNative() {
        Name = "IntEdit",
        Description = "Editor for an Integer. Proper validation is built-in.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Integer,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "prefix",
            Description = "Prefix string to display before the edited text - e.g. '$'",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "suffix",
            Description = "Suffix string to display after the edited text - e.g. 'kg'",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "FloatEdit",
        Description = "Editor for a Floating-point number. Proper validation is built-in.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Float,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "prefix",
            Description = "Prefix string to display before the edited text - e.g. '$'",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "suffix",
            Description = "Suffix string to display after the edited text - e.g. 'kg'",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "Checkbox",
        // Note: there is a conscious decision here not to allow tri-state, as this can be ambiguous from a user's perspective
        Description = "Editor for a Boolean value. Only two states are possible - checked and unchecked.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Boolean,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "checked",
            Description = "The state of the <Checkbox>",
            DataType = DataTypes.Singleton.Boolean,
          },
          new UiAttributeDefinitionAtomic() {
            // TODO: This is very WPF-ish. I don't think having this label applies
            // to web stuff. Strongly recommend removing this attribute.
            // Instead, xml code will just have to use a plain Text to the right.
            Name = "checkboxLabel",
            Description = "Text placed to the right of the checkbox",
            DataType = DataTypes.Singleton.String,
            IsPrimary = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "onChange",
            Description = "Function to invoke when state is changed",
            DataType = DataTypes.Singleton.Boolean,
          },
        },
      },
      new ClassDefNative() {
        Name = "BooleanViaButtons",
        Description = "Editor for A Boolean value using two labelled buttons.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Boolean,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "textForTrue",
            Description = "The text to display on the button for the 'True' value",
            DataType = DataTypes.Singleton.String,
            // TODO: Remove this default but make this mandatory once we
            // can set this from yaml - see IsLcl/IsLtl in Booking.yaml
            DefaultValue = "Yes",
          },
          new UiAttributeDefinitionAtomic() {
            Name = "textForFalse",
            Description = "The text to display on the button for the 'False' value",
            DataType = DataTypes.Singleton.String,
            // TODO: Ditto
            DefaultValue = "False",
          },
        },
      },
      new ClassDefNative() {
        Name = "DateEditor",
        Description = "Editor for a date.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Date,
      },
      new ClassDefNative() {
        Name = "TimeEditor",
        Description = "Editor for a time.",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Time,
      },
      new ClassDefNative() {
        Name = "TimestampEditor",
        Description = "Editor for Date and Time (i.e. 'Timestamp')",
        InheritsFrom = ClassDefNative.Editable,
        AtomicDataModel = DataTypes.Singleton.Timestamp,
      },
      new ClassDefNative() {
        Name = "EnumSelection",
        Description = "Editor for a fixed list of choices - a.k.a. 'Enumeration'",
        InheritsFrom = ClassDefNative.Editable,
        // AtomicDataModel is not set because this component can handle any type
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "order",
            Description = "Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.",
            DataType = new DataTypeEnum() {
              Name = "DropDownOptionOrder",
              EnumValueValues = new string[] { "sameAsDefined", "alphabetic" },
            },
            DefaultValue = "sameAsDefined"
          },
          new UiAttributeDefinitionAtomic() {
            Name = "itemsSource",
            Description = "If present, this must be a formula which gives the list of options for the drop-down (or similar selector).",
            // See Issue #39
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "excludeItems",
            Description = "Optional comma-separated list of (enum) items that should be excluded",
            DataType = DataTypes.Singleton.String,
            Pass2ActionPost = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
              string value = attributeValue.Value.ToString();
              if (String.IsNullOrEmpty(value)) {
                messages.AddError(attributeValue.XmlBase, "Cannot be empty");
                return;
              }

              string[] items = value.Split(',').Select(x => x.Trim()).ToArray();
              X10Attribute attribute= ((Instance)uiComponent).ModelMember as X10Attribute;
              DataType dataType = attribute?.DataType;
              if (!(dataType is DataTypeEnum dataTypeEnum)) {
                messages.AddError(attributeValue.XmlBase, "DataType for this component must be an enum, but is: {0}", 
                  dataType);
                return;
              }

              List<string> badValues = new List<String>();
              foreach (string item in items) {
                if (!dataTypeEnum.HasEnumValue(item))
                  badValues.Add(item);
              }

              if (badValues.Count > 0) {
                messages.AddError(attributeValue.XmlBase, "The following values are not part of the {0} enum: {1}", 
                  dataTypeEnum.Name,
                  string.Join(", ", badValues));
                return;
              }
            },
          },
        }
      },
      new ClassDefNative() {
        Name = "DropDown",
        Description = "Editor for a fixed list of choices - a.k.a. 'Enumeration'",
        InheritsFromName = "EnumSelection",
        // AtomicDataModel is not set because this component can handle any type
      },
      new ClassDefNative() {
        Name = "RadioButtonGroup",
        Description = "A list of radio (mutually exclusive) buttons to select one choice from list of choices - a.k.a. 'Enumeration'",
        InheritsFromName = "EnumSelection",
        // AtomicDataModel is not set because this component can handle any type
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "layout",
            Description = "The direction in which to order the radio buttons",
            DataType = new DataTypeEnum() {
              Name = "RadioButtonGroupLayout",
              EnumValueValues = new string[] { "vertical", "horizontal" },
            },
            DefaultValue = "vertical"
          },
        }
      },
      #endregion

      #region Association Edit/Display Components
      new ClassDefNative() {
        Name = "AssociationEditor",
        Description = "A drop-down style editor for selecting an associated entity. E.g. an editor for 'Appointment' in a clinic might have a drop-down to select the Doctor.",
        InheritsFrom = ClassDefNative.Editable,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "createForm",
            Description = "If present, this allows the user to add new entities. This field must point to the name of a <Form> for the Entity being selected.",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "order",
            Description = "Determines in what order to show the options. 'sameAsDefined' means the same order in which the data is returned from the back-end.",
            DataType = new DataTypeEnum() {
              Name = "AssociationEditorOptionOrder",
              EnumValueValues = new string[] { "sameAsDefined", "alphabetic" },
            },
            DefaultValue = "alphabetic"
          },
        }
      },
      new ClassDefNative() {
        Name = ASSOCIATION_DISPLAY,
        Description = "Display a non-owned association in a read-only context",
        InheritsFromName = "TextualDisplay",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
        }
      },

      #endregion
      #endregion

      #region Layout Components

      #region Vertical
      new ClassDefNative() {
        Name = "VerticalStackPanel",
        Description = "A layout panel which arranges its children veritcally, stacked on top of each other.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Children",
            Description = "The contents/children of the panel.",
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "gap",
            Description = "If present, specifies gap between items in pixels.",
            DataType = DataTypes.Singleton.Integer,
            DefaultValue = 8,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "align",
            Description = "Controls the horizontal alignment of the items in the Panel",
            DataType = new DataTypeEnum() {
              Name = "HorizontalAlignment",
              EnumValueValues = new string[] { "left", "center", "right" },
            },
          },
        }
      },
      new ClassDefNative() {
        Name = "Expander",
        Description = "A component with a header section and an 'Expander' button that toggles to show more detail",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Header",
            Description = "The contents of the header.",
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Body",
            Description = "The contents of the body when expanded.",
            ComplexAttributeType = ClassDefNative.Visual,
          },
        }
      },
      #endregion
      
      #region Horizontal
      new ClassDefNative() {
        Name = "Row",
        Description = "A layout panel which arranges its children horizontally, touching each other.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Children",
            Description = "The contents/children of the panel.",
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "gap",
            Description = "If present, specifies gap between items in pixels.",
            DataType = DataTypes.Singleton.Integer,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "align",
            Description = "Specify vertical alignment of items in the row.",
            DataType = new DataTypeEnum() {
              Name = "VerticalAlignment",
              EnumValueValues = new string[] { "top", "center", "botom" },
            },
            IsAttached = true,
          },
          // Other properties to add as passthrough to flex-box:
          // flexWrap
        }
      },
      new ClassDefNative() {
        Name = "RepellingRow",
        Description = "A layout panel which arranges its children horizontally, spread as far away from each other as possible. If only a single child is given, it will be centered.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Children",
            Description = "The contents/children of the panel.",
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
        }
      },
      #endregion

      #region 2-Dimensional
      new ClassDefNative() {
        Name = "PackingLayout",
        Description = "A layout which does its best to arrange its children in a grid, flowing top-to-bottom, left-to-right",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Children",
            Description = "The contents/children of the panel.",
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
        }
      },

      #region Grid
      new ClassDefNative() {
        Name = "Grid",
        Description = "A layout which lays out its children on a grid of arbitrary size. Children can span multiple columns and rows.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Children",
            Description = "The contents/children of the panel.",
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionComplex() {
            Name = "Columns",
            Description = "Defines the Grid columns",
            IsMany = true,
            ComplexAttributeTypeName = "GridColumn",
          },
          new UiAttributeDefinitionComplex() {
            Name = "Rows",
            Description = "Defines the Grid rows",
            IsMany = true,
            ComplexAttributeTypeName = "GridRow",
          },
          new UiAttributeDefinitionAtomic() {
            Name = "column",
            Description = "Attach this attribute to children of <Grid> to specify which column to show it in. Zero-based index.",
            IsAttached = true,
            DataType = DataTypes.Singleton.Integer,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "row",
            Description = "Attach this attribute to children of <Grid> to specify which row to show it in. Zero-based index.",
            IsAttached = true,
            DataType = DataTypes.Singleton.Integer,
          },
        }
      },
      new ClassDefNative() {
        Name = "GridColumn",
        Description = "Defines the width of a <Grid> column.",
        InheritsFrom = ClassDefNative.Object,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "widthPixels",
            Description = "The width of the column in pixels.",
            DataType = DataTypes.Singleton.Float,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "widthFraction",
            Description = "The width of the column as a fraction of space left after removing fixed-pixel column widths. If this is the only 'fraction' column, it will take all remaining width.",
            DataType = DataTypes.Singleton.Float,
          },
        },
      },
      new ClassDefNative() {
        Name = "GridRow",
        Description = "Defines the height of a <Grid> row.",
        InheritsFrom = ClassDefNative.Object,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "heightPixels",
            Description = "The height of the row in pixels.",
            DataType = DataTypes.Singleton.Float,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "heightFraction",
            Description = "The height of the row as a fraction of space left after removing fixed-pixel row heights. If this is the only 'fraction' row, it will take all remaining height.",
            DataType = DataTypes.Singleton.Float,
          },
        },
      },
      #endregion
      #endregion

      #endregion

      #region List
      new ClassDefNative() {
        Name = CLASS_DEF_LIST,
        Description = "A vertical list of repeated content. The content can be as simple as a string or a complicated panel.",
        ComponentDataModel = Entity.Object,
        IsMany = true,
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "ItemTemplate",
            Description = "In the one allowed child of <List>, define a template for the content of each item in the list.",
            IsMandatory = true,
            ReducesManyToOne = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "addItemLabel",
            Description = "The label for the button to add items to the list",
            DataType = DataTypes.Singleton.String,
            DefaultValue = "Add item",
          },
          new UiAttributeDefinitionAtomic() {
            Name = "canAdd",
            Description = "Can user add items to the list in edit mode?",
            DataType = DataTypes.Singleton.Boolean,
            DefaultValue = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "layout",
            Description = "How items in the list are laid out",
            DataType = new DataTypeEnum("ListLayout", new string[] {"vertical", "verticalCompact", "wrap" }),
            DefaultValue = "vertical",
          },
        },
      },
      #endregion

      #region Tabbed Pane
      new ClassDefNative() {
        Name = "TabbedPane",
        Description = "A component with labelled, user-selectable tabs that each show different content.",
        ComponentDataModel = Entity.Object,
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Tabs",
            Description = "Definitions of the tabs",
            IsMany = true,
            IsMandatory = true,
            ComplexAttributeTypeName = "Tab",
          },
        },
      },
      new ClassDefNative() {
        Name = "Tab",
        Description = "Defines a tab of a <TabbedPane>.",
        InheritsFrom = ClassDefNative.Object,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Content",
            Description = "The visual content of the tab",
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "Human-readable tab label.",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      #endregion

      #region Table
      new ClassDefNative() {
        Name = "Table",
        Description = "A table of data with rows and columns",
        ComponentDataModel = Entity.Object,
        IsMany = true,
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Columns",
            Description = "Definitions of the columns",
            ModelRefWrapperComponentName = "TableColumn",
            IsMany = true,
            IsMandatory = true,
            ReducesManyToOne = true,
            ComplexAttributeTypeName = "TableColumn",
          },
          new UiAttributeDefinitionComplex() {
            Name = "Header",
            Description = "An optional header area connected to the table. It appears above the column headings. Typical use is for pagination controls.",
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionComplex() {
            Name = "ExpandedContent",
            Description = "If specified, clicking on a table row will show this extra content for the row inline, immediately under the row",
            ComplexAttributeType = ClassDefNative.Visual,
            ReducesManyToOne = true,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "selected",
            Description = "Read/Write list of items which are currently selected",
            IsMany = true,
            // TODO: Add "MustBeFormula" - DataType irrelevant
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "selectionStyle",
            Description = "The way in which table selection is shown",
            DataType = new DataTypeEnum("TableSelectionStyle", new string[] {"single", "multiple", "checkBox" }),
            DefaultValue = "single",
          },
        },
        DefaultAttachedAttributes = new List<UiAttributeValue>() {
          new UiAttributeValueAtomic(ClassDefNative.ATTR_READ_ONLY_OBJ, true),
        },
      },
      new ClassDefNative() {
        Name = "TableColumn",
        Description = "Defines a column of a <Table>.",
        InheritsFrom = ClassDefNative.Object,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            IsPrimary = true,
            Name = "Renderer",
            Description = "If present, this allows the <Table> cell contents to be rendered with arbitrary content. Similar to ItemTemplate in <List>.",
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "<Table> column label.",
            DataType = DataTypes.Singleton.String,
            TakeValueFromModelAttrName = model.libraries.BaseLibrary.LABEL,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "toolTip",
            Description = "If present, an icon (?) will be placed after the column label - the Tool Tip message will be displayed to the user when they hover over the icon",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "width",
            Description = "Specifies the width of the table column in pixels",
            DataType = DataTypes.Singleton.Float,
          },
        },
      },
      new ClassDefNative() {
        Name = "TableSelectionColumn",
        Description = "A special column which shows a checkbox for each row of the column. Allows multi-selection.",
        InheritsFromName = "TableColumn",
      },
      new ClassDefNative() {
        Name = "TablePageControls",
        Description = "The pagination controls for the <Table> - e.g. 'Page 2 of 7', along with buttons to advance or flip back the page.",
        InheritsFrom = ClassDefNative.Visual,
      },
      #endregion

      #region Button / Actions
      new ClassDefNative() {
        Name = "Action",
        Description = "Base class for all actions",
        InheritsFrom = ClassDefNative.Object,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "backEndTarget",
            Description = "Name of back-end end-point to invoke when the action is executed.",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "successUrl",
            Description = "Url to jump to if the action is successful",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "ActionWithDialog",
        Description = "An action that will pop-up a confirming dialog when invoked.",
        InheritsFromName = "Action",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "dialogTitle",
            Description = "The title of the dialog",
            IsMandatory = true,
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "dialogText",
            Description = "Text explaining what the user is about to do",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "danger",
            Description = "If true, visual feedback will be provided to the user to indicate that the action is dangerous - e.g. deleting or cancelling something.",
            DataType = DataTypes.Singleton.Boolean,
            DefaultValue = false,
          },
          // TODO: Add optional confirm and cancel text
        },
      },
      new ClassDefNative() {
        Name = "UploadAction",
        Description = "Dialog action to upload a file",
        InheritsFromName = "ActionWithDialog",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "fileFilter",
            Description = "File filter - e.g. '*.csv'",
            IsMandatory = true,
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "Button",
        Description = "An application button with some attached action",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "Label of the button",
            IsMandatory = true,
            IsPrimary = true,
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "style",
            Description = "The visual style of the button",
            DataType = new DataTypeEnum("ButtonStyle", new string[] {"normal", "link" }),
          },
          new UiAttributeDefinitionAtomic() {
            Name = "url",
            Description = "Application Url to jump to (either url or Action must be provided).",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionComplex() {
            Name = "Action",
            Description = "An action to execute (either url or Action must be provided).",
            ComplexAttributeTypeName = "Action",
          },
        },
        // TODO: Add cross-validation... Either Action or url must be provided
      },
      new ClassDefNative() {
        Name = "CancelButton",
        Description = "A button which cancels the current user edits or in-progress action",
        InheritsFromName = "Button",
      },
      new ClassDefNative() {
        Name = "SelectableButton",
        Description = "A button with state - such that it can be toggled on and off",
        InheritsFromName = "Button",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "selected",
            Description = "Determines the state of the button. Typically, this is a formula.",
            DataType = DataTypes.Singleton.Boolean,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "onSelect",
            Description = "Custom function to invoke when user selects the button.",
            // TODO: Consider introducing a new data-type: EventHandler
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "onDeselect",
            Description = "Custom function to invoke when user de-selects the button.",
            // TODO: Consider introducing a new data-type: EventHandler
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "HollowButton",
        Description = "A buttom with no border around it.",
        InheritsFromName = "Button",
      },
      new ClassDefNative() {
        Name = "LinkButton",
        Description = "A button that looks like an HTML hyperlink.",
        InheritsFromName = "Button",
      },
      #endregion

      #region Menu
      new ClassDefNative() {
        Name = "Menu",
        Description = "An application menu, with possibly nested items.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Children",
            Description = "The child <MenuItem>'s of the <Menu>.",
            IsPrimary = true,
            IsMandatory = true,
            IsMany = true,
            ComplexAttributeTypeName = "MenuItem",
          },
        },
      },
      new ClassDefNative() {
        Name = "MenuItem",
        Description = "Either a top-level of a nested item in a <Menu>. Every 'leaf-level' <MenuItem> should have an attached <Action>.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "The label of the <MenuItem>",
            IsMandatory = true,
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "url",
            Description = "The Application Url to navigate to when user click the <MenuItem> (either url or Action must be provided)..",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionComplex() {
            Name = "Action",
            Description = "An <Action> to execute when user clicks the <MenuItem> (either url or Action must be provided).",
            ComplexAttributeTypeName = "Action",
          },
          new UiAttributeDefinitionComplex() {
            Name = "Children",
            Description = "If any children exist, this <MenuItem> represents a nested menu, and should not have an associated url/action.",
            IsPrimary = true,
            IsMany = true,
            ComplexAttributeTypeName = "MenuItem",
          },
        },
      },
      #endregion

      #region Display Form
      new ClassDefNative() {
        Name = "DisplayForm",
        Description = "A data display form. Every child within the form will be provided a <Label>.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Children",
            Description = "The child components. Often, these may be <FormSection>'s.",
            IsPrimary = true,
            IsMandatory = true,
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
            ModelRefWrapperComponentName = "Label",
          },
          new UiAttributeDefinitionAtomic() {
            Name = "gap",
            Description = "Change default gap between items in built-in VerticalStackPanel",
            DataType = DataTypes.Singleton.Integer,
          },
        },
        DefaultAttachedAttributes = new List<UiAttributeValue>() {
          new UiAttributeValueAtomic(ClassDefNative.ATTR_READ_ONLY_OBJ, true),
        },
      },
      #endregion

      #region (Edit) Form
      new ClassDefNative() {
        Name = CLASS_DEF_FORM,
        Description = "A data entry form. Every child within the form will be provided a <Label> and validation error behavior.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Children",
            Description = "The child components. Often, these may be <FormSection>'s.",
            IsPrimary = true,
            IsMandatory = true,
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
            ModelRefWrapperComponentName = "FormField",
          },
        },
        DefaultAttachedAttributes = new List<UiAttributeValue>() {
          new UiAttributeValueAtomic(ClassDefNative.ATTR_READ_ONLY_OBJ, false),
        },
      },
      new ClassDefNative() {
        Name = "FormErrorDisplay",
        Description = "Can only exist embedded in a <Form>. Displays all errors. Typicall, located next to the Submit button.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "paths",
            Description = "Optional comma-separated list of paths of form element - only these errors will be shown",
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "FormSection",
        Description = "If your <Form> naturally breaks into section, use this component to provide a consistent layout and heading labels.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionComplex() {
            Name = "Children",
            Description = "The child components.",
            IsPrimary = true,
            IsMandatory = true,
            IsMany = true,
            ComplexAttributeType = ClassDefNative.Visual,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "The label of this <FormSection>.",
            DataType = DataTypes.Singleton.String,
            IsMandatory = true,
          },
        },
      },
      new ClassDefNative() {
        Name = "FormField",
        Description = "A form field - includes a label plus validation display",
        InheritsFromName = "Label",
        Pass2ActionPost = (messages, allEntities, allEnums, allUiDefinitions, instance) => {
          Member member = instance.Unwrap().ModelMember;
          if (member == null)
            return;
          bool isMandatory = member.IsMandatory;

          object mandatoryIndicator = instance.FindValue(MANDATORY_INDICATOR);
          if (mandatoryIndicator == null && isMandatory) {
            UiAttributeDefinitionAtomic attribute = instance.ClassDef.FindAtomicAttribute(MANDATORY_INDICATOR);
            UiAttributeValueAtomic attrValue = attribute.CreateValueAndAddToOwnerAtomic(instance, null);
            attrValue.Value = "mandatory";
          }
        },
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = MANDATORY_INDICATOR,
            Description = "If 'mandatory', an asterisk will be added to the label. If 'optional', the word 'optional' will be added after the label.",
            DataType = new DataTypeEnum("FormFieldMandatoryIndicator", new string[] {"none", "mandatory", "optional" }),
          },
        }
      },
      new ClassDefNative() {
        Name = "SubmitButton",
        Description = "A button which triggers validation and submits user input",
        InheritsFromName = "Button",
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "successMessage",
            Description = "Message to display upon success",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "errorMessage",
            Description = "Message to display upon error",
            DataType = DataTypes.Singleton.String,
          },
        }
      },
      #endregion

      #region Misc
      new ClassDefNative() {
        Name = "Dialog",
        Description = "This action opens a dialog",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "title",
            Description = "The title of the dialog",
            IsMandatory = true,
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionComplex() {
            Name = "OpenButton",
            Description = "Defines the button which opens the dialog.",
            ComplexAttributeTypeName = "Button",
          },
          new UiAttributeDefinitionComplex() {
            Name = "Content",
            IsPrimary = true,
            Description = "The Content of the Dialog.",
            ComplexAttributeType = ClassDefNative.Visual,
          },
        },
      },
      new ClassDefNative() {
        Name = "CancelDialogButton",
        Description = "A button cancels the current Dialog",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            Description = "Label of the cancel button",
            IsPrimary = true,
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "HelpIcon",
        Description = "A question mark icon that can provide the user with contextual help information.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "text",
            Description = "The text of the help info.",
            DataType = DataTypes.Singleton.String,
            IsMandatory = true,
            IsPrimary = true,
          },
        },
      },
      new ClassDefNative() {
        Name = "Embed",
        Description = "Embed the contents of a url into the application",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "url",
            Description = "The url of the website to embed",
            DataType = DataTypes.Singleton.String,
            IsMandatory = true,
            IsPrimary = true,
          },
        },
      },
      new ClassDefNative() {
        Name = "SpaContent",
        Description = "'Single Page Application' content. This is the root-level placeholder for the entire application, except for common 'skin' like top-level menu and footer. There should only be one of these in an application.",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "rootComponent",
            Description = "The name of the component to show if there is no path in the url - just the raw domain (i.e. Home Page)",
            IsMandatory = true,
            DataType = DataTypes.Singleton.String,
            Pass2ActionPost = (messages, allEntities, allEnums, allUiDefinitions, uiComponent, attributeValue) => {
              allUiDefinitions.FindDefinitionByNameWithError(attributeValue.Value.ToString(), attributeValue.XmlBase);
            },
          },
        },
        // TODO... At some point, we should verify that there is only a single one of these in an App
      },
      #endregion
    };

    #region Glue it Together
    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static UiLibrary CreateLibrary() {
      definitions.AddRange(ClassDefNative.PRIMORDIAL_COMPONENTS);
      UiLibrary library = new UiLibrary(definitions) {
        Name = "Base Library",
      };

      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "IntEdit", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Float, "FloatEdit", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Date, "DateEditor", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Time, "TimeEditor", UseMode.ReadWrite);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Timestamp, "TimestampEditor", UseMode.ReadWrite);
      
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "Text", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "IntDisplay", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Float, "FloatDisplay", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Date, "DateDisplay", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Time, "TimeDisplay", UseMode.ReadOnly);
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Timestamp, "TimestampDisplay", UseMode.ReadOnly);
      
      library.AddDataTypeToComponentAssociation(x10.model.libraries.BaseLibrary.ICON_DATA_TYPE, "Icon", UseMode.ReadOnly);

      library.SetComponentForEnums("DropDown", UseMode.ReadWrite);
      library.SetComponentForEnums("EnumDisplay", UseMode.ReadOnly);

      library.SetComponentForAssociations("AssociationEditor", UseMode.ReadWrite);
      library.SetComponentForAssociations(ASSOCIATION_DISPLAY, UseMode.ReadOnly);

      return library;
    }
    #endregion
  }
}
