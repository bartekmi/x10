// @flow

import * as React from "react";
import { StyleSheet, css } from "aphrodite";

import NotificationModal from "latitude/modal/NotificationModal";

type Props = {|
  +openButton: any,
  +title: string,
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
    <NotificationModal
      onRequestClose={() => setIsOpen(false)}
      title={title}
      buttons={[]}
    >
      {children}
    </NotificationModal>

  );
}
