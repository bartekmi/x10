// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?string,
  +weight?: "bold",
|};
export default function TimestampDisplay(props: Props): React.Node {
  const {value, weight} = props;

  // TODO: Format to precision

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
  const time = timestamp.substr(index + 1, 8);
  return `${date} ${time}`;
}
