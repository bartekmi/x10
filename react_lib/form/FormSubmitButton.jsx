// @flow

import * as React from "react";

import Text from "latitude/Text";
import Group from "latitude/Group";
import Button from "latitude/button/Button";

import {FormContext} from "./FormProvider";

type Props = {|
  +label?: string,
  +onClick: () => mixed,
|};
export default function FormSubmitButton(props: Props): React.Node {
  const formContext = React.useContext(FormContext);
  const { label = "Save", onClick } = props

  return (
    <Button 
      disabled={formContext.errors.length > 0}
      intent="basic" kind="solid"
      onClick={onClick}
    >
      {label}
    </Button>
  );
}