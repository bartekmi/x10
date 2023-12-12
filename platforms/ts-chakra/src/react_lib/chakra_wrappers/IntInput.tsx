
import * as React from "react";

import { NumberInput, NumberInputField, NumberInputStepper, NumberIncrementStepper, NumberDecrementStepper } from '@chakra-ui/react'

type Props = {
  readonly value: number | null | undefined,
  readonly readOnly?: boolean,
  readonly onChange: (value: number) => void,
  readonly prefix?: string, // TODO
  readonly suffix?: string, // TODO
};
export default function TextInput(props: Props): React.JSX.Element {
  const {value, readOnly=false, onChange} = props;

  return (
    <NumberInput
      value={value || ""}
      isDisabled={readOnly}
      onChange={(_valueAsString: string, valueAsNumber: number) => {
        onChange(valueAsNumber)
      }}
    >
      <NumberInputField/>
      <NumberInputStepper>
        <NumberIncrementStepper />
        <NumberDecrementStepper />
      </NumberInputStepper>      
    </NumberInput>
  );
}
