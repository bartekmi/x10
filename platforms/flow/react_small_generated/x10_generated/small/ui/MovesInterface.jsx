// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';

import environment from 'environment';
import Moves from 'small/ui/Moves';



export default function MovesInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (moves) => <Moves moves={ moves }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query MovesInterfaceQuery {
    entities: moves {
      ...Moves_moves
    }
  }
`;

