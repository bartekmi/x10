import * as React from "react";

import { Text } from '@chakra-ui/react'
import { Flex } from '@chakra-ui/react'

type Option = {
  readonly label: string,
  readonly value: string,
  readonly icon?: string,
};

type Props = {
  readonly value?: string,
  readonly options: Option[],
  readonly weight?: "bold",
  readonly hideLabelIfIconPresent?: boolean,
};
export default function EnumDisplay(props: Props): React.JSX.Element {
  const {value, options, weight, hideLabelIfIconPresent} = props;

  let display = value;
  let icon: string | null = null;

  if (value != null) {
    const valueIsString = typeof value === 'string';

    const displayOption = valueIsString ?
      options.find(x => x.value == value.toLowerCase()) : null;

    if (displayOption) {
      display = displayOption.label;
      icon = displayOption.icon || null;
    }
  }

  if (display != null) {
    display = display.toString();
  }

  const text = 
    <Text as={weight == "bold" ? "b" : undefined}>
      {display}
    </Text>

  if (icon) {
    return (
      <Flex>
        {/*<Icon iconName={icon} size="m"/>   TODO   */}
        {hideLabelIfIconPresent ? null : text}
      </Flex>
    )
  } else {
    return text;
  }
}
