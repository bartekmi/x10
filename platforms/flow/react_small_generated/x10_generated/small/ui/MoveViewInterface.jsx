// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';

import environment from 'environment';
import { createDefaultMove } from 'small/entities/Move';
import MoveView from 'small/ui/MoveView';



type Props = { 
  +id?: string,      // When invoked from another Component
  +match?: {         // When invoked via Route
    +params: { 
      +id: string
    }
  }
};
export default function MoveViewInterface(props: Props): React.Node {
  return (
    <EntityQueryRenderer
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (move) => <MoveView move={ move }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query MoveViewInterfaceQuery($id: String!) {
    entity: move(id: $id) {
      ...MoveView_move
    }
  }
`;

