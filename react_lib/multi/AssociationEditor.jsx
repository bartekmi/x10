// @flow

import { any } from "prop-types";
import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import Loader from "latitude/Loader";
import SearchableSelectInput from "latitude/select/SearchableSelectInput";

import environment from "../relay/environment";

type Props = {|
  +id: ?string,  // This should be the relay id of an object
  +query: any,
  +toString: any => string,
  +onChange: (?string) => void,
  +isNullable: boolean,
|};
export default function AssociationEditor(props: Props): React.Node {
  const {id, query, toString, onChange, isNullable} = props;

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{
      }}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          const data = props.entities;
          return (
            <SearchableSelectInput
              value={id || null}
              options={data.map(x => ({ 
                value: x.id, 
                label: toString(x)
              }))}
              onChange={ value => onChange(value) }
              isNullable={ isNullable }
            />
          )
        }
        return <Loader loaded={false}/>;
      }}
    />
  );
}