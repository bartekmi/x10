// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";

import FormContext from "./FormContext";

export default function FormErrorDisplay(): React.Node {
  const errors = React.useContext(FormContext);

  return (
    <Group flexDirection="column">
      {errors.map(error => <Text key={error}>{error}</Text>)}
    </Group>
  );
}