import { gql } from '@apollo/client';
import * as React from 'react';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultTenant, type Tenant } from 'x10_generated/small/entities/Tenant';
import { TENANTFORM_TENANT_FRAGMENT, TenantFormStateful } from 'x10_generated/small/ui/TenantForm';



type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
};
export default function TenantFormInterface(props: Props): React.JSX.Element {
  return (
    <EntityQueryRenderer<Tenant>
      id={ props.id }
      match={ props.match }
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

