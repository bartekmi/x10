// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";

import {FormContext} from "./FormProvider";

type Props = {|
  +paths?: string,
|};
export default function FormErrorDisplay(props: Props): React.Node {
  const {paths} = props;
  const formContext = React.useContext(FormContext);

  let errors = formContext.errors;
  if (paths != null) {
    // 'paths' is a comma-separated list of string paths
    // We only want to show errors whose 'paths' property contains a value which is in the
    // set of 'paths' as defined above (i.e. the sets intersect)
    const pathsArray = paths.split(",").map(x => x.trim());
    errors = errors.filter(x => intersect(pathsArray, x.paths));
  }

  // In the future, these errors could be clickable and scroll to relevant field
  return (
    <Group flexDirection="column" gap={0}>
      {errors.map(error => <Text color="red40" key={error.error}>{error.error}</Text>)}
    </Group>
  );
}

function intersect(a: $ReadOnlyArray<string>, b: $ReadOnlyArray<string>): boolean {
  return a.some(x => b.includes(x));
}