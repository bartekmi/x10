// @flow

import * as React from "react";

type Props = {|
  +onText: string,
  +offText: string,
  +onChange: (value: boolean) => void,
|};

export default function DummyStatefulComponent(props: Props): React.Node {
  const {onText, offText, onChange} = props;

  const [isOn, setOn] = React.useState(false);

  return (
    <>
      <p>{isOn ? onText : offText}</p>
      <button onClick={() => {
        const newState = !isOn;
        setOn(newState);
        onChange(newState);
       }}>
        Click Me
      </button>
    </>
  );
}