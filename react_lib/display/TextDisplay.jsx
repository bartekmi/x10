// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?string,
  +weight?: "bold",
  // If needed, add bool prop to control behavior when blank (see comment below)
|};
export default function TextDisplay(props: Props): React.Node {
  let {value, weight} = props;

  // Without this, blank ("") texts leaves a gap, which generall looks bad
  if (typeof value === "string" && value.trim() === "")
    value = null;

  return (
    <Text weight={weight || "regular"}>
      {value}
    </Text>
  );
}
