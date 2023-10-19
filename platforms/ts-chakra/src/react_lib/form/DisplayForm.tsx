import * as React from "react";

import VerticalStackPanel from "../layout/VerticalStackPanel";

type Props = {
  readonly children: React.JSX.Element,
  readonly gap?: number,
};
export default function DisplayForm(props: Props): React.JSX.Element {
  const { children, gap = 20 } = props

  return (
    <VerticalStackPanel gap={gap}>
      { children }
    </VerticalStackPanel>
  );
}
