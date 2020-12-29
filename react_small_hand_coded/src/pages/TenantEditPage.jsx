// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';
import { graphql, QueryRenderer, commitMutation } from "react-relay";

import Group from "latitude/Group"
import Text from "latitude/Text";
import TextInput from "latitude/TextInput";
import TextareaInput from "latitude/TextareaInput";
import Label from "latitude/Label";
import Button from "latitude/button/Button";

import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';

import environment from "../environment";
import {DBID_LOCALLY_CREATED} from "react_lib/constants";

import type { TenantEditPageQueryResponse } from "./__generated__/TenantEditPageQuery.graphql";

type Tenant = $PropertyType<TenantEditPageQueryResponse, "tenant">;

type Props = {|
  +tenant: Tenant,
|};
function TenantEditPage(props: Props): React.Node {
  const { tenant } = props;
  const [editedTenant, setEditedTenant] = React.useState(tenant);

  const {
    name,
    phone,
    email,
    permanentMailingAddress,
  } = editedTenant;

  return (
    <FormProvider value={ [] }>
      <Text scale="headline">{`Editing Tenant ${name || ""}`}</Text>
      <FormSection
          label='Tenant Info'
      >
        <Label value="Name:" >
          <TextInput
            value={name}
            onChange={(value) => {
              setEditedTenant({ ...editedTenant, name: value })
            }}
          />
        </Label>
        <Label value="Phone:" >
          <TextInput
            value={phone || ""}
            onChange={(value) => {
              setEditedTenant({ ...editedTenant, phone: value })
            }}
          />
        </Label>
        <Label value="Email:" >
          <TextInput
            value={email}
            onChange={(value) => {
              setEditedTenant({ ...editedTenant, email: value })
            }}
          />
        </Label>
      </FormSection>
      <FormSection
          label='Permanent Mailing Address'
      >
        <Label
            value='Address'
        >
          <TextInput
            value={ permanentMailingAddress.theAddress || "" }
            onChange={ (value) => {
              const newValue = { ...editedTenant, permanentMailingAddress: {
                ...editedTenant.permanentMailingAddress,
                theAddress: value,
              }};
              setEditedTenant(newValue);
            } }
          />
        </Label>
        <Label
            value='City'
        >
          <TextInput
            value={ permanentMailingAddress.city || "" }
            onChange={ (value) => {
              const newValue = { ...editedTenant, permanentMailingAddress: {
                ...editedTenant.permanentMailingAddress,
                city: value,
              }};
              setEditedTenant(newValue);
            } }
          />
        </Label>
        <Label
            value='State Or Province'
        >
          <TextInput
            value={ permanentMailingAddress.stateOrProvince || "" }
            onChange={ (value) => {
              const newValue = { ...editedTenant, permanentMailingAddress: {
                ...editedTenant.permanentMailingAddress,
                stateOrProvince: value,
              }};
              setEditedTenant(newValue);
            } }
          />
        </Label>
        <Label
            value='Zip or Postal Code'
        >
          <TextInput
            value={ permanentMailingAddress.zip || "" }
            onChange={ (value) => {
              const newValue = { ...editedTenant, permanentMailingAddress: {
                ...editedTenant.permanentMailingAddress,
                zip: value,
              }};
              // $FlowIgnore
              setEditedTenant(newValue);
            } }
          />
        </Label>
      </FormSection>
      <Button 
        intent="basic" kind="solid"
        onClick={() => saveTenant(editedTenant)}
      >
        Save
      </Button>
    </FormProvider>
  );
}

type WrapperProps = {
  +match: {
  +params: {
    +id: string
  }
}
};
export default function TenantEditPageWrapper(props: WrapperProps): React.Node {
  const stringId = props.match.params.id;
  if (stringId == null) {
    return <TenantEditPage tenant={createDefaultTenant()}/>
  }

  const id: number = parseInt(stringId);
  if (isNaN(id)) {
    throw new Error("Not a number: " + stringId);
  }

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{
        id
      }}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          return (
            <TenantEditPage
              tenant={props.tenant}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

function createDefaultTenant(): Tenant {
  return {
    dbid: DBID_LOCALLY_CREATED,
    name: "",
    phone: "",
    email: "",
    permanentMailingAddress: {
      id: uuid(),
      dbid: DBID_LOCALLY_CREATED,
      city: "",
      stateOrProvince: "",
      theAddress: "",
      unitNumber: "",
      zip: "",
    },
  };
}

const query = graphql`
  query TenantEditPageQuery($id: Int!) {
    tenant(id: $id) {
      dbid
      name
      phone
      email
      permanentMailingAddress {
        id
        city
        dbid
        stateOrProvince
        theAddress
        unitNumber
        zip
      }
    }
  }
`;

function saveTenant(tenant: Tenant) {
  const variables = {
    dbid: tenant.dbid,
    name: tenant.name,
    phone: tenant.phone,
    email: tenant.email,
    permanentMailingAddress: tenant.permanentMailingAddress,
  };

  return commitMutation(
    environment,
    {
      mutation,
      variables,
    }
  );
}

const mutation = graphql`
  mutation TenantEditPageMutation(
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