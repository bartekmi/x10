// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import EntityQueryRenderer from "react_lib/relay/EntityQueryRenderer";

import BuildingEditPage from "./BuildingEditPage";
import { type Building, createDefaultBuilding } from "entities/Building";

type BuildingEditFormProps = {|
  +building: Building,
|};
function BuildingEditForm(props: BuildingEditFormProps): React.Node {
  const [editedBuilding, setEditedBuilding] = React.useState(props.building);
  return <BuildingEditPage 
    building={editedBuilding} 
    onChange={setEditedBuilding}
  />
}

type Props = {
  +match: {
    +params: {
      +id: string
    }
  }
};
export default function BuildingEditPageInterface(props: Props): React.Node {
  return (
      <EntityQueryRenderer
        match={props.match}
        createDefaultFunc={createDefaultBuilding}
        createComponentFunc={(building) => <BuildingEditForm building={building}/>}
        query={query}
      />
  );
}

const query = graphql`
  query BuildingEditPageInterfaceQuery($id: Int!) {
    entity: building(id: $id) {
      id
      dbid
      name
      description
      dateOfOccupancy
      mailboxType
      mailingAddress {
        id
        city
        dbid
        stateOrProvince
        theAddress
        unitNumber
        zip
      }
      mailingAddressSameAsPhysical
      petPolicy
      physicalAddress {
        id
        city
        dbid
        stateOrProvince
        theAddress
        unitNumber
        zip
      }
      units {
        id
        dbid
        hasBalcony
        number
        numberOfBathrooms
        numberOfBedrooms
        squareFeet
      }
    }
  }
`;
