import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import { Link } from '@chakra-ui/react'
import colors from "../colors";

type Props = {
  readonly label: string,
  readonly href: string,
};
export default function MenuItem(props: Props): React.JSX.Element {
  const {label, href} = props;
  return (
    <div className={css(styles.menuItem)}>
      <Link href={href}>{label}</Link>
    </div>
  )
}


const styles = StyleSheet.create({
  menuItem: {
    background: colors.green20,
    padding: 12,
    margins: "0px",
  },
});