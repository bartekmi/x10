import { gql } from '@apollo/client';
import * as React from 'react';

import MultiEntityQueryRenderer from 'react_lib/client_apollo/MultiEntityQueryRenderer';

import Moves from 'x10_generated/small/ui/Moves';



export default function MovesInterface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (moves) => <Moves moves={ moves }/> }
      query={ query }
    />
  );
}

const query = gql`
  query MovesInterfaceQuery {
    entities: moves {
      ...Moves_Moves
    }
  }
`;

