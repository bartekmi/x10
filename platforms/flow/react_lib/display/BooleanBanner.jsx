// @flow

import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import Text from "latitude/Text";
import Icon from "latitude/Icon";
import Group from "latitude/Group";
import colors from "latitude/colors";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";
import {type IconNames} from "latitude/tools/icons";

type Props = {|
  +value: bool,
  +label?: string,
  +icon?: IconNames,
  +weight?: "bold",
|};
export default function BooleanBanner(props: Props): React.Node {
  const {value, label, icon, weight} = props;

  return value ? (
    <div className={css(styles.border)}>
      <Group>
        {icon == null ? null : <Icon iconName={icon}/> }
        <Text weight={weight || "regular"}>
          {label}
        </Text>
      </Group>
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