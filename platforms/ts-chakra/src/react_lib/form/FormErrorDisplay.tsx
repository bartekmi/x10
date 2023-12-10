import * as React from "react";

import { Flex, Text } from '@chakra-ui/react'

import {FormContext} from "./FormProvider";
import colors from "../colors";

type Props = {
  // This optional setting allows the caller to narrow the 
  // set of errors that are shown. 
  readonly paths?: string,
};
export default function FormErrorDisplay(props: Props): React.JSX.Element {
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
    <Flex flexDirection="column" gap={0}>
      {errors.map(error => <Text color={colors.red40} key={error.error}>{error.error}</Text>)}
    </Flex>
  );
}

function intersect(a: string[], b: string[]): boolean {
  return a.some(x => b.includes(x));
}