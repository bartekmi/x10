// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import Text from "latitude/Text";
import {whitespaceSizeConstants} from "latitude/styles/whitespace";

type Props = {|
  +children: React.Node,
  +label?: string,
|};
export default function FormSection(props: Props): React.Node {
  const { children, label } = props

  return (
    <>
      <div className={css(styles.heading)}>
        <Text scale="headline">{label}</Text>
      </div>
      {children}
    </>
  );
}

const styles = StyleSheet.create({
  heading: {
    paddingTop: whitespaceSizeConstants.m,
    paddingBottom: whitespaceSizeConstants.m,
  },
});
