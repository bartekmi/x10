import { gql } from '@apollo/client';
import { Flex, Heading } from '@chakra-ui/react';
import * as React from 'react';

import TextInput from 'react_lib/chakra_wrappers/TextInput';
import TextDisplay from 'react_lib/display/TextDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';
import isExistingObject from 'react_lib/utils/isExistingObject';
import x10toString from 'react_lib/utils/x10toString';

import { tenantCalculateErrors, type Tenant } from 'x10_generated/small/entities/Tenant';

import { TenantForm_TenantFragment } from '__generated__/graphql';



type Props = {
  readonly tenant: TenantForm_TenantFragment,
  readonly onChange: (tenant: TenantForm_TenantFragment) => void,
};
function TenantForm(props: Props): React.JSX.Element {
  const { tenant, onChange } = props;

  return (
    <FormProvider
      context={ { errors: tenantCalculateErrors(tenant as Tenant) } }
    >
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children={ isExistingObject(tenant?.id) ? 'Editing Tenant: ' + x10toString(tenant?.name) : 'New Tenant' }
      />
      <Separator/>
      <FormSection
        label='Tenant Info'
      >
        <FormField
          editorFor='name'
          indicateRequired={ true }
          label='Name'
        >
          <TextInput
            value={ tenant?.name }
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
            value={ tenant?.phone }
            onChange={ (value) => {
              onChange({ ...tenant, phone: value })
            } }
          />
        </FormField>
        <FormField
          editorFor='email'
          indicateRequired={ true }
          label='Email'
        >
          <TextInput
            value={ tenant?.email }
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
          indicateRequired={ true }
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
          maxWidth={ 400 }
        >
          <FormField
            editorFor='permanentMailingAddress.city'
            indicateRequired={ true }
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
          maxWidth={ 250 }
        >
          <FormField
            editorFor='permanentMailingAddress.stateOrProvince'
            indicateRequired={ true }
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
          maxWidth={ 150 }
        >
          <FormField
            editorFor='permanentMailingAddress.zip'
            indicateRequired={ true }
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
          indicateRequired={ true }
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
          />
        </FormField>
      </FormSection>
      <Flex
        justifyContent='space-between'
      >
        <TextDisplay
          value='* Required'
        />
        <FormSubmitButton
          mutation={ mutation }
          variables={
            {
              tenant: {
                id: tenant.id,
                id: tenant.id,
                name: tenant.name,
                phone: tenant.phone,
                email: tenant.email,
                permanentMailingAddress: tenant.permanentMailingAddress,
              }
            }
          }
          label='Save'
        />
      </Flex>
    </FormProvider>
  );
}

type StatefulProps = {
  readonly tenant: TenantForm_TenantFragment,
};
export function TenantFormStateful(props: StatefulProps): React.JSX.Element {
  const [editedTenant, setEditedTenant] = React.useState(props.tenant);
  return <TenantForm
    tenant={ editedTenant }
    onChange={ setEditedTenant }
  />
}

const mutation = gql`
  mutation TenantFormMutation(
    $tenant: TenantFormTenantInput!
  ) {
    tenantFormUpdateTenant(
      data: $tenant
    ) {
      id
      email
      name
      permanentMailingAddress {
        id
        city
        country {
          id
        }
        stateOrProvince
        theAddress
        zip
      }
      phone
    }
  }
`;

  gql`
    fragment TenantForm_Tenant on Tenant {
      id
      email
      name
      permanentMailingAddress {
        id
        city
        country {
          id
        }
        stateOrProvince
        theAddress
        zip
      }
      phone
    }
  `

const countriesQuery = graphql`
  query TenantForm_countriesQuery {
    entities: countries {
      id
    }
  }
`;

