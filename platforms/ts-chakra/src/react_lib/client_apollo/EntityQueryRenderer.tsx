import * as React from "react";
import { useQuery } from '@apollo/client';
import { Spinner } from "@chakra-ui/react";

type Props<T> = {
  readonly id?: string,
  readonly query: any,  // TODO: Get an actual type for this
  readonly createComponentFunc: (entity: T) => React.JSX.Element,
  readonly createEntityFunc: () => T,
};
export default function EntityQueryRenderer<T>(props: Props<T>): React.JSX.Element {
  const {id, query, createComponentFunc, createEntityFunc} = props;

  if (id == null) {
    return createComponentFunc(createEntityFunc());
  }

  return <EntityQueryRendererWithQuery 
    id={id}
    query={query}
    createComponentFunc={createComponentFunc}
  />
}

type WithQueryProps<T> = {
  readonly id: string,
  readonly query: any,  // TODO: Get an actual type for this
  readonly createComponentFunc: (entity: T) => React.JSX.Element,
};
function EntityQueryRendererWithQuery<T>(props: WithQueryProps<T>): React.JSX.Element {
  const {id, query, createComponentFunc} = props;
  const { loading, error, data } = useQuery(
    query,
    { variables: { id: id } }
  );

  if (loading)
    return <Spinner/>
  if (error)
    return <div>`Error! ${error.message}`</div>;

  return createComponentFunc(data.entity);
}