// @flow

import * as React from "react";

import VerticalStackPanel from "../layout/VerticalStackPanel";

type Props = {|
  +children: React.Node,
|};
export default function DisplayForm(props: Props): React.Node {
  const { children } = props

  return (
    <VerticalStackPanel gap={20}>
      { children }
    </VerticalStackPanel>
  );
}
