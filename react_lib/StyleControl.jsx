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

  +padding?: number,

  +borderColor?: string,
  +borderWidth?: number,

  +fillColor?: string,

  +children: React.Node,
|};
export default function StyleControl(props: Props): React.Node {
  const {visible, 
    width, maxWidth, 
    margin, marginTop, marginRight, marginBottom, marginLeft, 
    padding,
    borderColor, borderWidth,
    fillColor,
    children} = props;

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

  if (padding != null)         style["padding"] = padding.toString() + "px";

  if (borderColor != null)         style["borderColor"] = borderColor;
  if (borderWidth != null)         style["borderWidth"] = borderWidth.toString() + "px";
  if (borderColor || borderWidth) {
    style["borderStyle"] = "solid";
  }

  if (fillColor != null)         style["backgroundColor"] = fillColor;

  if (Object.keys(style).length == 0) {
    return children;
  }

  return (
    <div style={style}>
      {children}
    </div>
  );
}
