// @flow

import { bool } from "prop-types";
import * as React from "react";

type Props = {|
  +visible: bool,
  +children: React.Node,
|};
export default function VisibilityControl(props: Props): React.Node {
  const {visible, children} = props;
  return visible ? children : null;
}
