// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';

import environment from 'environment';
import Tenants from 'small/ui/Tenants';



export default function TenantsInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (tenants) => <Tenants tenants={ tenants }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query TenantsInterfaceQuery {
    entities: tenants {
      ...Tenants_tenants
    }
  }
`;

