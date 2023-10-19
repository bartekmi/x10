// @flow

import * as React from "react";

import LatitudeButton from "latitude/button/Button";
import TextLink from "latitude/TextLink";

type Props = {|
  +label: ?string,          // Allow null primarily to appease Flow
  +onClick?: () => mixed,
  +url?: string,
  +style?: "normal" | "link"
|};
export default function Button(props: Props): React.Node {
  const {label, onClick, url, style} = props;

  const kind = style === "link" ? "bare" : "solid";
  
  return (
    url == null ?
      <LatitudeButton onClick={onClick} kind={kind} intent="basic" size="s">{label}</LatitudeButton> :
      <TextLink href={url}>{label}</TextLink>
  );
}
