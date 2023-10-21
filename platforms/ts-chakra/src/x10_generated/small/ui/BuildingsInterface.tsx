import { gql } from '@apollo/client';
import * as React from 'react';

import MultiEntityQueryRenderer from 'react_lib/client_apollo/MultiEntityQueryRenderer';



export default function BuildingsInterface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (buildings) => <Buildings buildings={ buildings }/> }
      query={ query }
    />
  );
}

const query = gql`
  query BuildingsInterfaceQuery {
    entities: buildings {
      ...Buildings_Buildings
    }
  }
`;

