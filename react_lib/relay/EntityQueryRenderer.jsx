// @flow

import { any } from "prop-types";
import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import Loader from "latitude/Loader";

import environment from "./environment";

type Props<T> = {|
  +match: {
    +params: {
      +id: string
    }
  },
  +query: any,
  +createDefaultFunc: () => T,
  +createComponentFunc: (T) => React.Node,
  +gqlToInernalConvertFunc?: (any) => T,
|};
export default function BasicQueryRenderer<T>(props: Props<T>): React.Node {
  const {match, createDefaultFunc, query, createComponentFunc, gqlToInernalConvertFunc} = props;
  const stringId = props.match.params.id;
  if (stringId == null) {
    const defaultEntity = createDefaultFunc();
    return createComponentFunc(defaultEntity);
  }

  const id: number = parseInt(stringId);
  if (isNaN(id)) {
    throw new Error("Not a number: " + stringId);
  }

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{
        id
      }}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          const converted = gqlToInernalConvertFunc ? gqlToInernalConvertFunc(props.entity) : props.entity;
          return createComponentFunc(converted);
        }
        return <Loader loaded={false}/>;
      }}
    />
  );
}