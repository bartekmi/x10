import * as React from "react";
// import {StyleSheet, css} from "aphrodite";

type Alignment = "flex-start" | "center" | "flex-end";
type Justification = "flex-start" | "flex-end" | "center" | "space-between" | "space-around" | "space-evenly";

type Props = {
  readonly gap?: number,
  readonly align?: Alignment,
  readonly justifyContent?: Justification,
  readonly children?: JSX.Element | JSX.Element[],
};
export default function Group(props: Props) {
  const { gap = 8, align = "center", justifyContent = "flex-start", children } = props  
  const elements = React.Children.toArray(children);

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: align,
        margin: `-${gap}px -${gap / 2}px 0 -${gap / 2}px`,
        pointerEvents: "none",
      }}
    >
      {elements.map((element, i) => (
        <div
          key={i}
          style={{
            margin: `${gap}px ${gap / 2}px 0 ${gap / 2}px`,
            pointerEvents: "auto",
          }}
        >
          {element}
        </div>
      ))}
    </div>
  );
}
