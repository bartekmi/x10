// TEAM: compliance
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';

import { createDefaultHit } from 'dps/entities/Hit';
import WorkspaceTabs from 'dps/ui/WorkspaceTabs';
import environment from 'environment';



type Props = { 
  +id?: string,      // When invoked from another Component
  +match?: {         // When invoked via Route
    +params: { 
      +id: string
    }
  }
};
export default function WorkspaceTabsInterface(props: Props): React.Node {
  return (
    <EntityQueryRenderer
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (hit) => <WorkspaceTabs hit={ hit }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query WorkspaceTabsInterfaceQuery($id: String!) {
    entity: hit(id: $id) {
      ...WorkspaceTabs_hit
    }
  }
`;

