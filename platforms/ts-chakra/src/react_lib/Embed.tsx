import * as React from "react";
import { Flex } from '@chakra-ui/react'

type Props = {
  url: string,
};
export default function Embed(props: Props): React.JSX.Element {
  const {url} = props;

  return (
    <Flex flexDirection="column" alignItems="center">
      <a href={url} target="_blank">Visit</a>
      <embed src={url}/>
    </Flex>
  );
}
