// @flow

import { any } from "prop-types";
import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { type GraphQLTaggedNode } from "relay-runtime/query/GraphQLTag";

import Loader from "latitude/Loader";

import environment from "./environment";

type Props<T> = {|
  +query: GraphQLTaggedNode,
  +createComponentFunc: ($ReadOnlyArray<T>) => React.Node,
|};
export default function MultiEntityQueryRenderer<T>(props: Props<T>): React.Node {
  const {query, createComponentFunc} = props;

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{}}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          return createComponentFunc(props.entities.nodes);
        }
        return <Loader loaded={false}/>;
      }}
    />
  );
}