import * as React from "react";

type Props = {
  readonly visible: boolean
  readonly children: React.JSX.Element,
};
export default function VisibilityControl(props: Props): React.JSX.Element | null {
  const {visible, children} = props;
  return visible ? children : null;
}
