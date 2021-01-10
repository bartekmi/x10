// @flow

import { commitMutation } from "react-relay";
import { type GraphQLTaggedNode } from "relay-runtime/query/GraphQLTag";
import environment from "./environment";

export default function basicCommitMutation<V: {...}>(mutation: GraphQLTaggedNode, variables: V) {
  commitMutation(
    environment,
    {
      mutation,
      variables,
    }
  );
}