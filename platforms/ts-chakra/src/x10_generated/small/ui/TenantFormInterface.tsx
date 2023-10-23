import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from 'react-router-dom';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultTenant, type Tenant } from 'x10_generated/small/entities/Tenant';
import { TENANTFORM_TENANT_FRAGMENT, TenantFormStateful } from 'x10_generated/small/ui/TenantForm';



export default function TenantFormInterface(): React.JSX.Element {
  const params = useParams()
  return (
    <EntityQueryRenderer<Tenant>
      id={ params.id }
      createComponentFunc={ (tenant) => <TenantFormStateful tenant={ tenant }/> }
      createEntityFunc={ createDefaultTenant }
      query={ query }
    />
  );
}

const query = gql`
  query TenantFormInterfaceQuery($id: String!) {
    entity: tenant(id: $id) {
      ...TenantForm_Tenant
    }
  }
  ${ TENANTFORM_TENANT_FRAGMENT }
`;

