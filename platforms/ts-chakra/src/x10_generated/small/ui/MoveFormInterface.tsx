import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from 'react-router-dom';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultMove, type Move } from 'x10_generated/small/entities/Move';
import { MOVEFORM_MOVE_FRAGMENT, MoveFormStateful } from 'x10_generated/small/ui/MoveForm';



export default function MoveFormInterface(): React.JSX.Element {
  const params = useParams()
  return (
    <EntityQueryRenderer<Move>
      id={ params.id }
      createComponentFunc={ (move) => <MoveFormStateful move={ move }/> }
      createEntityFunc={ createDefaultMove }
      query={ query }
    />
  );
}

const query = gql`
  query MoveFormInterfaceQuery($id: String!) {
    entity: move(id: $id) {
      ...MoveForm_Move
    }
  }
  ${ MOVEFORM_MOVE_FRAGMENT }
`;

