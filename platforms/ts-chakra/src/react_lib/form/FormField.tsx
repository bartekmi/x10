import * as React from "react";

import Label from "./Label";
import InputError from "./InputError";

import isBlank from "../utils/isBlank";
import { FormContext } from "./FormProvider";

type Props = {
  readonly children: React.JSX.Element,
  readonly editorFor: string,     // Path of field that this field represents. Used for error display.
  readonly inListIndex?: number,  // If this for field is in a list, the index of the item
  readonly label: string,
  readonly indicateRequired?: boolean,
  readonly toolTip?: string,
  readonly maxWidth?: number,
};
export default function FormField(props: Props): React.JSX.Element {
  const context = React.useContext(FormContext);

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

  function errorMessageForPath(path: string, index?: number): string | null {
    let errors = context.errors.filter(x => x?.paths.includes(path));

    if (index != null) {
      errors = errors.filter(x => x.inListIndex === index);
    }

    return errors.length === 0 ? null : errors.map(x => x.error).join("\n");
  }

  const errorMessage = errorMessageForPath(editorFor, inListIndex);

  if (isBlank(label)) {
    return <FieldWithMessage errorMessage={errorMessage} style={style} children={children}/>
  }

  return (
    <Label 
      value={label} 
      indicateRequired={indicateRequired}
      helpTooltip={toolTip}
    >
      <FieldWithMessage errorMessage={errorMessage} style={style} children={children}/>
    </Label>
  );
}

type FieldWithMessageProps = {
  readonly children: React.JSX.Element,
  readonly errorMessage: string | null,
  readonly style: any,
};
function FieldWithMessage(props: FieldWithMessageProps): React.JSX.Element {
  const { errorMessage, style, children } = props;

  return (
  <InputError errorText={errorMessage || null} showError={errorMessage != null}>
    <div style={style}>
      {children}
    </div>
  </InputError>
  );
}