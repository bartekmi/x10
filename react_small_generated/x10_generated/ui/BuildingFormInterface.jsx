// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { createDefaultAddress } from 'entities/Address';
import { createDefaultBuilding, type Building } from 'entities/Building';
import environment from 'environment';
import * as React from 'react';
import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';
import { graphql, QueryRenderer } from 'react-relay';
import BuildingForm from 'ui/BuildingForm';


type BuildingProps = {|
  +building: Building,
|};
function BuildingFormWrapper(props: BuildingProps): React.Node {
  const [editedBuilding, setEditedBuilding] = React.useState(props.building);
  return <BuildingForm
    building={ editedBuilding }
    onChange={ setEditedBuilding }
  />
}

type Props = { 
  +match: { 
    +params: { 
      +id: string
    }
  }
};
export default function BuildingFormInterface(props: Props): React.Node {
  return (
    <EntityQueryRenderer
      match={ props.match }
      createDefaultFunc={ createDefaultBuilding }
      createComponentFunc={ (building) => <BuildingFormWrapper building={ building }/> }
      gqlToInernalConvertFunc={ gqlToInernalConvert }
      query={ query }
    />
  );
}

function gqlToInernalConvert(data: any): Building {
  return {
    ...data,
    mailingAddress: data.mailingAddress || createDefaultAddress(),
  };
}

const query = graphql`
  query BuildingFormInterfaceQuery($id: Int!) {
    entity: building(id: $id) {
      id
      dbid
      moniker
      name
      description
      dateOfOccupancy
      mailboxType
      petPolicy
      mailingAddressSameAsPhysical
      units {
        id
        dbid
        number
        squareFeet
        numberOfBedrooms
        numberOfBathrooms
        hasBalcony
      }
      physicalAddress {
        id
        dbid
        unitNumber
        theAddress
        city
        stateOrProvince
        zip
      }
      mailingAddress {
        id
        dbid
        unitNumber
        theAddress
        city
        stateOrProvince
        zip
      }
    }
  }
`;
