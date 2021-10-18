// @flow

import * as React from "react";

type Props = {|
  +url: string,
|};
export default function Embed(props: Props): React.Node {
  const {url} = props;

  return (
    <embed src={url}/>
  );
}
