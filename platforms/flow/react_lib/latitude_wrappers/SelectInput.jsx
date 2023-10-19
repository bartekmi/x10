// @flow

import * as React from "react";

import LatitudeSelectInput from "latitude/select/SelectInput";

type Option = {
  +value: string | ?number,
  +label: string,
};
type Props = {|
  +value: ?string | ?number,
  +readOnly?: boolean,
  +options: $ReadOnlyArray<Option>,
  +onChange: (string | number) => void,
|};
export default function SelectInput(props: Props): React.Node {
  const {value, readOnly, options, onChange} = props;
  const valueIsNumber = options.length > 0 && typeof options[0].value === "number";

  return (
    <LatitudeSelectInput
      value={value?.toString()}
      readOnly={readOnly}
      options={options.map(x => ({
        value: x.value?.toString() || "",
        label: x.label,
      }))}
      // $FlowIgnoreError
      onChange={newValue => onChange(valueIsNumber ? parseInt(newValue) : newValue) }
      isNullable={true}
    />
  );
}
