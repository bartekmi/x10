// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import MultiEntityQueryRenderer from "react_lib/relay/MultiEntityQueryRenderer";

import BuildingsPage from "./BuildingsPage";
import { type Building, createDefaultBuilding } from "entities/Building";

export default function BuildingEditPageInterface(props: {}): React.Node {
  return (
      <MultiEntityQueryRenderer
        createComponentFunc={(buildings) => <BuildingsPage buildings={buildings}/>}
        query={query}
      />
  );
}

const query = graphql`
  query BuildingsPageInterfaceQuery {
    entities: buildings {
      nodes {
        dbid
        name
        description
        petPolicy
        physicalAddress {
          city
        }
      }
    }
  }
`;