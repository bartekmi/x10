// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";

import FormContext from "./FormContext";

export default function FormErrorDisplay(): React.Node {
  const errors = React.useContext(FormContext);

  return (
    <Group flexDirection="column" gap={0}>
      {errors.map(error => <Text color="red40" key={error}>{error}</Text>)}
    </Group>
  );
}