// Auto-generated by x10 - do not edit
import { gql } from '@apollo/client';
import { Heading } from '@chakra-ui/react';
import * as React from 'react';

import TextDisplay from 'react_lib/display/TextDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import FormSection from 'react_lib/form/FormSection';
import StyleControl from 'react_lib/StyleControl';

import { AppContext } from 'SmallAppContext';
import { type Tenant } from 'x10_generated/small/entities/Tenant';



type Props = {
  readonly tenant?: Tenant,
};
export default function TenantView(props: Props): React.JSX.Element {
  const { tenant } = props;
  const appContext = React.useContext(AppContext);

  return (
    <DisplayForm>
      <Heading
        as='h1'
        size='2xl'
        children='Tenant Details'
      />
      <FormSection
        label='Tenant Info'
      >
        <DisplayField
          label='Name'
        >
          <TextDisplay
            value={ tenant?.name }
          />
        </DisplayField>
        <DisplayField
          label='Phone'
        >
          <TextDisplay
            value={ tenant?.phone }
          />
        </DisplayField>
        <DisplayField
          label='Email'
        >
          <TextDisplay
            value={ tenant?.email }
          />
        </DisplayField>
      </FormSection>
      <FormSection
        label='Permanent Mailing Address'
      >
        <DisplayField
          label='The Address'
        >
          <TextDisplay
            value={ tenant?.permanentMailingAddress?.theAddress }
          />
        </DisplayField>
        <StyleControl
          maxWidth={ 400 }
        >
          <DisplayField
            label='City'
          >
            <TextDisplay
              value={ tenant?.permanentMailingAddress?.city }
            />
          </DisplayField>
        </StyleControl>
        <StyleControl
          maxWidth={ 250 }
        >
          <DisplayField
            label='State Or Province'
          >
            <TextDisplay
              value={ tenant?.permanentMailingAddress?.stateOrProvince }
            />
          </DisplayField>
        </StyleControl>
        <StyleControl
          maxWidth={ 150 }
        >
          <DisplayField
            label='Zip or Postal Code'
          >
            <TextDisplay
              value={ tenant?.permanentMailingAddress?.zip }
            />
          </DisplayField>
        </StyleControl>
      </FormSection>
    </DisplayForm>
  );
}

export const TENANTVIEW_TENANT_FRAGMENT = gql`
  fragment TenantView_Tenant on Tenant {
    id
    email
    name
    permanentMailingAddress {
      id
      city
      stateOrProvince
      theAddress
      zip
    }
    phone
  }
`

