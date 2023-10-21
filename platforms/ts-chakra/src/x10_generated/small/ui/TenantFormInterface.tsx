import { gql } from '@apollo/client';
import * as React from 'react';

import EntityQueryRenderer from 'react_lib/client_apollo/EntityQueryRenderer';

import { createDefaultTenant } from 'x10_generated/small/entities/Tenant';
import { TenantFormStateful } from 'x10_generated/small/ui/TenantForm';

import { TenantForm_TenantFragment } from '__generated__/graphql';



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
    <EntityQueryRenderer<TenantForm_TenantFragment>
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
      ...TenantForm_tenant
    }
  }
`;

