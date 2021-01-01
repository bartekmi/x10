// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';
import { graphql, QueryRenderer } from "react-relay";

import environment from "../environment";

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
  const stringId = props.match.params.id;
  if (stringId == null) {
    return <BuildingEditForm building={createDefaultBuilding()} />
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
          return (
            <BuildingEditForm
              building={props.building}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

const query = graphql`
  query BuildingEditPageInterfaceQuery($id: Int!) {
    building(id: $id) {
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
