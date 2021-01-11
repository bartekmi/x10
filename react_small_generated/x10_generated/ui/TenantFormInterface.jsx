// This file was auto-generated on 01/10/2021 22:40:08. Do not modify by hand.
// @flow

import { createDefaultTenant, type Tenant } from 'entities/Tenant';
import environment from 'environment';
import * as React from 'react';
import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';
import { graphql, QueryRenderer } from 'react-relay';
import TenantForm from 'ui/TenantForm';


type TenantProps = {|
  +tenant: Tenant,
|};
function TenantFormWrapper(props: TenantProps): React.Node {
  const [editedTenant, setEditedTenant] = React.useState(props.tenant);
  return <TenantForm
    tenant={ editedTenant }
    onChange={ setEditedTenant }
  />
}

type Props = { 
  +match: { 
    +params: { 
      +id: string
    }
  }
};
export default function TenantFormInterface(props: Props): React.Node { 
  return (
    <EntityQueryRenderer
      match={ props.match }
      createDefaultFunc={ createDefaultTenant }
      createComponentFunc={ (tenant) => <TenantFormWrapper tenant={ tenant }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query TenantFormInterfaceQuery($id: Int!) {
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

