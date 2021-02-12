// @flow

import * as React from "react";

import Text from "latitude/Text";

type Props = {|
  +value: ?number,
  +weight?: "bold",
|};
export default function TextInput(props: Props): React.Node {
  const {value, weight} = props;

  // TODO: Format to precision

  return (
    <Text weight={weight || "regular"}>
      {value}
    </Text>
  );
}
