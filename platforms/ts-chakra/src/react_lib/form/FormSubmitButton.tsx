import * as React from "react";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@apollo/react-hooks";

import { Button, useToast, AlertStatus, Spinner } from '@chakra-ui/react'

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
    label, 
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
    mutateFunc({ variables });
  }

  if (loading)
    return <Button disabled={true}><Spinner/></Button>

  const hasErrors = formContext.errors.filter(x => x).length > 0;
  return (
    <Button 
      isDisabled={hasErrors}
      onClick={handleOnClick}
    >
      {label}
    </Button>
  );
}
