// TEAM: compliance
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';

import { createDefaultHit } from 'dps/entities/Hit';
import SlideoutPanel from 'dps/ui/SlideoutPanel';
import environment from 'environment';



type Props = { 
  +id?: string,      // When invoked from another Component
  +match?: {         // When invoked via Route
    +params: { 
      +id: string
    }
  }
};
export default function SlideoutPanelInterface(props: Props): React.Node {
  return (
    <EntityQueryRenderer
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (hit) => <SlideoutPanel hit={ hit }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query SlideoutPanelInterfaceQuery($id: String!) {
    entity: hit(id: $id) {
      ...SlideoutPanel_hit
    }
  }
`;

