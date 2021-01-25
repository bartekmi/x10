// @flow

import * as React from "react";

import LatitudeTextInput from "latitude/TextInput";

type Props = {|
  +value: ?string,
  +readOnly?: boolean,
  +onChange: (string) => void,
|};
export default function TextInput(props: Props): React.Node {
  const {value, readOnly, onChange} = props;

  return (
    <LatitudeTextInput
      value={value || ""}
      readOnly={readOnly}
      onChange={onChange}
    />
  );
}
