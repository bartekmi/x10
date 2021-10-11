// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import TextDisplay from 'react_lib/display/TextDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';
import isExistingObject from 'react_lib/utils/isExistingObject';
import x10toString from 'react_lib/utils/x10toString';

import { tenantCalculateErrors, type Tenant } from 'small/entities/Tenant';



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
        weight='bold'
        children={ isExistingObject(tenant?.id) ? 'Editing Tenant: ' + x10toString(tenant?.name) : 'New Tenant' }
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
            value={ tenant?.name }
            onChange={ (value) => {
              // $FlowExpectedError
              onChange({ ...tenant, name: value })
            } }
          />
        </FormField>
        <FormField
          editorFor='phone'
          label='Phone'
        >
          <TextInput
            value={ tenant?.phone }
            onChange={ (value) => {
              // $FlowExpectedError
              onChange({ ...tenant, phone: value })
            } }
          />
        </FormField>
        <FormField
          editorFor='email'
          label='Email'
        >
          <TextInput
            value={ tenant?.email }
            onChange={ (value) => {
              // $FlowExpectedError
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
            value={ tenant?.permanentMailingAddress?.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.tenant.permanentMailingAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <StyleControl
          width={ 400 }
          maxWidth={ 400 }
        >
          <FormField
            editorFor='permanentMailingAddress.city'
            label='City'
          >
            <TextInput
              value={ tenant?.permanentMailingAddress?.city }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(tenant));
                newObj.tenant.permanentMailingAddress.city = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <StyleControl
          width={ 250 }
          maxWidth={ 250 }
        >
          <FormField
            editorFor='permanentMailingAddress.stateOrProvince'
            label='State Or Province'
          >
            <TextInput
              value={ tenant?.permanentMailingAddress?.stateOrProvince }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(tenant));
                newObj.tenant.permanentMailingAddress.stateOrProvince = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <StyleControl
          width={ 150 }
          maxWidth={ 150 }
        >
          <FormField
            editorFor='permanentMailingAddress.zip'
            label='Zip or Postal Code'
          >
            <TextInput
              value={ tenant?.permanentMailingAddress?.zip }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(tenant));
                newObj.tenant.permanentMailingAddress.zip = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <FormField
          editorFor='permanentMailingAddress.country'
          label='Country'
        >
          <AssociationEditor
            id={ tenant?.permanentMailingAddress?.country?.id }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.tenant.permanentMailingAddress.country = value == null ? null : { id: value };
              onChange(newObj);
            } }
            isNullable={ false }
            query={ countriesQuery }
            toString={ x => x.toStringRepresentation }
          />
        </FormField>
      </FormSection>
      <Group
        justifyContent='space-between'
      >
        <TextDisplay
          value='* Required'
        />
        <FormSubmitButton
          onClick={ () => save(tenant) }
          action={
            {
              successUrl: '/tenants',
            }
          }
          label='Save'
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
  basicCommitMutation(mutation, { tenant });
}

const mutation = graphql`
  mutation TenantFormMutation(
    $tenant: TenantInput!
  ) {
    createOrUpdateTenant(
      tenant: $tenant
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
        country {
          id
          toStringRepresentation
        }
        stateOrProvince
        theAddress
        zip
      }
      phone
    }
  `,
});

const countriesQuery = graphql`
  query TenantForm_countriesQuery {
    entities: countries {
      id
      toStringRepresentation
    }
  }
`;

