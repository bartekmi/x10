// @flow

import invariant from "invariant";
import * as React from "react";
import { v4 as uuid } from 'uuid';
import { graphql, commitMutation } from "react-relay";

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
import VisibilityControl from "react_lib/VisibilityControl";
import FormSubmitButton from "react_lib/form/FormSubmitButton";
import FormErrorDisplay from "react_lib/form/FormErrorDisplay";
import FormSection from "react_lib/form/FormSection";
import FormFooter from "react_lib/form/FormFooter";
import FormProvider from "react_lib/form/FormProvider";
import MultiStacker from "react_lib/multi/MultiStacker";

import environment from "environment";

import { type PetPolicyEnum, PetPolicyEnumPairs } from "constants/PetPolicyEnum";
import { type MailboxTypeEnum, MailboxTypeEnumPairs } from "constants/MailboxTypeEnum";
import AddressEditPage from "pages/AddressEditPage";
import UnitEdit, { createDefaultUnit, type Unit } from "pages/UnitEdit";

import { type Address, createDefaultAddress } from "entities/Address";
import { type Building, createDefaultBuilding } from "entities/Building";

type Props = {|
  +building: Building,
  +onChange: (building: Building) => void,
|};
export default function BuildingEditPage(props: Props): React.Node {
  const { building, onChange } = props;
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
  } = building;

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
                onChange({ ...building, name: value })
              }}
            />
          </FormField>

          <FormField label="Description: ">
            <TextareaInput
              value={description}
              onChange={(value) => {
                onChange({ ...building, description: value })
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
                onChange({ ...building, dateOfOccupancy: value })
              }}
            />
          </FormField>

          <FormField
            label="Mailbox Type: "
            indicateRequired={true}
          >
            <SelectInput
              value={mailboxType}
              options={MailboxTypeEnumPairs}
              onChange={(value) => {
                invariant(value, "Will always be set");
                onChange({ ...building, mailboxType: value })
              }}
            />
          </FormField>

          <FormField label="Pet Policy: ">
            <SelectInput
              value={petPolicy}
              isNullable={true}
              placeholder="(No Policy)"
              options={PetPolicyEnumPairs}
              onChange={(value) => {
                onChange({ ...building, petPolicy: value })
              }}
            />
          </FormField>
        </Group>

        <FormSection label="Physical Address">
          <AddressEditPage
            address={physicalAddress}
            onChange={(value) => onChange({ ...building, physicalAddress: value })}
          />
        </FormSection>

        <FormSection label="Mailing Address">
          <Checkbox 
            checked={mailingAddressSameAsPhysical}
            label="Mailing Address Same as Physical Address"
            onChange={(value) => onChange({ ...building, mailingAddressSameAsPhysical: value })}
          />

          <VisibilityControl visible={!mailingAddressSameAsPhysical}>
            <AddressEditPage
              address={mailingAddress || createDefaultAddress()}
              onChange={(value) => onChange({ ...building, mailingAddress: value })}
            />
          </VisibilityControl>
        </FormSection>

        <FormSection label="Units">
          <MultiStacker 
            items={units}
            itemDisplayFunc={(data, onChangeInner) => (<UnitEdit 
              unit={data}
              onChange={onChangeInner}
            />)}
            onChange={(value) => onChange({ ...building, units: value })}
            addNewItem={createDefaultUnit}
          />
        </FormSection>

        <FormFooter>
          <Group justifyContent="space-between"> 
            <FormErrorDisplay />
            <FormSubmitButton onClick={() => saveBuilding(building)} />
          </Group>
        </FormFooter>
      </Group>
    </FormProvider>
  );
}

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