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
      isChecked={checked || false}
      disabled={disabled}
      onChange={(event) => {
        const newValue = event.target.checked;
        if (checked !== newValue && onChange) {
          onChange(newValue)
        }
      }}
    >
      {label}
    </ChakraCheckbox>
  );
}
