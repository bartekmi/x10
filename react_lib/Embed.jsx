// @flow

import * as React from "react";
import Group from "latitude/Group";

type Props = {|
  +url: string,
|};
export default function Embed(props: Props): React.Node {
  const {url} = props;

  return (
    <Group flexDirection="column" alignItems="center">
      <a href={url} target="_blank">Visit</a>
      <embed src={url}/>
    </Group>
  );
}
