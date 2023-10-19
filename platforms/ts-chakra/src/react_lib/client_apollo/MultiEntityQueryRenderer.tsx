import * as React from "react";
import { useQuery } from '@apollo/client';
import { Spinner } from "@chakra-ui/react";

type Props<T> = {
  readonly query: any,  // TODO: Get an actual type for this
  readonly createComponentFunc: (entities: T[]) => React.JSX.Element,
};
export default function MultiEntityQueryRenderer<T>(props: Props<T>): React.JSX.Element {
  const {query, createComponentFunc} = props;

  const { loading, error, data } = useQuery(
    query, { variables: {} }
  );

  if (loading)
    return <Spinner/>
  if (error)
    return <div>`Error! ${error.message}`</div>;

  return createComponentFunc(data.entities);
}