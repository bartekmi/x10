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
  +createComponentFunc: (T) => React.Node,
  +createComponentFuncNew?: () => React.Node,
  +gqlToInernalConvertFunc?: (any) => T,
|};
export default function BasicQueryRenderer<T>(props: Props<T>): React.Node {
  const {match, query, createComponentFunc, createComponentFuncNew, gqlToInernalConvertFunc} = props;
  const id = props.match.params.id;
  if (id == null) {
    if (createComponentFuncNew)
      return createComponentFuncNew();
    return <div>Id could not be extracted from Props</div>
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