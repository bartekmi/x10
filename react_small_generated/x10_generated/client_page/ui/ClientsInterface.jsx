// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';

import Clients from 'client_page/ui/Clients';
import environment from 'environment';



export default function ClientsInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (clients) => <Clients clients={ clients }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query ClientsInterfaceQuery {
    entities: clients {
      ...Clients_clients
    }
  }
`;

