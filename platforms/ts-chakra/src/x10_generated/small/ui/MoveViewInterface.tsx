// Auto-generated by x10 - do not edit
import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from 'react-router-dom';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultMove, type Move } from 'x10_generated/small/entities/Move';
import { BUILDINGVIEW_BUILDING_FRAGMENT } from 'x10_generated/small/ui/BuildingView';
import MoveView, { MOVEVIEW_MOVE_FRAGMENT } from 'x10_generated/small/ui/MoveView';



export default function MoveViewInterface(): React.JSX.Element {
  const params = useParams()
  return (
    <EntityQueryRenderer<Move>
      id={ params.id }
      createComponentFunc={ (move) => <MoveView move={ move }/> }
      createEntityFunc={ createDefaultMove }
      query={ query }
    />
  );
}

const query = gql`
  query MoveViewInterfaceQuery($id: String!) {
    entity: move(id: $id) {
      ...MoveView_Move
    }
  }
  ${ BUILDINGVIEW_BUILDING_FRAGMENT }
  ${ MOVEVIEW_MOVE_FRAGMENT }
`;

