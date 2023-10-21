import { gql } from '@apollo/client';
import * as React from 'react';

import MultiEntityQueryRenderer from 'react_lib/client_apollo/MultiEntityQueryRenderer';

import { type Building } from 'x10_generated/small/entities/Building';
import Buildings from 'x10_generated/small/ui/Buildings';



export default function BuildingsInterface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer<Building>
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

