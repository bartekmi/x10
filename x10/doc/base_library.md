# Class Definition - 'Action'

Base class for all actions


### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

Data Type: String

# Class Definition - 'ActionWithDialog'

An action that will pop-up a confirming dialog when invoked.

Inherits From: Action

### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

Data Type: String
### Attribute 'danger'

If true, visual feedback will be provided to the user to indicate that the action is dangerous - e.g. deleting or cancelling something.

Data Type: Boolean
Default Value: False
### Attribute 'dialogText'

Text explaining what the user is about to do

Data Type: String
### Attribute 'dialogTitle'

The title of the dialog

Mandatory: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

Data Type: String

# Class Definition - 'AssociationDisplay'

Display a non-owned association in a read-only context

Inherits From: TextualDisplay

### Attribute 'value' (Primary)

The value to display - at present, this is assumed to be returned by the back-end in the form of the 'toStringRepresentation' property

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'AssociationEditor'

A drop-down style editor for selecting an associated entity. E.g. an editor for 'Appointment' in a clinic might have a drop-down to select the Doctor.


### Attribute 'createForm'

If present, this allows the user to add new entities. This field must point to the name of a <Form> for the Entity being selected.

Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the data is returned from the back-end.

Data Type: AssociationEditorOptionOrder
Default Value: alphabetic
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'BooleanBanner'

If the bound value is true, show the 'label' text in a box

Inherits From: TextualDisplay
Expects Data Type: Boolean

### Attribute 'icon'



Data Type: Icon
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'



Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'BooleanViaButtons'

Editor for A Boolean value using two labelled buttons.

Expects Data Type: Boolean

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'textForFalse'

The text to display on the button for the 'False' value

Data Type: String
Default Value: False
### Attribute 'textForTrue'

The text to display on the button for the 'True' value

Data Type: String
Default Value: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Bullet'

A black dot - a 'bullet' - used to break up text


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Button'

An application button with some attached action


### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'CancelButton'

A button which cancels the current user edits or in-progress action

Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'CancelDialogButton'

A button cancels the current Dialog


### Attribute 'label' (Primary)

Label of the cancel button

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Checkbox'

Editor for a Boolean value. Only two states are possible - checked and unchecked.

Expects Data Type: Boolean

### Attribute 'checkboxLabel' (Primary)

Text placed to the right of the checkbox

Primary Attribute: Yes
Data Type: String
### Attribute 'checked'

The state of the <Checkbox>

Data Type: Boolean
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'onChange'

Function to invoke when state is changed

Data Type: Boolean
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'ClassDefEditable'




### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'ClassDefVisual'




### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'DateDisplay'

Display an Date on the User Interface.

Inherits From: TextualDisplay
Expects Data Type: Date

### Attribute 'value' (Primary)

The value to display

Primary Attribute: Yes
Data Type: Date
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'DateEditor'

Editor for a date.

Expects Data Type: Date

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Dialog'

This action opens a dialog


### Attribute 'Content' (Primary)

The Content of the Dialog.

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'OpenButton'

Defines the button which opens the dialog.

### Attribute 'title'

The title of the dialog

Mandatory: Yes
Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'DisplayForm'

A data display form. Every child within the form will be provided a <Label>.


### Attribute 'Children' (Primary)

The child components. Often, these may be <FormSection>'s.

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'DropDown'

Editor for a fixed list of choices - a.k.a. 'Enumeration'

Inherits From: EnumSelection

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

Data Type: DropDownOptionOrder
Default Value: sameAsDefined
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'EnumDisplay'

Display for a choice from a fixed list of choices - a.k.a. 'Enumeration'

Inherits From: TextualDisplay

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'EnumSelection'

Editor for a fixed list of choices - a.k.a. 'Enumeration'


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

Data Type: DropDownOptionOrder
Default Value: sameAsDefined
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Expander'

A component with a header section and an 'Expander' button that toggles to show more detail


### Attribute 'Body' (Primary)

The contents of the body when expanded.

Primary Attribute: Yes
### Attribute 'Header'

The contents of the header.

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'FloatDisplay'

Display a Float on the User Interface.

Inherits From: TextualDisplay
Expects Data Type: Float

### Attribute 'value' (Primary)

