// @flow

import * as React from "react";

import LatRadioGroup, {type OptionObject} from "latitude/radio/RadioGroup";

type Props = {|
  +value: ?string,
  +onChange: (newValue: string) => void,
  +excludeItems?: string,
  +layout?: "vertical" | "horizontal",
  +options: $ReadOnlyArray<OptionObject<string>>,  
|};
export default function RadioGroup(props: Props): React.Node {
  let {value, onChange, excludeItems, layout, options} = props;

  if (excludeItems) {
    const excludedAsArray = excludeItems.split(",").map(x => x.trim());
    options = options.filter(item => !excludedAsArray.includes(item.value));
  }

  return (
    <LatRadioGroup
      value={value }
      onChange={ (newValue) => {
        onChange(newValue)
      } }
      options={ options }
    />
);
}
