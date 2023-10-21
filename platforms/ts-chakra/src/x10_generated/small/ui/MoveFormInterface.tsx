import { gql } from '@apollo/client';
import * as React from 'react';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultMove } from 'x10_generated/small/entities/Move';
import { MoveFormStateful } from 'x10_generated/small/ui/MoveForm';

import { MoveForm_MoveFragment } from '__generated__/graphql';



type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
};
export default function MoveFormInterface(props: Props): React.JSX.Element {
  return (
    <EntityQueryRenderer<MoveForm_MoveFragment>
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (move) => <MoveFormStateful move={ move }/> }
      createEntityFunc={ createDefaultMove }
      query={ query }
    />
  );
}

const query = gql`
  query MoveFormInterfaceQuery($id: String!) {
    entity: move(id: $id) {
      ...MoveForm_move
    }
  }
`;

