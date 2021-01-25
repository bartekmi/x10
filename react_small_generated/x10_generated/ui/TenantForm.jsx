// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { tenantCalculateErrors, type Tenant } from 'entities/Tenant';
import Group from 'latitude/Group';
import Text from 'latitude/Text';
import TextInput from 'latitude/TextInput';
import * as React from 'react';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';
import { createFragmentContainer, graphql } from 'react-relay';
import {formIsEditing} from 'react_lib/form/FormProvider';

type Props = {|
  +tenant: Tenant,
  +onChange: (tenant: Tenant) => void,
|};
function TenantForm(props: Props): React.Node {
  const { tenant, onChange } = props;

  return (
    <FormProvider
      value={ { errors: tenantCalculateErrors(tenant) } }
    >
      <Text
        scale='display'
        children={formIsEditing(tenant) ? "Editing Tenant: " + tenant.name : "New Tenant"}
      />
      <Separator/>
      <FormSection
        label='Tenant Info'
      >
        <FormField
          editorFor='name'
          label='Name'
        >
          <TextInput
            value={ tenant.name }
            onChange={ (value) => {
              onChange({ ...tenant, name: value })
            } }
          />
        </FormField>
        <FormField
          editorFor='phone'
          label='Phone'
        >
          <TextInput
            value={ tenant.phone }
            onChange={ (value) => {
              onChange({ ...tenant, phone: value })
            } }
          />
        </FormField>
        <FormField
          editorFor='email'
          label='Email'
        >
          <TextInput
            value={ tenant.email }
            onChange={ (value) => {
              onChange({ ...tenant, email: value })
            } }
          />
        </FormField>
      </FormSection>
      <FormSection
        label='Permanent Mailing Address'
      >
        <FormField
          editorFor='permanentMailingAddress.theAddress'
          label='The Address'
        >
          <TextInput
            value={ tenant.permanentMailingAddress.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <FormField
          editorFor='permanentMailingAddress.city'
          label='City'
          maxWidth={ 400 }
        >
          <TextInput
            value={ tenant.permanentMailingAddress.city }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.city = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <FormField
          editorFor='permanentMailingAddress.stateOrProvince'
          label='State Or Province'
          maxWidth={ 250 }
        >
          <TextInput
            value={ tenant.permanentMailingAddress.stateOrProvince }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.stateOrProvince = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <FormField
          editorFor='permanentMailingAddress.zip'
          label='Zip or Postal Code'
          maxWidth={ 150 }
        >
          <TextInput
            value={ tenant.permanentMailingAddress.zip }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.zip = value;
              onChange(newObj);
            } }
          />
        </FormField>
      </FormSection>
      <Group
        justifyContent='space-between'
      >
        <Text
          children='* Required'
        />
        <FormSubmitButton
          onClick={ () => save(tenant) }
          action={
            {
              successUrl: '/tenants',
            }
          }
        />
      </Group>
    </FormProvider>
  );
}

type StatefulProps = {|
  +tenant: Tenant,
|};
export function TenantFormStateful(props: StatefulProps): React.Node {
  const tenant = relayToInternal(props.tenant);
  const [editedTenant, setEditedTenant] = React.useState(tenant);
  return <TenantForm
    tenant={ editedTenant }
    onChange={ setEditedTenant }
  />
}

function relayToInternal(relay: any): Tenant {
  return {
    ...relay,
  };
}

function save(tenant: Tenant) {
  const variables = {
    id: tenant.id,
    name: tenant.name,
    phone: tenant.phone,
    email: tenant.email,
    permanentMailingAddress: tenant.permanentMailingAddress,
  };

  basicCommitMutation(mutation, variables);
}

const mutation = graphql`
  mutation TenantFormMutation(
    $id: String!
    $name: String!
    $phone: String!
    $email: String!
    $permanentMailingAddress: AddressInput!
  ) {
    createOrUpdateTenant(
      id: $id
      name: $name
      phone: $phone
      email: $email
      permanentMailingAddress: $permanentMailingAddress
    )
  }
`;

// $FlowExpectedError
export default createFragmentContainer(TenantFormStateful, {
  tenant: graphql`
    fragment TenantForm_tenant on Tenant {
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
  `,
});

