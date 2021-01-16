// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";

import {FormContext} from "./FormProvider";

export default function FormErrorDisplay(): React.Node {
  const errors = React.useContext(FormContext);

  // In the future, these errors could be clickable and scroll to relevant field
  return (
    <Group flexDirection="column" gap={0}>
      {errors.map(error => <Text color="red40" key={error.error}>{error.error}</Text>)}
    </Group>
  );
}