// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import * as React from 'react';

import { addressSecondAddressLine } from 'entities/Address';
import { buildingAgeInYears, buildingApplicableWhenForMailingAddress, MailboxTypeEnumPairs, PetPolicyEnumPairs } from 'entities/Building';
import Checkbox from 'latitude/Checkbox';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import Label from 'latitude/Label';
import SelectInput from 'latitude/select/SelectInput';
import Text from 'latitude/Text';
import TextareaInput from 'latitude/TextareaInput';
import TextInput from 'latitude/TextInput';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import VisibilityControl from 'react_lib/VisibilityControl';
import X10_CalendarDateInput from 'react_lib/X10_CalendarDateInput';
import { graphql } from 'react-relay';


import { type Building } from 'entities/Building';

type Props = {|
  +building: Building,
  +onChange: (building: Building) => void,
|};
export default function BuildingForm(props: Props): React.Node {
  const { building, onChange } = props;

  return (
    <FormProvider
      value={ [] }
    >
      <Text
        scale='display'
        children={ 'Editing Building in: ' + addressSecondAddressLine(building.physicalAddress) }
      />
      <Text
        scale='display'
        children={ buildingAgeInYears(building) }
      />
      <FormSection
        label='Building Info'
      >
        <Label
          value='Moniker'
        >
          <TextInput
            value={ building.moniker }
            onChange={ () => { } }
            readOnly={ true }
          />
        </Label>
        <Label
          value='Name'
        >
          <TextInput
            value={ building.name }
            onChange={ (value) => {
              onChange({ ...building, name: value })
            } }
          />
        </Label>
        <Label
          value='Description'
        >
          <TextareaInput
            value={ building.description }
            onChange={ (value) => {
              onChange({ ...building, description: value })
            } }
            rows={ 3 }
          />
        </Label>
      </FormSection>
      <FormSection
        label='Physical Address'
      >
        <Label
          value='The Address'
        >
          <TextInput
            value={ building.physicalAddress.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='City'
        >
          <TextInput
            value={ building.physicalAddress.city }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.city = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='State Or Province'
        >
          <TextInput
            value={ building.physicalAddress.stateOrProvince }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.stateOrProvince = value;
              onChange(newObj);
            } }
          />
        </Label>
        <Label
          value='Zip or Postal Code'
        >
          <TextInput
            value={ building.physicalAddress.zip }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.zip = value;
              onChange(newObj);
            } }
          />
        </Label>
      </FormSection>
      <FormSection
        label='Mailing Address'
      >
        <Label
          value='Mailing Address is Same as Physical Address'
        >
          <Checkbox
            checked={ building.mailingAddressSameAsPhysical }
            onChange={ (value) => {
              onChange({ ...building, mailingAddressSameAsPhysical: value })
            } }
          />
        </Label>
        <VisibilityControl
          visible={ buildingApplicableWhenForMailingAddress(building) }
        >
          <Group
            flexDirection='column'
          >
            <Label
              value='Address or Post Office (PO) Box'
            >
              <TextInput
                value={ building.mailingAddress.theAddress }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.theAddress = value;
                  onChange(newObj);
                } }
              />
            </Label>
            <Label
              value='City'
            >
              <TextInput
                value={ building.mailingAddress.city }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.city = value;
                  onChange(newObj);
                } }
              />
            </Label>
            <Label
              value='State Or Province'
            >
              <TextInput
                value={ building.mailingAddress.stateOrProvince }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.stateOrProvince = value;
                  onChange(newObj);
                } }
              />
            </Label>
            <Label
              value='Zip or Postal Code'
            >
              <TextInput
                value={ building.mailingAddress.zip }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.zip = value;
                  onChange(newObj);
                } }
              />
            </Label>
          </Group>
        </VisibilityControl>
      </FormSection>
      <FormSection
        label='Other Details'
      >
        <Group>
          <Label
            value='Date Of Occupancy'
          >
            <X10_CalendarDateInput
              value={ building.dateOfOccupancy }
              onChange={ (value) => {
                onChange({ ...building, dateOfOccupancy: value })
              } }
            />
          </Label>
          <Label
            value='Age In Years'
          >
            <FloatInput
              value={ buildingAgeInYears(building) }
              onChange={ () => { } }
              readOnly={ true }
            />
          </Label>
        </Group>
        <Label
          value='Mailbox Type'
        >
          <SelectInput
            value={ building.mailboxType }
            onChange={ (value) => {
              onChange({ ...building, mailboxType: value })
            } }
            options={ MailboxTypeEnumPairs }
          />
        </Label>
        <Label
          value='Pet Policy'
        >
          <SelectInput
            value={ building.petPolicy }
            onChange={ (value) => {
              onChange({ ...building, petPolicy: value })
            } }
            options={ PetPolicyEnumPairs }
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
            onClick={ () => save(building) }
          />
        </Group>
      </Group>
    </FormProvider>
  );
}

function save(building: Building) {
  const variables = {
    dbid: building.dbid,
    name: building.name,
    description: building.description,
    dateOfOccupancy: building.dateOfOccupancy,
    mailboxType: building.mailboxType,
    petPolicy: building.petPolicy,
    mailingAddressSameAsPhysical: building.mailingAddressSameAsPhysical,
    units: building.units,
    physicalAddress: building.physicalAddress,
    mailingAddress: building.mailingAddress,
  };

  basicCommitMutation(mutation, variables);
}

const mutation = graphql`
  mutation BuildingFormMutation(
    $dbid: Int!
    $name: String!
    $description: String!
    $dateOfOccupancy: DateTime!
    $mailboxType: MailboxTypeEnum!
    $petPolicy: PetPolicyEnum
    $mailingAddressSameAsPhysical: Boolean!
    $units: [UnitInput!]!
    $physicalAddress: AddressInput!
    $mailingAddress: AddressInput
  ) {
    createOrUpdateBuilding(
      dbid: $dbid
      name: $name
      description: $description
      dateOfOccupancy: $dateOfOccupancy
      mailboxType: $mailboxType
      petPolicy: $petPolicy
      mailingAddressSameAsPhysical: $mailingAddressSameAsPhysical
      units: $units
      physicalAddress: $physicalAddress
      mailingAddress: $mailingAddress
    )
  }
`;

