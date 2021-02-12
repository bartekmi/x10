// @flow

import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import Text from "latitude/Text";
import colors from "latitude/colors";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";

type Props = {|
  +value: bool,
  +label: string,
  +weight?: "bold",
|};
export default function BooleanBanner(props: Props): React.Node {
  const {value, label, weight} = props;

  return value ? (
    <div className={css(styles.border)}>
      <Text weight={weight || "regular"}>
        {label}
      </Text>
    </div>
  ) : null;
}

const styles = StyleSheet.create({
  border: {
    border: `3px solid ${colors.grey30}`,
    padding: whitespaceSizeConstants.xs,
    margins: "0px",
  },
});