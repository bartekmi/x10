import * as React from "react";

import { Text } from '@chakra-ui/react'

type Props = {
  readonly value: number | null | undefined,
  readonly weight?: "bold",
  readonly decimalPlaces?: number,
};
export default function FloatDisplay(props: Props): React.JSX.Element {
  let {value, weight, decimalPlaces} = props;

  if (decimalPlaces && value) {
    const tenToN = 10 ** decimalPlaces;
    value = Math.round(value * tenToN) / tenToN;  
  }

  return (
    <Text as={weight == "bold" ? "b" : undefined}>
      {value}
    </Text>
  );
}
