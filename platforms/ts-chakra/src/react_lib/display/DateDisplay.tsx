import * as React from "react";

import { Text } from '@chakra-ui/react'

type Props = {
  readonly value?: string,
  readonly weight?: "bold",
};
export default function DateDisplay(props: Props): React.JSX.Element {
  const {value, weight} = props;

  return (
    <Text as={weight === "bold" ? "b" : undefined}>
      {toDisplay(value)}
    </Text>
  );
}

function toDisplay(timestamp?: string): string | null {
  if (timestamp == null) {
    return null;
  }

  const index = timestamp.indexOf("T");
  if (index == -1) {
    throw "Invalid date/time format: " + timestamp;
  }
 
  const date = timestamp.substr(0, index);
  return date;
}
