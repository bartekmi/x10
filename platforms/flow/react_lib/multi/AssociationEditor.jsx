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
  +order?: "sameAsDefined" | "alphabetic",
|};
export default function AssociationEditor({id, query, toString, onChange, isNullable, order = "alphabetic"}: Props): React.Node {

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
          let options = data.map(x => ({ 
                value: x.id, 
                label: toString(x)
              }));

          if (order === "alphabetic") {
            options = options.sort((a, b) => a.label.localeCompare(b.label));
          }

          return (
            <SearchableSelectInput
              value={id || null}
              options={options}
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