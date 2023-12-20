import * as React from "react";

import { Flex } from '@chakra-ui/react'
import './FormSection.css'

type Props = {
  readonly children: JSX.Element | JSX.Element[],
  readonly label?: string,
};
export default function FormSection(props: Props): React.JSX.Element {
  const { children, label } = props

  return (
    <div className='form-section-component'>
      <div className='form-section-header'>
        <h2>{label}</h2>
      </div>
      <Flex flexDirection="column" gap={8} className='form-section-body'>
        {children}
      </Flex>
    </div>
  );
}

