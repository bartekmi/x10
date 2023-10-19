
import * as React from "react";
import {StyleSheet, css} from "aphrodite";
import colors from "./colors";

type Props = {
  readonly orientation?: "vertical" | "horizontal",
  readonly label?: string,
  readonly color?: string
};
export default function Separator(props: Props): React.JSX.Element {
  const {
    orientation = "horizontal", 
    label,
    color = colors.grey30,
  } = props;


  const styles = StyleSheet.create({
    horizontal: {
      height: "2px",
      borderBottom: `1px solid ${color}`,
      margin: "1em 0",
      padding: "0",
    },
    vertical: {
      width: "2px",
      borderLeft: `1px solid ${color}`,
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
      borderBottom: `1px solid ${color}`,
      margin: "1em 0",
      padding: "0",
      flexGrow: 1,
    },
    horizontalLabel: {
      padding: "0 10px",
    },
  });
  

  if (orientation === "horizontal" && label != null) {
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
