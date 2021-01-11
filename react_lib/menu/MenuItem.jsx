// @flow

import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import Group from "latitude/Group";
import Link from "latitude/Link";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";
import colors from "latitude/colors";

type Props = {|
  +label: string,
  +href: string,
|};
export default function MenuItem(props: Props): React.Node {
  const {label, href} = props;
  return (
    <div className={css(styles.menuItem)}>
      <Link href={href}>{label}</Link>
    </div>
  )
}


const styles = StyleSheet.create({
  menuItem: {
    background: colors.grey20,
    padding: whitespaceSizeConstants.m,
    margins: "0px",
  },
});