import * as React from "react";

import { Select as ChakraSelect } from '@chakra-ui/react'

type Option<T extends string> = {
  readonly value: T,
  readonly label: string,
};
type Props<T extends string> = {
  readonly value?: T,
  readonly readOnly?: boolean,
  readonly options: Option<T>[],
  readonly onChange: (value?: T) => unknown,
};
export default function SelectInput<T extends string>(props: Props<T>): React.JSX.Element {
  const {value, readOnly, options, onChange} = props;
  //const valueIsNumber = options.length > 0 && typeof options[0].value === "number";

  return (
    <ChakraSelect
      value={value?.toString()}
      isReadOnly={readOnly}
      // onChange={newValue => onChange(valueIsNumber ? parseInt(newValue.toString()) : newValue.toString()) }
      onChange={e => onChange(e.target.value as T) }
    >
      {options.map(option => <option value={option.value}>{option.label}</option>)}
    </ChakraSelect>
  );
}