The value to display

Primary Attribute: Yes
Data Type: Float
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'FloatEdit'

Editor for a Floating-point number. Proper validation is built-in.

Expects Data Type: Float

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Form'

A data entry form. Every child within the form will be provided a <Label> and validation error behavior.


### Attribute 'Children' (Primary)

The child components. Often, these may be <FormSection>'s.

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'FormErrorDisplay'

Can only exist embedded in a <Form>. Displays all errors. Typicall, located next to the Submit button.


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'FormField'

A form field - includes a label plus validation display

Inherits From: Label

### Attribute 'Content' (Primary)

Embedded UI components that are 'labelled' (i.e. that live within the label)

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

The text of the label. Normally rendered as bold text.

Data Type: String
### Attribute 'mandatoryIndicator'

If 'mandatory', an asterisk will be added to the label. If 'optional', the word 'optional' will be added after the label.

Data Type: FormFieldMandatoryIndicator
Default Value: none
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'toolTip'

If present, an icon (?) will be placed after the label - the Tool Tip message will be displayed to the user when they hover over the icon

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'FormRow'

A horizontal row layout with a wide spacing, suitable for use in forms


### Attribute 'Children' (Primary)

The contents/children of the row.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'FormSection'

If your <Form> naturally breaks into section, use this component to provide a consistent layout and heading labels.


### Attribute 'Children' (Primary)

The child components.

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

The label of this <FormSection>.

Mandatory: Yes
Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Grid'

A layout which lays out its children on a grid of arbitrary size. Children can span multiple columns and rows.


### Attribute 'Children' (Primary)

The contents/children of the panel.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'column'

Attach this attribute to children of <Grid> to specify which column to show it in. Zero-based index.

Data Type: Integer
Attached Attribute: Yes
### Attribute 'Columns'

Defines the Grid columns

Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'row'

Attach this attribute to children of <Grid> to specify which row to show it in. Zero-based index.

Data Type: Integer
Attached Attribute: Yes
### Attribute 'Rows'

Defines the Grid rows

Expects Array of Values: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'GridColumn'

Defines the width of a <Grid> column.


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'widthFraction'

The width of the column as a fraction of space left after removing fixed-pixel column widths. If this is the only 'fraction' column, it will take all remaining width.

Data Type: Float
### Attribute 'widthPixels'

The width of the column in pixels.

Data Type: Float

# Class Definition - 'GridRow'

Defines the height of a <Grid> row.


### Attribute 'heightFraction'

The height of the row as a fraction of space left after removing fixed-pixel row heights. If this is the only 'fraction' row, it will take all remaining height.

Data Type: Float
### Attribute 'heightPixels'

The height of the row in pixels.

Data Type: Float
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String

# Class Definition - 'Heading1'

A top-level heading - typically used for the main name of a page


### Attribute 'text' (Primary)

The text of the heading

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Heading2'

Same idea as <Heading1>, but smaller font - second-level heading on a page


### Attribute 'text' (Primary)

The text of the heading

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Heading3'

Same idea as <Heading2>, but even smaller font - third-level heading on a page


### Attribute 'text' (Primary)

The text of the heading

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'HelpIcon'

A question mark icon that can provide the user with contextual help information.


### Attribute 'text' (Primary)

The text of the help info.

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'HollowButton'

A buttom with no border around it.

Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'HorizontalDivider'

A horizontal divider line running the entire width of its container - typically, the entire page


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Icon'



Expects Data Type: String

### Attribute 'icon'



Data Type: Icon
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'IntDisplay'

Display an Integer on the User Interface.

Inherits From: TextualDisplay
Expects Data Type: Integer

### Attribute 'value' (Primary)

The value to display

Primary Attribute: Yes
Data Type: Integer
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'IntEdit'

Editor for an Integer. Proper validation is built-in.

Expects Data Type: Integer

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Label'

A label around a data entry or data display field/content. Normally, you get this 'for free' around model fields, but it's useful if you want to have a label around a group of fields


### Attribute 'Content' (Primary)

Embedded UI components that are 'labelled' (i.e. that live within the label)

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

The text of the label. Normally rendered as bold text.

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'toolTip'

If present, an icon (?) will be placed after the label - the Tool Tip message will be displayed to the user when they hover over the icon

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'LinkButton'

