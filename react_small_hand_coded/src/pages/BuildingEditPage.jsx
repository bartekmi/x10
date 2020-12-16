// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';
import { graphql, QueryRenderer, commitMutation } from "react-relay";

import Group from "latitude/Group"
import Text from "latitude/Text";
import TextInput from "latitude/TextInput";
import TextareaInput from "latitude/TextareaInput";
import SelectInput from "latitude/select/SelectInput";
import Label from "latitude/Label";
import Button from "latitude/button/Button";
import Checkbox from "latitude/Checkbox";

import isEmpty from "react_lib/utils/isEmpty";
import X10_CalendarDateInput from "react_lib/X10_CalendarDateInput";
import { DBID_LOCALLY_CREATED } from "react_lib/constants";
import FormField from "react_lib/form/FormField";
import FormSubmitButton from "react_lib/form/FormSubmitButton";
import FormErrorDisplay from "react_lib/form/FormErrorDisplay";
import FormSection from "react_lib/form/FormSection";
import FormFooter from "react_lib/form/FormFooter";
import FormProvider from "react_lib/form/FormProvider";
import MultiStacker from "react_lib/multi/MultiStacker";

import environment from "../environment";

import PetPolicyEnum from "../constants/PetPolicyEnum";
import MailboxTypeEnum from "../constants/MailboxTypeEnum";
import AddressEditPage, { createDefaultAddress } from "./AddressEditPage";
import UnitEdit, { createDefaultUnit, type Unit } from "./UnitEdit";

import type { BuildingEditPageQueryResponse } from "./__generated__/BuildingEditPageQuery.graphql";

const DEFAULT_MAILBOX_TYPE = "IN_BUILDING";
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
        <FormSection label="Building Info">
          <FormField
            label="Name: "
            indicateRequired={true}
            errorMessage={isEmpty(name) ? "Name is mandatory" : null}
            maxWidth={350}
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
        </FormSection>

        <Group gap={40}>
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
        </Group>

        <FormSection label="Physical Address">
          <AddressEditPage
            address={physicalAddress}
            onChange={(value) => setEditedBuilding({ ...editedBuilding, physicalAddress: value })}
          />
        </FormSection>

        <FormSection label="Mailing Address">
          <Checkbox 
            checked={mailingAddressSameAsPhysical}
            label="Mailing Address Same as Physical Address"
            onChange={(value) => setEditedBuilding({ ...editedBuilding, mailingAddressSameAsPhysical: value })}
          />

          {!mailingAddressSameAsPhysical ? (
            <AddressEditPage
              address={mailingAddress || createDefaultAddress()}
              onChange={(value) => setEditedBuilding({ ...editedBuilding, mailingAddress: value })}
            />
          ) : null}
        </FormSection>

        <FormSection label="Units">
          <MultiStacker 
            items={units}
            itemDisplayFunc={(data, onChangeInner) => (<UnitEdit 
              unit={data}
              onChange={onChangeInner}
            />)}
            onChange={(value) => setEditedBuilding({ ...editedBuilding, units: value })}
            addNewItem={createDefaultUnit}
          />
        </FormSection>

        <FormFooter>
          <Group justifyContent="space-between"> 
            <FormErrorDisplay />
            <FormSubmitButton onClick={() => saveBuilding(editedBuilding)} />
          </Group>
        </FormFooter>
      </Group>
    </FormProvider>
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
    id: uuid(),
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
      id
      dbid
      name
      description
      dateOfOccupancy
      mailboxType
      mailingAddress {
        id
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
        id
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
    units: building.units,
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
    $units: [UnitInput!]!
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
      units: $units
    )
  }
`;