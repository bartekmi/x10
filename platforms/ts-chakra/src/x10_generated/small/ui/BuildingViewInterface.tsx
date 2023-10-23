import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from "react-router-dom";

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultBuilding, type Building } from 'x10_generated/small/entities/Building';
import BuildingView, { BUILDINGVIEW_BUILDING_FRAGMENT } from 'x10_generated/small/ui/BuildingView';

export default function BuildingViewInterface(): React.JSX.Element {
  const params = useParams();
  return (
    <EntityQueryRenderer<Building>
      id={ params.id }
      createComponentFunc={ (building) => <BuildingView building={ building }/> }
      createEntityFunc={ createDefaultBuilding }
      query={ query }
    />
  );
}

const query = gql`
  query BuildingViewInterfaceQuery($id: String!) {
    entity: building(id: $id) {
      ...BuildingView_Building
    }
  }
  ${ BUILDINGVIEW_BUILDING_FRAGMENT }
`;

