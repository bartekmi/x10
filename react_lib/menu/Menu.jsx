// @flow

import * as React from "react";
import Group from "latitude/Group";

type Props = {|
  +children: React.Node,
|};
export default function Menu(props: Props): React.Node {
  const {children} = props;
  return <Group>{children}</Group>
}