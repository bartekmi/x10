// @flow

import * as React from "react";

import Label from "latitude/Label";
import HelpTooltip from "latitude/HelpTooltip";
import InputValidationMessage from "latitude/InputValidationMessage";

import isBlank from "../utils/isBlank";
import { FormContext } from "./FormProvider";

type Props = {|
  +children: React.Node,
  +editorFor: string,     // Path of field that this field represents. Used for error display.
  +inListIndex?: number,  // If this for field is in a list, the index of the item
  +label: string,
  +indicateRequired?: boolean,
  +toolTip?: string,
  +maxWidth?: number,
|};
export default function FormField(props: Props): React.Node {
  const {
    children, 
    editorFor, 
    inListIndex, 
    label, 
    indicateRequired = false, 
    toolTip, 
    maxWidth
  } = props;

  const style = maxWidth ? {
    maxWidth: maxWidth + "px"
  } : {};

  const errorMessage = errorMessageForPath(editorFor, inListIndex);

  if (isBlank(label)) {
    return <RawField errorMessage={errorMessage} style={style} children={children}/>
  }

  return (
    <Label 
      value={label} 
      paddingBottom={"none"}
      indicateRequired={indicateRequired}
      helpTooltip={toolTip ? <HelpTooltip
          iconName="question"
          position="top"
          size="s"
          text={toolTip}
        /> : null}
    >
      <RawField errorMessage={errorMessage} style={style} children={children}/>
    </Label>
  );
}

function errorMessageForPath(path: string, index?: number): string | null {
  const context = React.useContext(FormContext);
  let errors = context.errors.filter(x => x.paths.includes(path));

  if (index != null) {
    errors = errors.filter(x => x.inListIndex == index);
  }

  return errors.length == 0 ? null : errors.map(x => x.error).join("\n");
}

type RawFieldProps = {|
  +children: React.Node,
  +errorMessage: ?string,
  +style: any,
|};
function RawField(props: RawFieldProps): React.Node {
  const { errorMessage, style, children } = props;

  return (
  <InputValidationMessage errorText={errorMessage || null} showError={errorMessage != null}>
    <div style={style}>
      {children}
    </div>
  </InputValidationMessage>
  );
}