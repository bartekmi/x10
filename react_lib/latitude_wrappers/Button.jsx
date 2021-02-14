// @flow

import * as React from "react";

import LatitudeButton from "latitude/button/Button";
import TextLink from "latitude/TextLink";

type Props = {|
  +label: string,
  +onClick?: () => mixed,
  +url?: string,
|};
export default function Button(props: Props): React.Node {
  const {label, onClick, url} = props;
  return (
    url == null ?
      <LatitudeButton onClick={onClick} kind="solid" intent="basic" size="s">{label}</LatitudeButton> :
      <TextLink href={url}>{label}</TextLink>
  );
}
