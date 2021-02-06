// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import colors from "latitude/colors";

type Props = {|
    
|};
export default function Expander(): React.Node {
  return (
    <div className={css(styles.main)}/>
  );
}

const styles = StyleSheet.create({
  main: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey30}`,
    margin: "1em 0",
    padding: "0",
  },
});
