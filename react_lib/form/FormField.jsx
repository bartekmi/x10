// @flow

import * as React from "react";

import Label from "latitude/Label";
import HelpTooltip from "latitude/HelpTooltip";
import InputValidationMessage from "latitude/InputValidationMessage";

import { errorMessageForPath } from "./FormProvider";

type Props = {|
  +children: React.Node,
  +editorFor: string, // Path of field that this field represents. Used for error display.
  +label: string,
  +indicateRequired?: boolean,
  +toolTip?: string,
  +maxWidth?: number,
|};
export default function FormField(props: Props): React.Node {
  const {children, editorFor, label, indicateRequired = false, toolTip, maxWidth} = props;

  const style = maxWidth ? {
    maxWidth: maxWidth + "px"
  } : {};

  const errorMessage = errorMessageForPath(editorFor);

  return (
    <Label 
      value={label} 
      indicateRequired={indicateRequired}
      helpTooltip={toolTip ? <HelpTooltip
          iconName="question"
          position="top"
          size="s"
          text={toolTip}
        /> : null}
    >
      <InputValidationMessage errorText={errorMessage || null} showError={errorMessage != null}>
        <div style={style}>
          {children}
        </div>
      </InputValidationMessage>
    </Label>
  );
}