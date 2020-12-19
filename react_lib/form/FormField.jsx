// @flow

import * as React from "react";

import Label from "latitude/Label";
import InputError from "latitude/InputError";

import {FormContext} from "./FormProvider";

type Props = {|
  +children: React.Node,
  +label: string,
  +indicateRequired?: boolean,
  +errorMessage?: string | null,
  +errorMessageFullContext?: string,
  +toolTip?: string,
  +maxWidth?: number,
|};
export default function FormField(props: Props): React.Node {
  const errors = React.useContext(FormContext);
  const {children, label, indicateRequired = false, errorMessage, errorMessageFullContext = null, toolTip, maxWidth} = props;

  if (errorMessage != null) {
    errors.push(errorMessageFullContext || errorMessage);
  }

  const style = maxWidth ? {
    maxWidth: maxWidth + "px"
  } : {};

  return (
    <Label 
      value={label} 
      indicateRequired={indicateRequired}
      helpTooltip={toolTip}
    >
      <InputError errorText={errorMessage || null} showError={errorMessage != null}>
        <div style={style}>
          {children}
        </div>
      </InputError>
    </Label>
  );
}