// @flow

import { any } from "prop-types";
import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import Loader from "latitude/Loader";

import environment from "./environment";

type Props<T> = {|
  +id?: string,       // When invoked from another Component
  +match?: {          // When invoked via Route
    +params: {
      +id: string
    }
  },
  +query: any,
  +createComponentFunc: (T) => React.Node,
  +createComponentFuncNew?: () => React.Node,
|};
export default function BasicQueryRenderer<T>(props: Props<T>): React.Node {
  const {id, match, query, createComponentFunc, createComponentFuncNew} = props;
  const extractedId = id || props.match?.params?.id;
  if (extractedId == null) {
    if (createComponentFuncNew)
      return createComponentFuncNew();
    return <div>Id could not be extracted from Props</div>
  }

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{
        id: extractedId
      }}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          return createComponentFunc(props.entity);
        }
        return <Loader loaded={false}/>;
      }}
    />
  );
}