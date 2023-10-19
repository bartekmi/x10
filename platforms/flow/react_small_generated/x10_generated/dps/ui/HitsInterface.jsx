// TEAM: compliance
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';

import Hits from 'dps/ui/Hits';
import environment from 'environment';



export default function HitsInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (hits) => <Hits hits={ hits }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query HitsInterfaceQuery {
    entities: hits {
      ...Hits_hits
    }
  }
`;

