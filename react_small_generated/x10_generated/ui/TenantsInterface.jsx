// This file was auto-generated on 01/10/2021 23:35:27. Do not modify by hand.
// @flow

import { type Tenant } from 'entities/Tenant';
import environment from 'environment';
import * as React from 'react';
import MultiEntityQueryRenderer from 'react_lib/relay/MultiEntityQueryRenderer';
import { graphql, QueryRenderer } from 'react-relay';
import Tenants from 'ui/Tenants';


export default function TenantsInterface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ (tenants) => <Tenants tenants={ tenants }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query TenantsInterfaceQuery($id: Int!) {
    entity: tenant(id: $id) {
      id
      dbid
      name
      phone
      email
      permanentMailingAddress {
        id
        dbid
        unitNumber
        theAddress
        city
        stateOrProvince
        zip
      }
    }
  }
`;

