// @flow

import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import NotificationModal from "latitude/modal/NotificationModal";

export type DialogContextType = {|
  +setIsOpen: (value: bool) => void,
|};

export const DialogContext: React.Context<DialogContextType> = React.createContext({
  setIsOpen: (value) => {},
});

type Props = {|
  +openButton: any,
  +title: ?string,        // Allow null primarily to appease Flow
  +children: React.Node, // The body of the Dialog
|};
export default function Dialog(props: Props): React.Node {
  const { openButton, title, children } = props;
  const [isOpen, setIsOpen] = React.useState(false);

  if (!isOpen) {
    return React.cloneElement(openButton, 
      {
        onClick: () => setIsOpen(true)
      });
  }

  return (
    <DialogContext.Provider value={ {setIsOpen} }>
      <NotificationModal
        onRequestClose={() => setIsOpen(false)}
        title={title}
        buttons={[]}
      >
        {children}
      </NotificationModal>
    </DialogContext.Provider>
  );
}
