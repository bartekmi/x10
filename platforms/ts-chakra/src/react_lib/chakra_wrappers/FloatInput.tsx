import * as React from "react";

import { NumberInput } from '@chakra-ui/react'

type Props = {
  readonly value?: number,
  readonly precision?: number,
  readonly readOnly?: boolean,
  readonly onChange: (value: number) => void,
  readonly prefix?: string, // TODO
  readonly suffix?: string, // TODO
};
export default function TextInput(props: Props): React.JSX.Element {
  const {value, readOnly=false, precision, onChange} = props;

  return (
    <NumberInput
      value={value || ""}
      isDisabled={readOnly}
      precision={precision}
      onChange={(valueAsString: string, valueAsNumber: number) => {
        onChange(valueAsNumber)
      }}
    />
  );
}
