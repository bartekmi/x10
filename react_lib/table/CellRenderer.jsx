// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

type Props = {|
  +children: React.Node
|};
export default function CellRenderer(props: Props): React.Node {
  return (
    <div className={css(styles.styling)}>
      {props.children}
    </div>
  );
}

const styles = StyleSheet.create({
  styling: {
    display: "flex",
    flexDirection: "column",
    minWidth: 0,
    textAlign: "left",
    justifyContent: "center",
  }
});
