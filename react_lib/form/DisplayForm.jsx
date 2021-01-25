// @flow

import * as React from "react";

import Group from "latitude/Group";

type Props = {|
  +children: React.Node,
|};
export default function DisplayForm(props: Props): React.Node {
  const { children } = props

  return (
    <Group flexDirection="column">
      { children }
    </Group>
  );
}
