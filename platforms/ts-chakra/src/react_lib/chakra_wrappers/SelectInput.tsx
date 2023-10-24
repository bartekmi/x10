import * as React from "react";

import { Select as ChakraSelect } from '@chakra-ui/react'

type Option<T extends string> = {
  readonly value?: T,
  readonly label: string,
};
type Props<T extends string> = {
  readonly value: T | null | undefined,
  readonly readOnly?: boolean,
  readonly options: Option<T>[],
  readonly onChange: (value?: T) => unknown,
  readonly isNullable?: boolean,
};
export default function SelectInput<T extends string>(props: Props<T>): React.JSX.Element {
  const {value, readOnly, options, onChange, isNullable} = props;
  //const valueIsNumber = options.length > 0 && typeof options[0].value === "number";

  function generateOptions() : Option<T>[] {
    const optionsWithNull: Option<T>[] = [];
    if (isNullable) {
      optionsWithNull.push({ value: undefined, label: ''});
    }
    return [...optionsWithNull, ...options];
  }

  return (
    <ChakraSelect
      value={value?.toString()}
      isReadOnly={readOnly}
      // onChange={newValue => onChange(valueIsNumber ? parseInt(newValue.toString()) : newValue.toString()) }
      onChange={e => onChange(e.target.value as T) }
    >
      {generateOptions().map(option => <option 
        key={option.value}
        value={option.value}
      >
        {option.label}
      </option>)}
    </ChakraSelect>
  );
}
