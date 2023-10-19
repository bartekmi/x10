import * as React from "react";
import { Flex} from '@chakra-ui/react'

type Props = {
  readonly children: JSX.Element | JSX.Element[],
};
export default function Menu(props: Props): React.JSX.Element {
  const {children} = props;
  return <Flex>{children}</Flex>
}