// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?number,
  +weight?: "bold",
  +decimalPlaces?: number,
|};
export default function FloatDisplay(props: Props): React.Node {
  let {value, weight, decimalPlaces} = props;

  if (decimalPlaces && value) {
    const tenToN = 10 ** decimalPlaces;
    value = Math.round(value * tenToN) / tenToN;  
  }

  return (
    <Text weight={weight || "regular"}>
      {value}
    </Text>
  );
}
