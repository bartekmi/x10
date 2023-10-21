import { gql } from '@apollo/client';
import * as React from 'react';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultMove } from 'x10_generated/small/entities/Move';
import MoveView from 'x10_generated/small/ui/MoveView';

import { MoveView_MoveFragment } from '__generated__/graphql';



type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
};
export default function MoveViewInterface(props: Props): React.JSX.Element {
  return (
    <EntityQueryRenderer<MoveView_MoveFragment>
      id={ props.id }
      match={ props.match }
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
`;

