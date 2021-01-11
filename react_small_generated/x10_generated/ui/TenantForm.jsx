// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import * as React from 'react';

import Group from 'latitude/Group';
import Label from 'latitude/Label';
import Text from 'latitude/Text';
import TextInput from 'latitude/TextInput';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import { graphql } from 'react-relay';


import { type Tenant } from 'entities/Tenant';

type Props = {|
  +tenant: Tenant,
  +onChange: (tenant: Tenant) => void,
|};
export default function TenantForm(props: Props): React.Node {
  const { tenant, onChange } = props;

  return (
    <FormProvider
      value={ [] }
    >
      <FormSection
        label='Tenant Info'
      >
        <Label
          value='Name'
        >
          <TextInput
            value={ tenant.name }
            onChange={ (value) => {
              onChange({ ...tenant, name: value })
            } }
          />
        </Label>
        <Label
          value='Phone'
        >
          <TextInput
            value={ tenant.phone }
            onChange={ (value) => {
              onChange({ ...tenant, phone: value })
            } }
          />
        </Label>
        <Label
          value='Email'
        >
          <TextInput
            value={ tenant.email }
            onChange={ (value) => {
              onChange({ ...tenant, email: value })
            } }
          />
        </Label>
      </FormSection>
      <FormSection
        label='Permanent Mailing Address'
      >
        <Label
          value='The Address'
        >
          <TextInput
            value={ tenant.permanentMailingAddress.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='City'
        >
          <TextInput
            value={ tenant.permanentMailingAddress.city }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.city = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='State Or Province'
        >
          <TextInput
            value={ tenant.permanentMailingAddress.stateOrProvince }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.stateOrProvince = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='Zip or Postal Code'
        >
          <TextInput
            value={ tenant.permanentMailingAddress.zip }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(tenant));
              newObj.permanentMailingAddress.zip = value;
              onChange(newObj);
            } }
          />
        </Label>
      </FormSection>
      <Group
        justifyContent='space-between'
      >
        <Text
          children='* Required'
        />
        <Group>
          <FormSubmitButton
            onClick={ () => save(tenant) }
          />
        </Group>
      </Group>
    </FormProvider>
  );
}

function save(tenant: Tenant) {
  const variables = {
    dbid: tenant.dbid,
    name: tenant.name,
    phone: tenant.phone,
    email: tenant.email,
    permanentMailingAddress: tenant.permanentMailingAddress,
  };

  basicCommitMutation(mutation, variables);
}

const mutation = graphql`
  mutation TenantFormMutation(
    $dbid: Int!
    $name: String!
    $phone: String!
    $email: String!
    $permanentMailingAddress: AddressInput!
  ) {
    createOrUpdateTenant(
      dbid: $dbid
      name: $name
      phone: $phone
      email: $email
      permanentMailingAddress: $permanentMailingAddress
    )
  }
`;

