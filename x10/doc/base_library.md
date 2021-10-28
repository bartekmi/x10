# Class Definition - 'Action'

Base class for all actions

- Inherits From: ClassDefObject

### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

- Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

- Data Type: String

# Class Definition - 'ActionWithDialog'

An action that will pop-up a confirming dialog when invoked.

- Inherits From: Action

### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

- Data Type: String
### Attribute 'danger'

If true, visual feedback will be provided to the user to indicate that the action is dangerous - e.g. deleting or cancelling something.

- Data Type: Boolean
- Default Value: False
### Attribute 'dialogText'

Text explaining what the user is about to do

- Data Type: String
### Attribute 'dialogTitle'

The title of the dialog

- Mandatory: Yes
- Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

- Data Type: String

# Class Definition - 'AssociationDisplay'

Display a non-owned association in a read-only context

- Inherits From: TextualDisplay

### Attribute 'value' (Primary)

The value to display - at present, this is assumed to be returned by the back-end in the form of the 'toStringRepresentation' property

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'AssociationEditor'

A drop-down style editor for selecting an associated entity. E.g. an editor for 'Appointment' in a clinic might have a drop-down to select the Doctor.

- Inherits From: ClassDefEditable

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'createForm'

If present, this allows the user to add new entities. This field must point to the name of a <Form> for the Entity being selected.

- Data Type: String
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the data is returned from the back-end.

- Data Type: AssociationEditorOptionOrder
- Default Value: alphabetic
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'BooleanBanner'

If the bound value is true, show the 'label' text in a box

- Inherits From: TextualDisplay
- Expects Data Type: Boolean

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'icon'



- Data Type: Icon
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'



- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'BooleanViaButtons'

Editor for A Boolean value using two labelled buttons.

- Inherits From: ClassDefEditable
- Expects Data Type: Boolean

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'textForFalse'

The text to display on the button for the 'False' value

- Data Type: String
- Default Value: False
### Attribute 'textForTrue'

The text to display on the button for the 'True' value

- Data Type: String
- Default Value: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Bullet'

A black dot - a 'bullet' - used to break up text

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Button'

An application button with some attached action

- Inherits From: ClassDefVisual

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'CancelButton'

A button which cancels the current user edits or in-progress action

- Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'CancelDialogButton'

A button cancels the current Dialog

- Inherits From: ClassDefVisual

### Attribute 'label' (Primary)

Label of the cancel button

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Checkbox'

Editor for a Boolean value. Only two states are possible - checked and unchecked.

- Inherits From: ClassDefEditable
- Expects Data Type: Boolean

### Attribute 'checkboxLabel' (Primary)

Text placed to the right of the checkbox

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'checked'

The state of the <Checkbox>

- Data Type: Boolean
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'onChange'

Function to invoke when state is changed

- Data Type: Boolean
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'ClassDefEditable'



- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'ClassDefVisual'



- Inherits From: ClassDefObject

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'DateDisplay'

Display an Date on the User Interface.

- Inherits From: TextualDisplay
- Expects Data Type: Date

### Attribute 'value' (Primary)

The value to display

- Data Type: Date
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'DateEditor'

Editor for a date.

- Inherits From: ClassDefEditable
- Expects Data Type: Date

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Dialog'

This action opens a dialog

- Inherits From: ClassDefVisual

### Attribute 'Content' (Primary)

The Content of the Dialog.

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'OpenButton'

Defines the button which opens the dialog.

### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'title'

The title of the dialog

- Mandatory: Yes
- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'DisplayForm'

A data display form. Every child within the form will be provided a *Label*.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The child components. Often, these may be <FormSection>'s.

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'DropDown'

Editor for a fixed list of choices - a.k.a. 'Enumeration'

- Inherits From: EnumSelection
- Expects Data Type: EnumType

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'excludeItems'

Optional comma-separated list of (enum) items that should be excluded

- Data Type: String
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

- Data Type: DropDownOptionOrder
- Default Value: sameAsDefined
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Embed'

Embed the contents of a url into the application

- Inherits From: ClassDefVisual

### Attribute 'url' (Primary)

The url of the website to embed

- Mandatory: Yes
- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'EnumDisplay'

Display for a choice from a fixed list of choices - a.k.a. 'Enumeration'

- Inherits From: TextualDisplay

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'hideLabelIfIconPresent'

If true, hide the enum label if an icon is present

- Data Type: Boolean
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'EnumSelection'

