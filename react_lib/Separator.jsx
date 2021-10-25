// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import colors from "latitude/colors";

type Props = {|
  +orientation?: "vertical" | "horizontal",
  +label?: string
|};
export default function Separator(props: Props): React.Node {
  const {
    orientation = "horizontal", 
    label
  } = props;

  if (orientation === "horizontal" && label != null) {
    // return (
    //   <div className={css(styles.horizontalWithLabel)}>{label}</div>
    // );
    return (
      <div className={css(styles.horizontalWithLabel)}>
        <div className={css(styles.horizontalGrow)}/>
        <div className={css(styles.horizontalLabel)}>{label}</div>
        <div className={css(styles.horizontalGrow)}/>
      </div>
    );
  }

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
  horizontalWithLabel: {
    display: 'flex',
    alignItems: 'center',
    textAlign: 'center',
  },
  horizontalGrow: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey30}`,
    margin: "1em 0",
    padding: "0",
    flexGrow: 1,
  },
  horizontalLabel: {
    padding: "0 10px",
  },
});
