// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import colors from "latitude/colors";

type Props = {|
  +orientation?: "vertical" | "horizontal"
|};
export default function Separator(props: Props): React.Node {
  const {orientation} = props;

  return (
    <div className={css(orientation === "vertical" ? styles.vertical : styles.horizontal)}/>
  );
}

const styles = StyleSheet.create({
  horizontal: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey30}`,
    margin: "1em 0",
    padding: "0",
  },
  vertical: {
    width: "2px",
    borderLeft: `1px solid ${colors.grey30}`,
    margin: "1em 0",
    padding: "0",
  },
});