A button that looks like an HTML hyperlink.

Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'List'

A vertical list of repeated content. The content can be as simple as a string or a complicated panel.

Expects Array of Data: Yes

### Attribute 'ItemTemplate' (Primary)

In the one allowed child of <List>, define a template for the content of each item in the list.

Primary Attribute: Yes
Mandatory: Yes
### Attribute 'addItemLabel'

The label for the button to add items to the list

Data Type: String
Default Value: Add item
### Attribute 'canAdd'

Can user add items to the list in edit mode?

Data Type: Boolean
Default Value: True
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Menu'

An application menu, with possibly nested items.


### Attribute 'Children' (Primary)

The child <MenuItem>'s of the <Menu>.

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'MenuItem'

Either a top-level of a nested item in a <Menu>. Every 'leaf-level' <MenuItem> should have an attached <Action>.


### Attribute 'Children' (Primary)

If any children exist, this <MenuItem> represents a nested menu, and should not have an associated url/action.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'Action'

An <Action> to execute when user clicks the <MenuItem> (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

The label of the <MenuItem>

Mandatory: Yes
Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

The Application Url to navigate to when user click the <MenuItem> (either url or Action must be provided)..

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'PackingLayout'

A layout which does its best to arrange its children in a grid, flowing top-to-bottom, left-to-right


### Attribute 'Children' (Primary)

The contents/children of the panel.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Pill'

Like the <Text> component, but the text will be displayed within a colored oval background. Useful for displaying small but important information - e.g. number of returned results

Inherits From: Text

### Attribute 'text' (Primary)

The text to display

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'RadioButtonGroup'

A list of radio (mutually exclusive) buttons to select one choice from list of choices - a.k.a. 'Enumeration'

Inherits From: EnumSelection

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'layout'

The direction in which to order the radio buttons

Data Type: RadioButtonGroupLayout
Default Value: vertical
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

Data Type: DropDownOptionOrder
Default Value: sameAsDefined
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'RawHtml'

A placeholder within which you can put raw HTML which will be rendered in the UI. No validation will be performed by the compiler.


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'RepellingRow'

A layout panel which arranges its children horizontally, spread as far away from each other as possible. If only a single child is given, it will be centered.


### Attribute 'Children' (Primary)

The contents/children of the panel.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Row'

A layout panel which arranges its children horizontally, touching each other.


### Attribute 'Children' (Primary)

The contents/children of the panel.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'gap'

If present, specifies gap between items in pixels.

Data Type: Integer
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'SelectableButton'

A button with state - such that it can be toggled on and off

Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'onDeselect'

Custom function to invoke when user de-selects the button.

Data Type: String
### Attribute 'onSelect'

Custom function to invoke when user selects the button.

Data Type: String
### Attribute 'selected'

Determines the state of the button. Typically, this is a formula.

Data Type: Boolean
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'SpaContent'

'Single Page Application' content. This is the root-level placeholder for the entire application, except for common 'skin' like top-level menu and footer. There should only be one of these in an application.


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'rootComponent'

The name of the component to show if there is no path in the url - just the raw domain (i.e. Home Page)

Mandatory: Yes
Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'State'

Defines a single state variable on a UI Component


### Attribute 'dataType'



Data Type: String
### Attribute 'default'



Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'many'



Data Type: Boolean
### Attribute 'model'



Data Type: String
### Attribute 'variable'



Mandatory: Yes
Data Type: String

# Class Definition - 'SubmitButton'

A button which triggers validation and submits user input

Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

Primary Attribute: Yes
Mandatory: Yes
Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Tab'

Defines a tab of a <TabbedPane>.


### Attribute 'Content' (Primary)

The visual content of the tab

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

Human-readable tab label.

Data Type: String

# Class Definition - 'TabbedPane'

A component with labelled, user-selectable tabs that each show different content.


### Attribute 'Tabs' (Primary)

Definitions of the tabs

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'Table'

A table of data with rows and columns

Expects Array of Data: Yes

### Attribute 'Columns' (Primary)

Definitions of the columns

Primary Attribute: Yes
Mandatory: Yes
Expects Array of Values: Yes
### Attribute 'Header'

An optional header area connected to the table. It appears above the column headings. Typical use is for pagination controls.

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'selected'

Read/Write list of items which are currently selected

Expects Array of Values: Yes
Data Type: String
### Attribute 'selectionStyle'

The way in which table selection is shown

Data Type: TableSelectionStyle
Default Value: single
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'TableColumn'

Defines a column of a <Table>.


### Attribute 'Renderer' (Primary)

If present, this allows the <Table> cell contents to be rendered with arbitrary content. Similar to ItemTemplate in <List>.

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

<Table> column label.

Data Type: String
### Attribute 'toolTip'

If present, an icon (?) will be placed after the column label - the Tool Tip message will be displayed to the user when they hover over the icon

Data Type: String
### Attribute 'width'

Specifies the width of the table column in pixels

Data Type: Float

# Class Definition - 'TablePageControls'

The pagination controls for the <Table> - e.g. 'Page 2 of 7', along with buttons to advance or flip back the page.


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'TableSelectionColumn'

A special column which shows a checkbox for each row of the column. Allows multi-selection.

Inherits From: TableColumn

### Attribute 'Renderer' (Primary)

If present, this allows the <Table> cell contents to be rendered with arbitrary content. Similar to ItemTemplate in <List>.

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'label'

<Table> column label.

Data Type: String
### Attribute 'toolTip'

If present, an icon (?) will be placed after the column label - the Tool Tip message will be displayed to the user when they hover over the icon

Data Type: String
### Attribute 'width'

Specifies the width of the table column in pixels

Data Type: Float

# Class Definition - 'Text'

Display text on the User Interface.

Inherits From: TextualDisplay

### Attribute 'text' (Primary)

The text to display

Primary Attribute: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'TextArea'

Multi-line editor for text. Suitable for longer descriptions.

Expects Data Type: String

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'TextEdit'

One-line editor for text

Expects Data Type: String

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'TextualDisplay'

Base class for all components which display text


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'TimestampDisplay'

Display an Timestamp on the User Interface.

Inherits From: TextualDisplay
Expects Data Type: Timestamp

### Attribute 'value' (Primary)

The value to display

Primary Attribute: Yes
Data Type: Timestamp
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False
### Attribute 'weight'

You can optionally make the text bold using this attribute

Data Type: FontWeight
Attached Attribute: Yes

# Class Definition - 'TimestampEditor'

Editor for Date and Time (i.e. 'Timestamp')

Expects Data Type: Timestamp

### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'mandatory'



Data Type: Boolean
Default Value: True
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'readOnly'



Data Type: Boolean
Default Value: False
Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'UploadAction'

Dialog action to upload a file

Inherits From: ActionWithDialog

### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

Data Type: String
### Attribute 'danger'

If true, visual feedback will be provided to the user to indicate that the action is dangerous - e.g. deleting or cancelling something.

Data Type: Boolean
Default Value: False
### Attribute 'dialogText'

Text explaining what the user is about to do

Data Type: String
### Attribute 'dialogTitle'

The title of the dialog

Mandatory: Yes
Data Type: String
### Attribute 'fileFilter'

File filter - e.g. '*.csv'

Mandatory: Yes
Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

Data Type: String

# Class Definition - 'VerticalDivider'

A vertical divider line running the entire height of its container


### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'VerticalStackPanel'

A layout panel which arranges its children veritcally, stacked on top of each other.


### Attribute 'Children' (Primary)

The contents/children of the panel.

Primary Attribute: Yes
Expects Array of Values: Yes
### Attribute 'align'

Controls the horizontal alignment of the items in the Panel

Data Type: HorizontalAlignment
### Attribute 'gap'

If present, specifies gap between items in pixels.

Data Type: Integer
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

# Class Definition - 'VisibilityControl'

Automatically inserted by code generation schemes which require separate intermediate component to control visibility


### Attribute 'Content' (Primary)

The content which is made visible or invisible

Primary Attribute: Yes
### Attribute 'id'

Id for any purpose - e.g. debugging

Data Type: String
### Attribute 'maxWidth'

Maximum width that the object should have in the UI

Data Type: Integer
Default Value: False
### Attribute 'visible'

Is the object visible on the UI?

Data Type: Boolean
Default Value: False

