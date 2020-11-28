// @flow

import * as React from "react";
import OnOffContext from "./OnOffContext";

type Props = {|
  +text: string,
|};

export default function DummyContextComponent(props: Props): React.Node {
  const {text} = props;

  const value = React.useContext(OnOffContext);
  const {valueBefore, valueAfter} = value;

  return (
    <>
      <p>{valueBefore}</p>
      <p>{text}</p>
      <p>{valueAfter}</p>
    </>
  );
}