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
import Checkbox from "latitude/Checkbox";

import isEmpty from "../lib_components/utils/isEmpty";
import X10_CalendarDateInput from "../lib_components/X10_CalendarDateInput";
import { DBID_LOCALLY_CREATED } from "../lib_components/constants";
import FormField from "../lib_components/form/FormField";
import FormSubmitButton from "../lib_components/form/FormSubmitButton";
import FormErrorDisplay from "../lib_components/form/FormErrorDisplay";
import { FormProvider } from "../lib_components/form/FormContext";
import MultiStacker from "../lib_components/multi/MultiStacker";

import environment from "../environment";

import PetPolicyEnum from "../constants/PetPolicyEnum";
import MailboxTypeEnum from "../constants/MailboxTypeEnum";
import AddressEditPage, { createDefaultAddress } from "./AddressEditPage";
import UnitEdit, { createDefaultUnit, type Unit } from "./UnitEdit";

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
  const { 
    name, 
    description, 
    dateOfOccupancy, 
    petPolicy, 
    mailboxType,
    mailingAddressSameAsPhysical, 
    physicalAddress, 
    mailingAddress, 
    units 
  } = editedBuilding;

  return (
    <FormProvider value={[]}>
      <Group flexDirection="column">
        <Text scale="headline">{`Editing Building ${name || ""}`}</Text>
        <FormField
          label="Name: "
          indicateRequired={true}
          errorMessage={isEmpty(name) ? "Name is mandatory" : null}
        >
          <TextInput
            value={name}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, name: value })
            }}
          />
        </FormField>

        <FormField label="Description: ">
          <TextareaInput
            value={description}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, description: value })
            }}
          />
        </FormField>

        <FormField
          label="Date of Occupancy: "
          indicateRequired={true}
          errorMessage={isEmpty(dateOfOccupancy) ? "Date of Occupancy is mandatory" : null}
        >
          <X10_CalendarDateInput
            value={dateOfOccupancy}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, dateOfOccupancy: value })
            }}
          />
        </FormField>

        <FormField
          label="Mailbox Type: "
          indicateRequired={true}
        >
          <SelectInput
            value={mailboxType}
            options={MailboxTypeEnum}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, mailboxType: value || DEFAULT_MAILBOX_TYPE })
            }}
          />
        </FormField>

        <FormField label="Pet Policy: ">
          <SelectInput
            value={petPolicy}
            isNullable={true}
            placeholder="(No Policy)"
            options={PetPolicyEnum}
            onChange={(value) => {
              setEditedBuilding({ ...editedBuilding, petPolicy: value })
            }}
          />
        </FormField>

        <Text scale="title">Physical Address</Text>
        <AddressEditPage
          address={physicalAddress}
          onChange={(value) => setEditedBuilding({ ...editedBuilding, physicalAddress: value })}
        />

        <Checkbox 
          checked={mailingAddressSameAsPhysical}
          label="Mailing Address Same as Physical Address"
          onChange={(value) => setEditedBuilding({ ...editedBuilding, mailingAddressSameAsPhysical: value })}
        />

        {!mailingAddressSameAsPhysical ? (
          <Group flexDirection="column">
            <Text scale="title">Mailing Address</Text>
            <AddressEditPage
              address={mailingAddress || createDefaultAddress()}
              onChange={(value) => setEditedBuilding({ ...editedBuilding, mailingAddress: value })}
            />
          </Group>
        ) : null}

        <Text scale="title">Units</Text>
        <MultiStacker 
          items={units}
          itemDisplayFunc={(data, onChangeInner) => (<UnitEdit 
            unit={data}
            onChange={onChangeInner}
          />)}
          onChange={(value) => setEditedBuilding({ ...editedBuilding, units: value })}
          addNewItem={createDefaultUnit}
        />

        <Group>
          <FormErrorDisplay />
          <FormSubmitButton onClick={() => saveBuilding(editedBuilding)} />
        </Group>
      </Group>
    </FormProvider>
  );
}

function createUnitEdit(data: Unit, onChange: (unit: Unit) => void): React.Node {
  return (
    <UnitEdit 
      unit={data}
      onChange={onChange}
    />
  );
}

// TODO: Split this out into a separate wrapper file!
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
    return <BuildingEditPage building={createDefaultBuilding()} />
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
    mailingAddress: null,
    mailingAddressSameAsPhysical: true,
    name: "",
    petPolicy: DEFAULT_PET_POLICY,
    physicalAddress: createDefaultAddress(),
    units: [],
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
      mailingAddress {
        city
        dbid
        stateOrProvince
        theAddress
        unitNumber
        zip
      }
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
      units {
        id
        dbid
        hasBalcony
        number
        numberOfBathrooms
        numberOfBedrooms
        squareFeet
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
    mailingAddress: building.mailingAddress,
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
    $mailingAddress: AddressInput
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
      mailingAddress: $mailingAddress
      mailingAddressSameAsPhysical: $mailingAddressSameAsPhysical
      name: $name
      petPolicy: $petPolicy
      physicalAddress: $physicalAddress
    )
  }
`;