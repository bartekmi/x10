// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';

import environment from 'environment';
import { createDefaultBuilding } from 'small/entities/Building';
import BuildingForm, { BuildingFormStateful } from 'small/ui/BuildingForm';



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
      createComponentFunc={ (building) => <BuildingForm building={ building }/> }
      createComponentFuncNew={ () => <BuildingFormStateful building={ createDefaultBuilding() }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query BuildingFormInterfaceQuery($id: String!) {
    entity: building(id: $id) {
      ...BuildingForm_building
    }
  }
`;
