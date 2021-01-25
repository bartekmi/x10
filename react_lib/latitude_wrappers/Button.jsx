// @flow

import * as React from "react";

import LatitudeButton from "latitude/button/Button";
import TextLink from "latitude/TextLink";

type Props = {|
  +label: string,
  +url?: string,
|};
export default function Button(props: Props): React.Node {
  const {label, url} = props;
  return (
    url == null ?
      <LatitudeButton>{label}</LatitudeButton> :
      <TextLink href={url}>{label}</TextLink>
  );
}
