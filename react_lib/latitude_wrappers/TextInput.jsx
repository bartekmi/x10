// @flow

import * as React from "react";

import LatitudeTextInput from "latitude/TextInput";

type Props = {|
  +value: ?string,
  +readOnly?: boolean,
  +prefix?: string,
  +suffix?: string,
  +onChange: (string) => void,
|};
export default function TextInput(props: Props): React.Node {
  const {value, readOnly, prefix, suffix, onChange} = props;

  return (
    <LatitudeTextInput
      value={value || ""}
      readOnly={readOnly}
      prefix={prefix}
      suffix={suffix}
      onChange={onChange}
    />
  );
}
