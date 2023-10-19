import * as React from "react";
import Label from "./Label";

type Props = {
  readonly children: React.JSX.Element,
  readonly label: string,
    readonly toolTip?: string,
    readonly maxWidth?: number,
};
export default function DisplayField(props: Props): React.JSX.Element {
  const { children, label, toolTip, maxWidth } = props;

  const style = maxWidth ? {
    maxWidth: maxWidth + "px"
  } : {};

  return (
    <Label
      value={label}
      helpTooltip={toolTip}
    >
      <div style={style}>
        {children}
      </div>
    </Label>
  );
}
