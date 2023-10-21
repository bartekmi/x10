import * as React from "react";

import { NumberInput } from '@chakra-ui/react'

type Props = {
  readonly value: number | null | undefined,
  readonly decimalPrecision?: number,
  readonly readOnly?: boolean,
  readonly onChange: (value: number) => void,
  readonly prefix?: string, // TODO
  readonly suffix?: string, // TODO
};
export default function TextInput(props: Props): React.JSX.Element {
  const {value, readOnly=false, decimalPrecision, onChange} = props;

  return (
    <NumberInput
      value={value || ""}
      isDisabled={readOnly}
      precision={decimalPrecision}
      onChange={(valueAsString: string, valueAsNumber: number) => {
        onChange(valueAsNumber)
      }}
    />
  );
}
