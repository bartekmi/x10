// @flow

import * as React from "react";

import Label from "latitude/Label";
import HelpTooltip from "latitude/HelpTooltip";
import InputValidationMessage from "latitude/InputValidationMessage";

import { errorMessageForPath } from "./FormProvider";
import isBlank from "../utils/isBlank";

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