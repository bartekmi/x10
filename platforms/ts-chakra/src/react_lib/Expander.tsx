import * as React from "react";

import { Flex } from '@chakra-ui/react'
import { IconButton } from '@chakra-ui/react'
import { ArrowDownIcon, ArrowRightIcon } from '@chakra-ui/icons'

type Props = {
  // Why is header a function, but the body just a component?
  readonly headerFunc: () => React.JSX.Element,
  readonly body: React.JSX.Element, // The body when expanded
  readonly defaultState?: "open" | "closed"
};
export default function Expander(props: Props) : React.JSX.Element {
  const {headerFunc, body: children, defaultState = "open"} = props;
  const [expanded, setExpanded] = React.useState(defaultState == "open");

  return (
    <Flex flexDirection="column">
      <Flex alignItems="center" justifyContent="space-between">
        {headerFunc()}
        <IconButton 
          icon={expanded ? <ArrowDownIcon/> : <ArrowRightIcon/>} 
          aria-label={expanded ? "click to collapse" : "click to expand"} 
          type="button" 
          onClick={() => setExpanded(!expanded)} />
      </Flex>
      {expanded ? children : null}
    </Flex>
  );
}
