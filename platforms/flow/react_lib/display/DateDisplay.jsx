// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?string,
  +weight?: "bold",
|};
export default function DateDisplay(props: Props): React.Node {
  const {value, weight} = props;

  // TODO: Formatting options

  return (
    <Text weight={weight || "regular"}>
      {toDisplay(value)}
    </Text>
  );
}

function toDisplay(timestamp: ?string): string | null {
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