Editor for a fixed list of choices - a.k.a. 'Enumeration'

- Inherits From: ClassDefEditable
- Expects Data Type: EnumType

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'excludeItems'

Optional comma-separated list of (enum) items that should be excluded

- Data Type: String
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

- Data Type: DropDownOptionOrder
- Default Value: sameAsDefined
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Expander'

A component with a header section and an 'Expander' button that toggles to show more detail

- Inherits From: ClassDefVisual

### Attribute 'Body' (Primary)

The contents of the body when expanded.

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'Header'

The contents of the header.

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FloatDisplay'

Display a Float on the User Interface.

- Inherits From: TextualDisplay
- Expects Data Type: Float

### Attribute 'value' (Primary)

The value to display

- Data Type: Float
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'decimalPlaces'

Optionally, specify to how many decimal place to round the value

- Data Type: Integer
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FloatEdit'

Editor for a Floating-point number. Proper validation is built-in.

- Inherits From: ClassDefEditable
- Expects Data Type: Float

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'prefix'

Prefix string to display before the edited text - e.g. '$'

- Data Type: String
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'suffix'

Suffix string to display after the edited text - e.g. 'kg'

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Form'

A data entry form. Every child within the form will be provided a *Label* and validation error behavior.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The child components. Often, these may be <FormSection>'s.

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FormErrorDisplay'

Can only exist embedded in a *Form*. Displays all errors. Typicall, located next to the Submit button.

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FormField'

A form field - includes a label plus validation display

- Inherits From: Label

### Attribute 'Content' (Primary)

Embedded UI components that are 'labelled' (i.e. that live within the label)

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

The text of the label. Normally rendered as bold text.

- Data Type: String
### Attribute 'mandatoryIndicator'

If 'mandatory', an asterisk will be added to the label. If 'optional', the word 'optional' will be added after the label.

- Data Type: FormFieldMandatoryIndicator
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'toolTip'

If present, an icon (?) will be placed after the label - the Tool Tip message will be displayed to the user when they hover over the icon

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FormRow'

A horizontal row layout with a wide spacing, suitable for use in forms

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the row.

- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'FormSection'

If your *Form* naturally breaks into section, use this component to provide a consistent layout and heading labels.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The child components.

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

The label of this <FormSection>.

- Mandatory: Yes
- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Grid'

A layout which lays out its children on a grid of arbitrary size. Children can span multiple columns and rows.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the panel.

- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'column'

Attach this attribute to children of <Grid> to specify which column to show it in. Zero-based index.

- Data Type: Integer
- Attached Attribute: Yes
### Attribute 'Columns'

Defines the Grid columns

- Expects Array of Values: Yes
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'row'

Attach this attribute to children of <Grid> to specify which row to show it in. Zero-based index.

- Data Type: Integer
- Attached Attribute: Yes
### Attribute 'Rows'

Defines the Grid rows

- Expects Array of Values: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'GridColumn'

Defines the width of a *Grid* column.

- Inherits From: ClassDefObject

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'widthFraction'

The width of the column as a fraction of space left after removing fixed-pixel column widths. If this is the only 'fraction' column, it will take all remaining width.

- Data Type: Float
### Attribute 'widthPixels'

The width of the column in pixels.

- Data Type: Float

# Class Definition - 'GridRow'

Defines the height of a *Grid* row.

- Inherits From: ClassDefObject

### Attribute 'heightFraction'

The height of the row as a fraction of space left after removing fixed-pixel row heights. If this is the only 'fraction' row, it will take all remaining height.

- Data Type: Float
### Attribute 'heightPixels'

The height of the row in pixels.

- Data Type: Float
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String

# Class Definition - 'Heading1'

A top-level heading - typically used for the main name of a page

- Inherits From: ClassDefVisual

### Attribute 'text' (Primary)

The text of the heading

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Heading2'

Same idea as *Heading1*, but smaller font - second-level heading on a page

- Inherits From: ClassDefVisual

### Attribute 'text' (Primary)

The text of the heading

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Heading3'

Same idea as *Heading2*, but even smaller font - third-level heading on a page

- Inherits From: ClassDefVisual

### Attribute 'text' (Primary)

The text of the heading

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'HelpIcon'

A question mark icon that can provide the user with contextual help information.

- Inherits From: ClassDefVisual

### Attribute 'text' (Primary)

The text of the help info.

- Mandatory: Yes
- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'HollowButton'

