import * as React from "react";

import { Radio, RadioGroup as ChakraRadioGroup } from '@chakra-ui/react'

type OptionObject<T extends string> = {
  value: T,
  label: string,
}

type Props<T extends string> = {
  readonly value: string | null | undefined,
  readonly onChange: (newValue: T) => void,
  readonly excludeItems?: string,
  readonly layout?: "vertical" | "horizontal",
  readonly options: OptionObject<T>[],  
};
export default function RadioGroup<T extends string>(props: Props<T>): React.JSX.Element {
  let {value, onChange, excludeItems, layout, options} = props;

  if (excludeItems) {
    const excludedAsArray = excludeItems.split(",").map(x => x.trim());
    options = options.filter(item => !excludedAsArray.includes(item.value));
  }

  return (
    <ChakraRadioGroup onChange={ (newValue) => onChange(newValue as T) }>
      {options.map(option => <Radio marginRight={10}
        key = {option.value}
        value = {option.value}
        isChecked = {option.value == value}
      >{option.label}</Radio>)}
    </ChakraRadioGroup>
  );
}
