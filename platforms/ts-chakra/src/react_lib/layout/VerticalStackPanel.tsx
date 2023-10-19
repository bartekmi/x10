import * as React from "react";
import {StyleSheet, css} from "aphrodite";

type Alignment = "left" | "center" | "right";

type Props = {
  readonly gap?: number,
  readonly align?: Alignment,
  readonly children: JSX.Element | JSX.Element[],
};
export default function VerticalStackPanel(props: Props): React.JSX.Element {
  const { gap = 8, align = "left", children } = props

  const styles = StyleSheet.create({
    styling: {
      display: "flex", 
      flexDirection: "column", 
      rowGap: `${gap}px`,
      justifyContent: toJustifyContent(align),
    },
  });
  
  return (
    <div className={css(styles.styling)}>
      { children }
    </div>
  );
}

type Justify = "flex-start" | "center" | "flex-end";

function toJustifyContent(align: Alignment): Justify | undefined {
  switch (align) {
    case "left": return "flex-start";
    case "center": return "center";
    case "right": return "flex-end";
  }

  return undefined;
}

