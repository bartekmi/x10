// @flow

import * as React from "react";

import Text from "latitude/Text";
import Icon from "latitude/Icon";
import Group from "latitude/Group";

type Option = {
  +label: string,
  +value: string,
  +icon?: string,
};

type Props = {|
  +value: ?string,
  +options: $ReadOnlyArray<Option>,
  +weight?: "bold",
  +hideLabelIfIconPresent?: boolean,
|};
export default function EnumDisplay(props: Props): React.Node {
  const {value, options, weight, hideLabelIfIconPresent} = props;

  let display = value;
  let icon = null;

  if (value != null) {
    const valueIsString = typeof value === 'string';

    const displayOption = valueIsString ?
     options.find(x => x.value == value.toLowerCase()) : null;

    if (displayOption) {
      display = displayOption.label;
      icon = displayOption.icon;
    }
  }

  if (display != null) {
    display = display.toString();
  }

  const text = 
    <Text weight={weight || "regular"}>
      {display}
    </Text>

  if (icon) {
    return (
      <Group>
        <Icon iconName={icon} size="m"/>
        {hideLabelIfIconPresent ? null : text}
      </Group>
    )
  } else {
    return text;
  }
}
