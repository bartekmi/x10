// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";

type Props = {|
  +value: ?string,
  +weight?: "bold",
|};
export default function TimestampDisplay(props: Props): React.Node {
  const {value, weight} = props;

  // TODO: Formatting options
  if (value == null)
    return null;

  const [date, time] = extractDateAndTime(value);
  const nonNullWeight = weight || "regular";

  return (
    <Group gap={12}>
      <Text weight={nonNullWeight}>{date}</Text>
      <Text weight={nonNullWeight}>{time}</Text>
    </Group>
  );
}

function extractDateAndTime(timestamp: string): $ReadOnlyArray<string> {
  const index = timestamp.indexOf("T");
  if (index == -1) {
    throw "Invalid date/time format: " + timestamp;
  }
 
  const date = timestamp.substr(0, index);
  const time = timestamp.substr(index + 1, 8);

  return [date, time];
}
