// @flow

import * as React from "react";
import { graphql, QueryRenderer, commitMutation } from "react-relay";

import Group from "latitude/Group"
import Text from "latitude/Text";
import TextInput from "latitude/TextInput";
import TextareaInput from "latitude/TextareaInput";
import Label from "latitude/Label";
import Button from "latitude/button/Button";

import environment from "../environment";
import {DBID_LOCALLY_CREATED} from "../lib_components/constants";

import type { TenantEditPageQueryResponse } from "./__generated__/TenantEditPageQuery.graphql";

type Tenant = $PropertyType<TenantEditPageQueryResponse, "tenant">;

type Props = {|
  +tenant: Tenant,
|};
function TenantEditPage(props: Props): React.Node {
  const { tenant } = props;
  const [editedTenant, setEditedTenant] = React.useState(tenant);

  const contextValues = {
    on: {
      valueBefore: "ON - BEFORE",
      valueAfter: "ON - AFTER",
    },
    off: {
      valueBefore: "OFF - BEFORE",
      valueAfter: "OFF - AFTER",
    },
  };

  return (
    <Group flexDirection="column">
      <Text scale="headline">{`Editing Tenant ${tenant.name || ""}`}</Text>
      <Label value="Name:" >
        <TextInput
          value={editedTenant.name}
          onChange={(value) => {
            setEditedTenant({ ...editedTenant, name: value })
          }}
        />
      </Label>
      <Label value="Phone:" >
        <TextInput
          value={editedTenant.phone || ""}
          onChange={(value) => {
            setEditedTenant({ ...editedTenant, phone: value })
          }}
        />
      </Label>
      <Label value="Email:" >
        <TextInput
          value={editedTenant.email}
          onChange={(value) => {
            setEditedTenant({ ...editedTenant, email: value })
          }}
        />
      </Label>
      <Button 
        intent="basic" kind="solid"
        onClick={() => saveTenant(editedTenant)}
      >
        Save
      </Button>
    </Group>
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
  };
}

const query = graphql`
  query TenantEditPageQuery($id: Int!) {
    tenant(id: $id) {
      dbid
      name
      phone
      email
    }
  }
`;

function saveTenant(tenant: Tenant) {
  const variables = {
    dbid: tenant.dbid,
    name: tenant.name,
    phone: tenant.phone,
    email: tenant.email,
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
  ) {
    createOrUpdateTenant(
      dbid: $dbid
      name: $name
      phone: $phone
      email: $email
    )
  }
`;