A buttom with no border around it.

- Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'HorizontalDivider'

A horizontal divider line running the entire width of its container - typically, the entire page

- Inherits From: ClassDefVisual

### Attribute 'label' (Primary)

Optional label embedded in divider

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Icon'



- Inherits From: ClassDefVisual
- Expects Data Type: String

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'color'

The color of the icon

- Data Type: Color
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'icon'

Type of icon

- Data Type: Icon
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'IntDisplay'

Display an Integer on the User Interface.

- Inherits From: TextualDisplay
- Expects Data Type: Integer

### Attribute 'value' (Primary)

The value to display

- Data Type: Integer
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'IntEdit'

Editor for an Integer. Proper validation is built-in.

- Inherits From: ClassDefEditable
- Expects Data Type: Integer

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'prefix'

Prefix string to display before the edited text - e.g. '$'

- Data Type: String
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'suffix'

Suffix string to display after the edited text - e.g. 'kg'

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Label'

A label around a data entry or data display field/content. Normally, you get this 'for free' around model fields, but it's useful if you want to have a label around a group of fields

- Inherits From: ClassDefVisual

### Attribute 'Content' (Primary)

Embedded UI components that are 'labelled' (i.e. that live within the label)

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

The text of the label. Normally rendered as bold text.

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'toolTip'

If present, an icon (?) will be placed after the label - the Tool Tip message will be displayed to the user when they hover over the icon

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'LinkButton'

A button that looks like an HTML hyperlink.

- Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'List'

A vertical list of repeated content. The content can be as simple as a string or a complicated panel.

- Inherits From: ClassDefVisual
- Expects Array of Data: Yes

### Attribute 'ItemTemplate' (Primary)

In the one allowed child of <List>, define a template for the content of each item in the list.

- Mandatory: Yes
### Attribute 'addItemLabel'

The label for the button to add items to the list

- Data Type: String
- Default Value: Add item
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'canAdd'

Can user add items to the list in edit mode?

- Data Type: Boolean
- Default Value: True
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'layout'

How items in the list are laid out

- Data Type: ListLayout
- Default Value: vertical
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Menu'

An application menu, with possibly nested items.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The child <MenuItem>'s of the <Menu>.

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'MenuItem'

Either a top-level of a nested item in a *Menu*. Every 'leaf-level' *MenuItem* should have an attached *Action*.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

If any children exist, this <MenuItem> represents a nested menu, and should not have an associated url/action.

- Expects Array of Values: Yes
### Attribute 'Action'

