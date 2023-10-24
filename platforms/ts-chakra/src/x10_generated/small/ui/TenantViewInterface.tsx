import { gql } from '@apollo/client';
import * as React from 'react';
import { useParams } from 'react-router-dom';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultTenant, type Tenant } from 'x10_generated/small/entities/Tenant';
import TenantView, { TENANTVIEW_TENANT_FRAGMENT } from 'x10_generated/small/ui/TenantView';



export default function TenantViewInterface(): React.JSX.Element {
  const params = useParams()
  return (
    <EntityQueryRenderer<Tenant>
      id={ params.id }
      createComponentFunc={ (tenant) => <TenantView tenant={ tenant }/> }
      createEntityFunc={ createDefaultTenant }
      query={ query }
    />
  );
}

const query = gql`
  query TenantViewInterfaceQuery($id: String!) {
    entity: tenant(id: $id) {
      ...TenantView_Tenant
    }
  }
  ${ TENANTVIEW_TENANT_FRAGMENT }
`;
