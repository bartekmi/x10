// @flow

import * as React from "react";

import LatitudeCheckbox from "latitude/Checkbox";

type Props = {|
  +checked?: boolean,
  +disabled: boolean,
  +label?: string,
  +onChange?: (checked: boolean) => void,
|};
export default function Checkbox(props: Props): React.Node {
  const {checked, disabled, label, onChange} = props;
  return (
    <LatitudeCheckbox
      checked={checked || false}  // null/undefined input interpreted as 'false'
      disabled={disabled}
      label={label}
      onChange={(newState) => onChangeWrapper( checked, newState, onChange)}
    />
  );
}

function onChangeWrapper(reactState, newState, onChange) {
  if (reactState !== newState && onChange) {
    onChange(newState)
  }
}
