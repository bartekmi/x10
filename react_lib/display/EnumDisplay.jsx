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
    const valueIsString = typeof value === 'string';

    const displayOption = valueIsString ?
     options.find(x => x.value == value.toLowerCase()) : null;

    if (displayOption) {
      display = displayOption.label;
    }
  }

  if (display != null) {
    display = display.toString();
  }

  return (
    <Text weight={weight || "regular"}>
      {display}
    </Text>
  );
}
