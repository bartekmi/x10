// @flow

import moment from "moment-timezone";
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

  const theMoment = moment(value, [moment.ISO_8601, "YYYY-MM-DD"]);
  const formattedDate = theMoment.format("YYYY-MM-DD");
  const formattedTime = theMoment.format("h:mm A");

  const nonNullWeight = weight || "regular";

  return (
    <Group gap={12}>
      <Text weight={nonNullWeight}>{formattedDate}</Text>
      <Text weight={nonNullWeight}>{formattedTime}</Text>
    </Group>
  );
}
