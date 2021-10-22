// @flow

import * as React from "react";
import { useHistory } from "react-router-dom";
import { type GraphQLTaggedNode } from "relay-runtime/query/GraphQLTag";
import { commitMutation } from "react-relay";

import Button from "latitude/button/Button";
import ToastActions from "latitude/toast/ToastActions";

import {FormContext} from "./FormProvider";
import { string } from "prop-types";
import environment from "../relay/environment";


type Props = {|
  +label?: string,
  +mutation: GraphQLTaggedNode,
  +variables: any,
  +successMessage?: string,
  +errorMessage?: string,
  +url?: string,
|};
export default function FormSubmitButton(props: Props): React.Node {
  const history = useHistory();
  const formContext = React.useContext(FormContext);

  const { 
    label, 
    mutation, 
    variables, 
    successMessage = "Saved successfully.", 
    errorMessage = "Failed!!!", 
    url 
  } = props

  const handleOnClick = () => {
    commitMutation(
      environment,
      {
        mutation,
        variables,
        onCompleted: () => {
          ShowMessage(successMessage, "success");
          if (url) {
            history.push(url);
          }
        },
        onError: () => ShowMessage(errorMessage, "danger"),
      }
    );
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

function ShowMessage(message: string, intent: string) {
  ToastActions.show(
    {
      intent,
      message
    },
    3000
  );

}