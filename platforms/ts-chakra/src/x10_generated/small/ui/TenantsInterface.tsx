import { gql } from '@apollo/client';
import * as React from 'react';

import MultiEntityQueryRenderer from 'react_lib/client_apollo/MultiEntityQueryRenderer';

import { type Tenant } from 'x10_generated/small/entities/Tenant';
import Tenants, { TENANTS_TENANTS_FRAGMENT } from 'x10_generated/small/ui/Tenants';



export default function TenantsInterface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer<Tenant>
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
  ${ TENANTS_TENANTS_FRAGMENT }
`;

