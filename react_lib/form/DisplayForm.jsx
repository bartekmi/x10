// @flow

import * as React from "react";

import VerticalStackPanel from "../layout/VerticalStackPanel";

type Props = {|
  +children: React.Node,
  +gap?: number,
|};
export default function DisplayForm(props: Props): React.Node {
  const { children, gap = 20 } = props

  return (
    <VerticalStackPanel gap={gap}>
      { children }
    </VerticalStackPanel>
  );
}