An <Action> to execute when user clicks the <MenuItem> (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

The label of the <MenuItem>

- Mandatory: Yes
- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'url'

The Application Url to navigate to when user click the <MenuItem> (either url or Action must be provided)..

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'PackingLayout'

A layout which does its best to arrange its children in a grid, flowing top-to-bottom, left-to-right

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the panel.

- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Pill'

Like the *Text* component, but the text will be displayed within a colored oval background. Useful for displaying small but important information - e.g. number of returned results

- Inherits From: Text

### Attribute 'text' (Primary)

The text to display

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'RadioButtonGroup'

A list of radio (mutually exclusive) buttons to select one choice from list of choices - a.k.a. 'Enumeration'

- Inherits From: EnumSelection
- Expects Data Type: EnumType

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'excludeItems'

Optional comma-separated list of (enum) items that should be excluded

- Data Type: String
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'layout'

The direction in which to order the radio buttons

- Data Type: RadioButtonGroupLayout
- Default Value: vertical
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'order'

Determines in what order to show the options. 'sameAsDefined' means the same order in which the Enumerated type was defined in yaml.

- Data Type: DropDownOptionOrder
- Default Value: sameAsDefined
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'RawHtml'

A placeholder within which you can put raw HTML which will be rendered in the UI. No validation will be performed by the compiler.

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'RepellingRow'

A layout panel which arranges its children horizontally, spread as far away from each other as possible. If only a single child is given, it will be centered.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the panel.

- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Row'

A layout panel which arranges its children horizontally, touching each other.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the panel.

- Expects Array of Values: Yes
### Attribute 'align'

Specify vertical alignment of items in the row.

- Data Type: VerticalAlignment
- Attached Attribute: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'gap'

If present, specifies gap between items in pixels.

- Data Type: Integer
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'SelectableButton'

A button with state - such that it can be toggled on and off

- Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'onDeselect'

Custom function to invoke when user de-selects the button.

- Data Type: String
### Attribute 'onSelect'

Custom function to invoke when user selects the button.

- Data Type: String
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'selected'

Determines the state of the button. Typically, this is a formula.

- Data Type: Boolean
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'SpaContent'

'Single Page Application' content. This is the root-level placeholder for the entire application, except for common 'skin' like top-level menu and footer. There should only be one of these in an application.

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'rootComponent'

The name of the component to show if there is no path in the url - just the raw domain (i.e. Home Page)

- Mandatory: Yes
- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'State'

Defines a single state variable on a UI Component

- Inherits From: ClassDefObject

### Attribute 'dataType'



- Data Type: String
### Attribute 'default'



- Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'many'



- Data Type: Boolean
### Attribute 'model'



- Data Type: String
### Attribute 'variable'



- Mandatory: Yes
- Data Type: String

# Class Definition - 'StyleControl'

Automatically inserted by code generation schemes which require separate intermediate component to control style (see attributes of 'Visual')

- Inherits From: ClassDefVisual

### Attribute 'Content' (Primary)

The content which is styled by this control

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'SubmitButton'

A button which triggers validation and submits user input

- Inherits From: Button

### Attribute 'label' (Primary)

Label of the button

- Mandatory: Yes
- Data Type: String
### Attribute 'Action'

An action to execute (either url or Action must be provided).

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'errorMessage'

Message to display upon error

- Data Type: String
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'style'

The visual style of the button

- Data Type: ButtonStyle
### Attribute 'successMessage'

Message to display upon success

- Data Type: String
### Attribute 'url'

Application Url to jump to (either url or Action must be provided).

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Tab'

Defines a tab of a *TabbedPane*.

- Inherits From: ClassDefObject

### Attribute 'Content' (Primary)

The visual content of the tab

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

Human-readable tab label.

- Data Type: String

# Class Definition - 'TabbedPane'

A component with labelled, user-selectable tabs that each show different content.

- Inherits From: ClassDefVisual

### Attribute 'Tabs' (Primary)

Definitions of the tabs

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'Table'

A table of data with rows and columns

- Inherits From: ClassDefVisual
- Expects Array of Data: Yes

### Attribute 'Columns' (Primary)

Definitions of the columns

- Mandatory: Yes
- Expects Array of Values: Yes
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'ExpandedContent'

If specified, clicking on a table row will show this extra content for the row inline, immediately under the row

### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'Header'

An optional header area connected to the table. It appears above the column headings. Typical use is for pagination controls.

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'selected'

Read/Write list of items which are currently selected

- Expects Array of Values: Yes
- Data Type: String
### Attribute 'selectionStyle'

The way in which table selection is shown

- Data Type: TableSelectionStyle
- Default Value: single
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TableColumn'

Defines a column of a *Table*.

- Inherits From: ClassDefObject

### Attribute 'Renderer' (Primary)

If present, this allows the <Table> cell contents to be rendered with arbitrary content. Similar to ItemTemplate in <List>.

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

<Table> column label.

- Data Type: String
### Attribute 'toolTip'

If present, an icon (?) will be placed after the column label - the Tool Tip message will be displayed to the user when they hover over the icon

- Data Type: String
### Attribute 'width'

Specifies the width of the table column in pixels

- Data Type: Float

# Class Definition - 'TablePageControls'

The pagination controls for the *Table* - e.g. 'Page 2 of 7', along with buttons to advance or flip back the page.

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TableSelectionColumn'

A special column which shows a checkbox for each row of the column. Allows multi-selection.

- Inherits From: TableColumn

### Attribute 'Renderer' (Primary)

If present, this allows the <Table> cell contents to be rendered with arbitrary content. Similar to ItemTemplate in <List>.

### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'label'

<Table> column label.

- Data Type: String
### Attribute 'toolTip'

If present, an icon (?) will be placed after the column label - the Tool Tip message will be displayed to the user when they hover over the icon

- Data Type: String
### Attribute 'width'

Specifies the width of the table column in pixels

- Data Type: Float

# Class Definition - 'Text'

Display text on the User Interface.

- Inherits From: TextualDisplay

### Attribute 'text' (Primary)

The text to display

- Data Type: String
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TextArea'

Multi-line editor for text. Suitable for longer descriptions.

- Inherits From: ClassDefEditable
- Expects Data Type: String

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TextEdit'

One-line editor for text

- Inherits From: ClassDefEditable
- Expects Data Type: String

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'prefix'

Prefix string to display before the edited text - e.g. '$'

- Data Type: String
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'suffix'

Suffix string to display after the edited text - e.g. 'kg'

- Data Type: String
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TextualDisplay'

Base class for all components which display text

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TimeDisplay'

Display an Time on the User Interface.

- Inherits From: TextualDisplay
- Expects Data Type: Time

### Attribute 'value' (Primary)

The value to display

- Data Type: Time
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TimeEditor'

Editor for a time.

- Inherits From: ClassDefEditable
- Expects Data Type: Time

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TimestampDisplay'

Display an Timestamp on the User Interface.

- Inherits From: TextualDisplay
- Expects Data Type: Timestamp

### Attribute 'value' (Primary)

The value to display

- Data Type: Timestamp
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'textColor'

The color of the text

- Data Type: Color
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'weight'

You can optionally make the text bold using this attribute

- Data Type: FontWeight
- Attached Attribute: Yes
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'TimestampEditor'

Editor for Date and Time (i.e. 'Timestamp')

- Inherits From: ClassDefEditable
- Expects Data Type: Timestamp

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'mandatory'



- Data Type: Boolean
- Default Value: True
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'readOnly'



- Data Type: Boolean
- Default Value: False
- Attached Attribute: Yes
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'UploadAction'

Dialog action to upload a file

- Inherits From: ActionWithDialog

### Attribute 'backEndTarget'

Name of back-end end-point to invoke when the action is executed.

- Data Type: String
### Attribute 'danger'

If true, visual feedback will be provided to the user to indicate that the action is dangerous - e.g. deleting or cancelling something.

- Data Type: Boolean
- Default Value: False
### Attribute 'dialogText'

Text explaining what the user is about to do

- Data Type: String
### Attribute 'dialogTitle'

The title of the dialog

- Mandatory: Yes
- Data Type: String
### Attribute 'fileFilter'

File filter - e.g. '*.csv'

- Mandatory: Yes
- Data Type: String
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'successUrl'

Url to jump to if the action is successful

- Data Type: String

# Class Definition - 'VerticalDivider'

A vertical divider line running the entire height of its container

- Inherits From: ClassDefVisual

### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

# Class Definition - 'VerticalStackPanel'

A layout panel which arranges its children veritcally, stacked on top of each other.

- Inherits From: ClassDefVisual

### Attribute 'Children' (Primary)

The contents/children of the panel.

- Expects Array of Values: Yes
### Attribute 'align'

Controls the horizontal alignment of the items in the Panel

- Data Type: HorizontalAlignment
### Attribute 'borderColor'

If specified, element will have a border of this color and some small padding

- Data Type: Color
### Attribute 'borderWidth'

If specified, element will have a border of this thickness and some small padding

- Data Type: Float
### Attribute 'fillColor'

Specifies background color of element

- Data Type: Color
### Attribute 'gap'

If present, specifies gap between items in pixels.

- Data Type: Integer
- Default Value: 8
### Attribute 'id'

Id for any purpose - e.g. debugging

- Data Type: String
### Attribute 'margin'

Margin around the component (in pixels). Margin is the distance between the border (if any) and the surrounding world.

- Data Type: Float
### Attribute 'marginBottom'

Margin below the component (in pixels)

- Data Type: Float
### Attribute 'marginLeft'

Margin to the left of the component (in pixels)

- Data Type: Float
### Attribute 'marginRight'

Margin to the right of the component (in pixels)

- Data Type: Float
### Attribute 'marginTop'

Margin on top of the component (in pixels)

- Data Type: Float
### Attribute 'maxWidth'

Maximum width that the object should have in the UI (in pixels)

- Data Type: Float
### Attribute 'padding'

Padding around the component (in pixels). Padding is the distance between the element and border (if any).

- Data Type: Float
### Attribute 'paddingBottom'

Padding below the component (in pixels)

- Data Type: Float
### Attribute 'paddingLeft'

Padding to the left of the component (in pixels)

- Data Type: Float
### Attribute 'paddingRight'

Padding to the right of the component (in pixels)

- Data Type: Float
### Attribute 'paddingTop'

Padding on top of the component (in pixels)

- Data Type: Float
### Attribute 'visible'

Is the object visible on the UI?

- Data Type: Boolean
### Attribute 'width'

Exact width that the object should have in the UI (in pixels)

- Data Type: Float

