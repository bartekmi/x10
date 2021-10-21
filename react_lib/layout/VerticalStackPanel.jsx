// @flow

import * as React from "react";

import Group from "latitude/Group";

type Props = {|
  +gap?: number,
  +align?: "left" | "center" | "right",
  +children: React.Node,
|};
export default function VerticalStackPanel(props: Props): React.Node {
  const { gap = 8, align = "left", children } = props

  const style = {
    display: "flex", 
    flexDirection: "column", 
    rowGap: `${gap}px`,
    alignItems: align
  };

  return (
    <div style={style}>
      { children }
    </div>
  );
}
