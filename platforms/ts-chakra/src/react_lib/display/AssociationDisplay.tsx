import * as React from "react";
import "./TextDisplay";
import TextDisplay from "./TextDisplay";

type Props = {
  readonly value?: any,
  readonly toStringRepresentation: (appContext: any, item: any) => string | undefined,
}

export default function AssociationDisplay(props: Props): React.JSX.Element {
  const { value, toStringRepresentation } = props;

  const display = toStringRepresentation(null, value);
  return <TextDisplay value={display}/>;
}