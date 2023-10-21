import { gql } from '@apollo/client';
import * as React from 'react';

import MultiEntityQueryRenderer from 'react_lib/client_apollo/MultiEntityQueryRenderer';

import Tenants from 'x10_generated/small/ui/Tenants';



export default function TenantsInterface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (tenants) => <Tenants tenants={ tenants }/> }
      query={ query }
    />
  );
}

const query = gql`
  query TenantsInterfaceQuery {
    entities: tenants {
      ...Tenants_Tenants
    }
  }
`;

