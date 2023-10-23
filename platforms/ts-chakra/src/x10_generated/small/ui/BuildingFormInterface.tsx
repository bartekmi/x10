import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from 'react-router-dom';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultBuilding, type Building } from 'x10_generated/small/entities/Building';
import { BUILDINGFORM_BUILDING_FRAGMENT, BuildingFormStateful } from 'x10_generated/small/ui/BuildingForm';



export default function BuildingFormInterface(): React.JSX.Element {
  const params = useParams()
  return (
    <EntityQueryRenderer<Building>
      id={ params.id }
      createComponentFunc={ (building) => <BuildingFormStateful building={ building }/> }
      createEntityFunc={ createDefaultBuilding }
      query={ query }
    />
  );
}

const query = gql`
  query BuildingFormInterfaceQuery($id: String!) {
    entity: building(id: $id) {
      ...BuildingForm_Building
    }
  }
  ${ BUILDINGFORM_BUILDING_FRAGMENT }
`;

