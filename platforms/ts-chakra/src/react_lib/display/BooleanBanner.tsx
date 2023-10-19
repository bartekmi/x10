import * as React from "react";
import { StyleSheet, css } from "aphrodite";
import { Flex, Text } from '@chakra-ui/react'
import colors from "../colors";


type Props = {
  readonly value: boolean,
  readonly label?: string,
  // readonly icon?: IconNames,
  readonly weight?: "bold",
};
export default function BooleanBanner(props: Props): React.JSX.Element | null {
  const {value, label, /*icon,*/ weight} = props;

  return value ? (
    <div className={css(styles.border)}>
      <Flex>
        {/* icon == null ? null : <Icon iconName={icon}/>  TODO */}
        <Text as={weight == "bold" ? "b" : undefined}>
          {label}
        </Text>
      </Flex>
    </div>
  ) : null;
}

const styles = StyleSheet.create({
  border: {
    border: `3px solid ${colors.grey30}`,
    padding: 6,
    margins: "0px",
  },
});