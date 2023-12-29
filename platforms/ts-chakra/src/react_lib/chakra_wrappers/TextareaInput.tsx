import * as React from "react";

import { Textarea } from '@chakra-ui/react'

type Props = {
  readonly value?: string,
  readonly readOnly?: boolean,
  readonly placeholder?: string,
  readonly onChange?: (value: string) => void,
};
export default function TextInput(props: Props): React.JSX.Element {
  const {value, readOnly, placeholder, onChange} = props;

  return (
    <Textarea
      value={value || ""}
      readOnly={readOnly}
      placeholder={placeholder}
      onChange={(event) => {
        if (onChange) {
          onChange(event.target.value);
        }
      }}
    />
  );
}
