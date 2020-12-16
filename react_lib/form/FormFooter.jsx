// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import {whitespaceSizeConstants} from "latitude/styles/whitespace";

type Props = {|
  +children: React.Node,
|};
export default function FormSection(props: Props): React.Node {
  const { children } = props

  return (
    <>
      <div className={css(styles.styling)}>
        {children}
      </div>
    </>
  );
}

const styles = StyleSheet.create({
  styling: {
    padding: whitespaceSizeConstants.l,
  },
});
