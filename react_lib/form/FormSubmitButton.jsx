// @flow

import * as React from "react";
import { useHistory } from "react-router-dom";

import Text from "latitude/Text";
import Group from "latitude/Group";
import Button from "latitude/button/Button";
import ToastActions from "latitude/toast/ToastActions";

import {FormContext} from "./FormProvider";
import { string } from "prop-types";

type Props = {|
  +label?: string,
  +action?: {
    +successUrl?: string,
  },
  +onClick: () => mixed,
|};
export default function FormSubmitButton(props: Props): React.Node {
  const history = useHistory();
  const formContext = React.useContext(FormContext);
  const { label = "Save", onClick, action: {successUrl} = {} } = props

  const handleOnClick = () => {
    onClick();

    // TODO: Obviously, onClick() should return a promise, and depending on its state
    // we should show an appropriate Toast message and only navigate on success
    ToastActions.show(
      {
        intent: "success",
        message: "Saved successfully!",
      },
      3000
    );
  
    if (successUrl != null) {
      history.push(successUrl);
    }
  }

  return (
    <Button 
      disabled={formContext.errors.length > 0}
      intent="basic" kind="solid"
      onClick={handleOnClick}
    >
      {label}
    </Button>
  );
}

