// @flow

import * as React from "react";

type Props = {|
  +visible?: bool,
  +width?: number,
  +maxWidth?: number,
  +margin?: number,
  +marginTop?: number,
  +marginRight?: number,
  +marginBottom?: number,
  +marginLeft?: number,
  +children: React.Node,
|};
export default function StyleControl(props: Props): React.Node {
  const {visible, width, maxWidth, margin, marginTop, marginRight, marginBottom, marginLeft, children} = props;
  if (visible  === false) {
    return null;
  }

  const style = {};

  if (width != null)          style["width"] = width.toString() + "px";
  if (maxWidth != null)       style["maxWidth"] = maxWidth.toString() + "px";
  if (margin != null)         style["margin"] = margin.toString() + "px";
  if (marginTop != null)      style["marginTop"] = marginTop.toString() + "px";
  if (marginRight != null)    style["marginRight"] = marginRight.toString() + "px";
  if (marginBottom != null)   style["marginBottom"] = marginBottom.toString() + "px";
  if (marginLeft != null)     style["marginLeft"] = marginLeft.toString() + "px";

  if (Object.keys(style).length == 0) {
    return children;
  }

  return (
    <div style={style}>
      {children}
    </div>
  );
}
