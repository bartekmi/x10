// @flow

import { bool } from "prop-types";
import * as React from "react";

type Props = {|
  +visible?: bool,
  +width?: number,
  +maxWidth?: number,
  +children: React.Node,
|};
export default function StyleControl(props: Props): React.Node {
  const {visible, width, maxWidth, children} = props;
  if (visible  === false) {
    return null;
  }

  if (width || maxWidth) {
    const style = {};

    if (width != null) {
      style["width"] = width.toString() + "px";
    }
    if (maxWidth != null) {
      style["maxWidth"] = maxWidth.toString() + "px";
    }

    return (
      <div style={style}>
        {children}
      </div>
    );
  }

  return children;
}
