import * as React from "react";

import { Select as ChakraSelect } from '@chakra-ui/react'
import { isNull } from "util";

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
  let {value, readOnly, options, onChange, isNullable} = props;
  //const valueIsNumber = options.length > 0 && typeof options[0].value === "number";

  // TODO...
  // Even if a field is mandatory, when creating a brand-new object still needs a blank
  // value to prevent an undesired default.
  // The caller of this component need to know the context - first-time creation or edit.
  isNullable = true;

  let optionsMaybeNull = [];
  if (isNullable) {
    optionsMaybeNull.push({ value: undefined, label: ''});
  }
  optionsMaybeNull = [...optionsMaybeNull, ...options];

  return (
    <ChakraSelect
      value={value?.toString()}
      isReadOnly={readOnly}
      // onChange={newValue => onChange(valueIsNumber ? parseInt(newValue.toString()) : newValue.toString()) }
      onChange={e => onChange(e.target.value as T) }
    >
      {
        optionsMaybeNull.map(option => <option 
          key={option.value}
          value={option.value}
        >
          {option.label}
        </option>)
      }
    </ChakraSelect>
  );
}
