import * as React from "react";

import {
  Button,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
} from '@chakra-ui/react'

type Props = {
  readonly openButton: any,
  readonly title: string,
  readonly children: React.JSX.Element, // The body of the Dialog
};
export default function Dialog(props: Props): React.JSX.Element {
  const { openButton, title, children } = props;
  const [isOpen, setIsOpen] = React.useState(false);

  if (!isOpen) {
    return React.cloneElement(openButton, 
      {
        onClick: () => setIsOpen(true)
      });
  }

  return (
    <>
      <ModalOverlay/>
      <ModalContent>
        <ModalHeader>{title}</ModalHeader>
        <ModalBody>
          {children}
        </ModalBody>
        <ModalFooter>
          <Button colorScheme='blue' mr={3} onClick={() => setIsOpen(false)}>
              Close
          </Button>          
        </ModalFooter>
      </ModalContent>
    </>
  );
}
