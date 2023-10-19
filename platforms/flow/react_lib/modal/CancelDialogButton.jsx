// @flow

import * as React from "react";

import Button from "latitude/button/Button";

import {DialogContext} from "./Dialog";
import { FormContext } from "../form/FormProvider";

type Props = {|
  +label?: string,
|};
export default function CancelDialogButton({
  label="Cancel",
}: Props): React.Node {
  const dialogContext = React.useContext(DialogContext);

  return (
    <Button 
      label={label}
      onClick={() => dialogContext.setIsOpen(false)}
      kind="hollow"
      intent="none"
    />
  );
}
