import * as React from "react";

import { Checkbox as ChakraCheckbox } from '@chakra-ui/react'

type Props = {
  readonly checked: boolean | null | undefined,
  readonly disabled?: boolean,
  readonly label?: string,
  readonly onChange?: (checked: boolean) => void,
};
export default function Checkbox(props: Props): React.JSX.Element {
  const {checked, disabled = false, label, onChange} = props;
  return (
    <ChakraCheckbox
      checked={checked || false}
      disabled={disabled}
      //onChange={(newState) => onChangeWrapper( checked, newState, onChange)}
      onChange={(event) => {
        const newValue = event.target.value;
        throw "Fix this Checkbox TODO: " + newValue
        // if (checked !== newState && onChange) {
        //   onChange(newState)
        // }
      }}
    >
      {label}
    </ChakraCheckbox>
  );
}
