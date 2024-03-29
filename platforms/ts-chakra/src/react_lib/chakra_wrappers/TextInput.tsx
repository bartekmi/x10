import * as React from "react";

import { Input } from '@chakra-ui/react'

type Props = {
  readonly value?: string,
  readonly readOnly?: boolean,
  readonly prefix?: string,
  readonly suffix?: string,
  readonly placeholder?: string,
  readonly onChange: (value: string) => void,
};
export default function TextInput(props: Props): React.JSX.Element {
  const {value, readOnly, prefix, placeholder, onChange} = props;

  return (
    <Input
      value={value || ""}
      readOnly={readOnly}
      prefix={prefix}
      //suffix={suffix}       // TODO
      placeholder={placeholder}
      onChange={(event) => {
        onChange(event.target.value);
      }}
    />
  );
}
