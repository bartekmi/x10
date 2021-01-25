// @flow

import * as React from "react";

import Label from "latitude/Label";
import HelpTooltip from "latitude/HelpTooltip";

type Props = {|
  +children: React.Node,
  +label: string,
    +toolTip ?: string,
    +maxWidth ?: number,
|};
export default function DisplayField(props: Props): React.Node {
  const { children, label, toolTip, maxWidth } = props;

  const style = maxWidth ? {
    maxWidth: maxWidth + "px"
  } : {};

  return (
    <Label
      value={label}
      helpTooltip={toolTip ? <HelpTooltip
        iconName="question"
        position="top"
        size="s"
        text={toolTip}
      /> : null}
    >
      <div style={style}>
        {children}
      </div>
    </Label>
  );
}
