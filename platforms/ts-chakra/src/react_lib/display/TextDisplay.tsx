import * as React from "react";

import { Text } from '@chakra-ui/react'

type Props = {
  readonly value?: string | number | null,
  readonly weight?: "bold",
  readonly textColor?: string,
  // If needed, add bool prop to control behavior when blank (see comment below)
};
export default function TextDisplay(props: Props): React.JSX.Element | null {
  let {value, weight, textColor} = props;

  // Without this, blank ("") texts leaves a gap, which generally looks bad
  if (value == null ||
    (typeof value === "string" && value.trim() === "") ||
    (typeof value === "number" && isNaN(value))) {
        return null;
  }

  // Otherwise, things will blow up if value is, say, an object
  value = value.toString();

  return (
    <Text 
      as={weight === "bold" ? "b" : undefined}
      textColor={textColor}
    >
      {value}
    </Text>
  );
}
