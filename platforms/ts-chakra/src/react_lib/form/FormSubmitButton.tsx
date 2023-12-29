import * as React from "react";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";

import { useToast, AlertStatus, Spinner } from '@chakra-ui/react'

import Button from "../chakra_wrappers/Button";
import {FormContext} from "./FormProvider";


type Props = {
  readonly label?: string,
  readonly mutation: any,
  readonly variables: any,
  readonly successMessage?: string,
  readonly errorMessage?: string,
  readonly url?: string,
};
export default function FormSubmitButton(props: Props): React.JSX.Element {
  const { 
    label="Save Changes", 
    mutation, 
    variables, 
    successMessage = "Your data has been saved.", 
    errorMessage = "Something went wrong.", 
    url,
  } = props

  const navigation = useNavigate();
  const formContext = React.useContext(FormContext);
  const toast = useToast()

  const [mutateFunc, { loading }] = useMutation(mutation, {
    onCompleted: () => {
      showMessage("Success", successMessage, "success");
      if (url) {
        navigation(url);
      }
    },
    onError: () => showMessage("Error", errorMessage, "error")
  });

  function showMessage(title: string, description: string, status: AlertStatus) {
    toast({
        title,
        description,
        status,
        duration: 5000,
        isClosable: true,
      });
  }

  const handleOnClick = () => {
    mutateFunc({ 
      // We must strip out "__typename" to prevent edited entities
      // with owned associations from being rejected by the GQL server.
      // Since the incoming entity has { id: <id>, __typename: <typename> },
      // if this is echoed back to the server which expects IdWrapper
      // this blows up.
      variables: removeTypename(variables) 
    });
  }

  if (loading)
    return <Button disabled={true}><Spinner/></Button>

  const hasErrors = formContext.errors.filter(x => x).length > 0;
  return (
    <Button 
      disabled={hasErrors}
      onClick={handleOnClick}
      intent="save-changes"
    >
      {label}
    </Button>
  );
}

function removeTypename<T extends Array<any> | object>(obj: T): T {
  if (Array.isArray(obj)) {
      return obj.map(value => removeTypename(value)) as T;
  } else if (obj !== null && typeof obj === 'object') {
      const newObj: { [key: string]: any } = {};
      for (const key in obj as object) {
          if (key !== '__typename') {
              newObj[key] = removeTypename((obj as { [key: string]: any })[key]);
          }
      }
      return newObj as T;
  }
  return obj;
}
