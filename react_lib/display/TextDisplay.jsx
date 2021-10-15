// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?string | ?number,
  +weight?: "bold",
  +textColor?: string,
  // If needed, add bool prop to control behavior when blank (see comment below)
|};
export default function TextDisplay(props: Props): React.Node {
  let {value, weight, textColor} = props;

  // Without this, blank ("") texts leaves a gap, which generally looks bad
  if (typeof value === "string" && value.trim() === "")
    value = null;

  if (value != null) {
    value = value.toString();
  }

  return (
    // $FlowExpectedError error around textColor
    <Text weight={weight || "regular"} color={textColor}>
      {value}
    </Text>
  );
}
