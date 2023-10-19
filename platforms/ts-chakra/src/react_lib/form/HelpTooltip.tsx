import * as React from "react";

import { Tooltip } from '@chakra-ui/react'
import { QuestionIcon } from '@chakra-ui/icons'

type Props = {
  text: string,
};

export default function HelpTooltip(props: Props): React.JSX.Element | null {
  let {text} = props;

  return (
    <Tooltip label={text}>
      <QuestionIcon/>
    </Tooltip>
  );
}
