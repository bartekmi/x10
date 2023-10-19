import * as React from 'react';
import { gql } from '@apollo/client';

import EntityQueryRenderer from '../../../react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultBuilding } from '../entities/Building';
import { BuildingFormStateful } from './BuildingForm';
import { BuildingForm_BuildingFragment } from '../../../__generated__/graphql';



type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
};
export default function BuildingFormInterface(props: Props): React.JSX.Element {
  return (
    <EntityQueryRenderer<BuildingForm_BuildingFragment>
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (data) => <BuildingFormStateful building={ data }/> }
      createEntityFunc={ createDefaultBuilding }
      query={ query }
    />
  );
}

const query = gql`
  query BuildingFormInterfaceQuery($id: String!) {
    entity: building(id: $id) {
      ...BuildingForm_building
    }
  }
`;

