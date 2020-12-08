// @flow

import * as React from "react";
import { graphql, QueryRenderer, commitMutation } from "react-relay";

import Group from "latitude/Group"
import Text from "latitude/Text";
import TextInput from "latitude/TextInput";
import TextareaInput from "latitude/TextareaInput";
import SelectInput from "latitude/select/SelectInput";
import Label from "latitude/Label";
import Button from "latitude/button/Button";
import InputError from "latitude/InputError";

import isEmpty from "../lib_components/utils/isEmpty";
import X10_CalendarDateInput from "../lib_components/X10_CalendarDateInput";
import {DBID_LOCALLY_CREATED} from "../lib_components/constants";
import environment from "../environment";

import PetPolicyEnum from "../constants/PetPolicyEnum";
import MailboxTypeEnum from "../constants/MailboxTypeEnum";
import AddressEditPage, {createDefaultAddress} from "./AddressEditPage";

import type { BuildingEditPageQueryResponse } from "./__generated__/BuildingEditPageQuery.graphql";

const DEFAULT_MAILBOX_TYPE = "INBUILDING";
const DEFAULT_PET_POLICY = null;

type Building = $PropertyType<BuildingEditPageQueryResponse, "building">;

type Props = {|
  +building: Building,
|};
function BuildingEditPage(props: Props): React.Node {
  const { building } = props;
  const [editedBuilding, setEditedBuilding] = React.useState(building);
  const { name, description, dateOfOccupancy, petPolicy } = editedBuilding;

  return (
    <Group flexDirection="column">
      <Text scale="headline">{`Editing Building ${name || ""}`}</Text>
      <Label value="Name:" >
        <InputError errorText="Name is mandatory" showError={isEmpty(name)}>
          <TextInput
            value={name}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, name: value })
            }}
          />
        </InputError>
      </Label>
      <Label value="Description:" >
        <TextareaInput
          value={description}
          onChange={(value) => {
            setEditedBuilding({ ...editedBuilding, description: value })
          }}
        />
      </Label>
      <Label value="Date of Occupancy:" >
        <InputError errorText="Date of Occupancy is mandatory" showError={isEmpty(dateOfOccupancy)}>
          <X10_CalendarDateInput
            value={dateOfOccupancy}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, dateOfOccupancy: value })
            }}
          />
        </InputError>
      </Label>
      <Label value="Mailbox Type:" >
        <SelectInput
          value={editedBuilding.mailboxType}
          options={MailboxTypeEnum}
          onChange={(value) => {
            setEditedBuilding({ ...editedBuilding, mailboxType: value || DEFAULT_MAILBOX_TYPE})
          }}
        />
      </Label>
      <Label value="Pet Policy:" >
        <SelectInput
          value={editedBuilding.petPolicy}
          isNullable={true}
          placeholder="(No Policy)"
          options={PetPolicyEnum}
          onChange={(value) => {
            setEditedBuilding({ ...editedBuilding, petPolicy: value})
          }}
        />
      </Label>

      <Text scale="title">Physical Address</Text>
      <AddressEditPage 
        address={editedBuilding.physicalAddress}
        onChange={(value) => setEditedBuilding({ ...editedBuilding, physicalAddress: value })}
      />

      <Button 
        intent="basic" kind="solid"
        onClick={() => saveBuilding(editedBuilding)}
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
export default function BuildingEditPageWrapper(props: WrapperProps): React.Node {
  const stringId = props.match.params.id;
  if (stringId == null) {
    return <BuildingEditPage building={createDefaultBuilding()}/>
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
            <BuildingEditPage
              building={props.building}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

function createDefaultBuilding(): Building {
  return {
    dbid: DBID_LOCALLY_CREATED,
    dateOfOccupancy: null,
    mailboxType: DEFAULT_MAILBOX_TYPE,
    description: "",
    mailingAddressSameAsPhysical: true,
    name: "",
    petPolicy: DEFAULT_PET_POLICY,
    physicalAddress: createDefaultAddress(),
  };
}

const query = graphql`
  query BuildingEditPageQuery($id: Int!) {
    building(id: $id) {
      dbid
      name
      description
      dateOfOccupancy
      mailboxType
      mailingAddressSameAsPhysical
      petPolicy
      physicalAddress {
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

function saveBuilding(building: Building) {
  const variables = {
    dbid: building.dbid,
    name: building.name,
    description: building.description,
    dateOfOccupancy: building.dateOfOccupancy,
    mailboxType: building.mailboxType,
    mailingAddressSameAsPhysical: building.mailingAddressSameAsPhysical,
    petPolicy: building.petPolicy,
    physicalAddress: building.physicalAddress,
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
  mutation BuildingEditPageMutation(
    $dbid: Int!
    $dateOfOccupancy: DateTime!
    $description: String!
    $mailboxType: MailboxTypeEnum!
    $mailingAddressSameAsPhysical: Boolean!
    $name: String!
    $petPolicy: PetPolicyEnum
    $physicalAddress: AddressInput!
  ) {
    createOrUpdateBuilding(
      dbid: $dbid
      dateOfOccupancy: $dateOfOccupancy
      description: $description
      mailboxType: $mailboxType
      mailingAddressSameAsPhysical: $mailingAddressSameAsPhysical
      name: $name
      petPolicy: $petPolicy
      physicalAddress: $physicalAddress
    )
  }
`;