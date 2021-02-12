// @flow

import * as React from "react";

import Text from "latitude/Text";

type Option = {
  +label: string,
  +value: string,
};

type Props = {|
  +value: ?string,
  +options: $ReadOnlyArray<Option>,
  +weight?: "bold",
|};
export default function EnumDisplay(props: Props): React.Node {
  const {value, options, weight} = props;

  let display = value;
  if (value != null) {
    const displayOption = options.find(x => x.value == value);
    if (displayOption) {
      display = displayOption.label;
    }
  }

  return (
    <Text weight={weight || "regular"}>
      {display}
    </Text>
  );
}
