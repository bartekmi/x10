import { gql } from '@apollo/client';
import * as React from 'react';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultBuilding, type Building } from 'x10_generated/small/entities/Building';
import BuildingView from 'x10_generated/small/ui/BuildingView';



type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
};
export default function BuildingViewInterface(props: Props): React.JSX.Element {
  return (
    <EntityQueryRenderer<Building>
      id={ props.id }
      match={ props.match }
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
`;

