// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { addressSecondAddressLine, createDefaultAddress } from 'entities/Address';
import { buildingAgeInYears, buildingApplicableWhenForMailingAddress, buildingCalculateErrors, MailboxTypeEnumPairs, PetPolicyEnumPairs, type Building } from 'entities/Building';
import { createDefaultUnit, NumberOfBathroomsEnumPairs } from 'entities/Unit';
import Checkbox from 'latitude/Checkbox';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import SelectInput from 'latitude/select/SelectInput';
import Text from 'latitude/Text';
import TextareaInput from 'latitude/TextareaInput';
import TextInput from 'latitude/TextInput';
import * as React from 'react';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import MultiStacker from 'react_lib/multi/MultiStacker';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';
import x10toString from 'react_lib/utils/x10toString';
import VisibilityControl from 'react_lib/VisibilityControl';
import X10_CalendarDateInput from 'react_lib/X10_CalendarDateInput';
import { createFragmentContainer, graphql } from 'react-relay';


type Props = {|
  +building: Building,
  +onChange: (building: Building) => void,
|};
function BuildingForm(props: Props): React.Node {
  const { building, onChange } = props;

  return (
    <FormProvider
      value={ { errors: buildingCalculateErrors(building) } }
    >
      <Group
        flexDirection='column'
      >
        <Text
          scale='display'
          children={ 'Editing Building in: ' + addressSecondAddressLine(building.physicalAddress) }
        />
        <Text
          scale='display'
          children={ 'Age in Years: ' + x10toString(buildingAgeInYears(building)) }
        />
        <Separator/>
        <FormSection
          label='Building Info'
        >
          <FormField
            editorFor='moniker'
            label='Moniker'
          >
            <TextInput
              value={ building.moniker }
              onChange={ () => { } }
              readOnly={ true }
            />
          </FormField>
          <FormField
            editorFor='name'
            toolTip='A short and memorable name of the Building'
            label='Name'
            maxWidth={ 350 }
          >
            <TextInput
              value={ building.name }
              onChange={ (value) => {
                onChange({ ...building, name: value })
              } }
            />
          </FormField>
          <FormField
            editorFor='description'
            toolTip='Description for advertising purposes'
            label='Description'
          >
            <TextareaInput
              value={ building.description }
              onChange={ (value) => {
                onChange({ ...building, description: value })
              } }
              rows={ 3 }
            />
          </FormField>
        </FormSection>
        <FormSection
          label='Physical Address'
        >
          <FormField
            editorFor='physicalAddress.theAddress'
            label='The Address'
          >
            <TextInput
              value={ building.physicalAddress.theAddress }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.theAddress = value;
                onChange(newObj);
              } }
            />
          </FormField>
          <FormField
            editorFor='physicalAddress.city'
            label='City'
            maxWidth={ 400 }
          >
            <TextInput
              value={ building.physicalAddress.city }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.city = value;
                onChange(newObj);
              } }
            />
          </FormField>
          <FormField
            editorFor='physicalAddress.stateOrProvince'
            label='State Or Province'
            maxWidth={ 250 }
          >
            <TextInput
              value={ building.physicalAddress.stateOrProvince }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.stateOrProvince = value;
                onChange(newObj);
              } }
            />
          </FormField>
          <FormField
            editorFor='physicalAddress.zip'
            label='Zip or Postal Code'
            maxWidth={ 150 }
          >
            <TextInput
              value={ building.physicalAddress.zip }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.zip = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </FormSection>
        <FormSection
          label='Mailing Address'
        >
          <Checkbox
            checked={ building.mailingAddressSameAsPhysical }
            onChange={ (value) => {
              onChange({ ...building, mailingAddressSameAsPhysical: value })
            } }
            label='Mailing Address Same as Physical Address'
          />
          <VisibilityControl
            visible={ buildingApplicableWhenForMailingAddress(building) }
          >
            <Group
              flexDirection='column'
            >
              <FormField
                editorFor='mailingAddress.theAddress'
                label='Address or Post Office (PO) Box'
              >
                <TextInput
                  value={ building.mailingAddress.theAddress }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.theAddress = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
              <FormField
                editorFor='mailingAddress.city'
                label='City'
                maxWidth={ 400 }
              >
                <TextInput
                  value={ building.mailingAddress.city }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.city = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
              <FormField
                editorFor='mailingAddress.stateOrProvince'
                label='State Or Province'
                maxWidth={ 250 }
              >
                <TextInput
                  value={ building.mailingAddress.stateOrProvince }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.stateOrProvince = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
              <FormField
                editorFor='mailingAddress.zip'
                label='Zip or Postal Code'
                maxWidth={ 150 }
              >
                <TextInput
                  value={ building.mailingAddress.zip }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.zip = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
            </Group>
          </VisibilityControl>
        </FormSection>
        <FormSection
          label='Other Details'
        >
          <Group
            gap={ 40 }
          >
            <FormField
              editorFor='dateOfOccupancy'
              label='Date Of Occupancy'
            >
              <X10_CalendarDateInput
                value={ building.dateOfOccupancy }
                onChange={ (value) => {
                  onChange({ ...building, dateOfOccupancy: value })
                } }
              />
            </FormField>
            <FormField
              editorFor='ageInYears'
              toolTip='Strongly affects annual propery maintenance costs.'
              label='Age In Years'
            >
              <FloatInput
                value={ buildingAgeInYears(building) }
                onChange={ () => { } }
                readOnly={ true }
              />
            </FormField>
          </Group>
          <FormField
            editorFor='mailboxType'
            label='Mailbox Type'
          >
            <SelectInput
              value={ building.mailboxType }
              onChange={ (value) => {
                onChange({ ...building, mailboxType: value })
              } }
              options={ MailboxTypeEnumPairs }
            />
          </FormField>
          <FormField
            editorFor='petPolicy'
            label='Pet Policy'
          >
            <SelectInput
              value={ building.petPolicy }
              onChange={ (value) => {
                onChange({ ...building, petPolicy: value })
              } }
              options={ PetPolicyEnumPairs }
            />
          </FormField>
        </FormSection>
        <FormSection
          label='Units'
        >
          <MultiStacker
            items={ building.units }
            onChange={ (value) => {
              onChange({ ...building, units: value })
            } }
            itemDisplayFunc={ (data, onChange) => (
              <Group
                flexDirection='column'
              >
                <Group>
                  <FormField
                    editorFor='number'
                    toolTip='Unit number corresponding to mailing address'
                    label='Number'
                  >
                    <TextInput
                      value={ data.number }
                      onChange={ (value) => {
                        onChange({ ...data, number: value })
                      } }
                    />
                  </FormField>
                  <FormField
                    editorFor='squareFeet'
                    label='Square Feet'
                  >
                    <FloatInput
                      value={ data.squareFeet }
                      onChange={ (value) => {
                        onChange({ ...data, squareFeet: value })
                      } }
                      decimalPrecision={ 0 }
                    />
                  </FormField>
                  <FormField
                    editorFor='hasBalcony'
                    label='Unit has Blacony?'
                  >
                    <Checkbox
                      checked={ data.hasBalcony }
                      onChange={ (value) => {
                        onChange({ ...data, hasBalcony: value })
                      } }
                    />
                  </FormField>
                </Group>
                <Group>
                  <FormField
                    editorFor='numberOfBedrooms'
                    label='Number Of Bedrooms'
                  >
                    <FloatInput
                      value={ data.numberOfBedrooms }
                      onChange={ (value) => {
                        onChange({ ...data, numberOfBedrooms: value })
                      } }
                    />
                  </FormField>
                  <FormField
                    editorFor='numberOfBathrooms'
                    label='Number Of Bathrooms'
                  >
                    <SelectInput
                      value={ data.numberOfBathrooms }
                      onChange={ (value) => {
                        onChange({ ...data, numberOfBathrooms: value })
                      } }
                      options={ NumberOfBathroomsEnumPairs }
                    />
                  </FormField>
                </Group>
              </Group>
            ) }
            addNewItem={ createDefaultUnit }
          />
        </FormSection>
        <Group
          justifyContent='space-between'
        >
          <Text
            children='* Required'
          />
          <FormSubmitButton
            onClick={ () => save(building) }
            action={
              {
                successUrl: '/buildings',
              }
            }
          />
        </Group>
      </Group>
    </FormProvider>
  );
}

type StatefulProps = {|
  +building: Building,
|};
export function BuildingFormStateful(props: StatefulProps): React.Node {
  const building = relayToInternal(props.building);
  const [editedBuilding, setEditedBuilding] = React.useState(building);
  return <BuildingForm
    building={ editedBuilding }
    onChange={ setEditedBuilding }
  />
}

function relayToInternal(relay: any): Building {
  return {
    ...relay,
    mailingAddress: relay.mailingAddress || createDefaultAddress(),
  };
}

function save(building: Building) {
  const variables = {
    id: building.id,
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
    $id: String!
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
      id: $id
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

// $FlowExpectedError
export default createFragmentContainer(BuildingFormStateful, {
  building: graphql`
    fragment BuildingForm_building on Building {
      id
      dateOfOccupancy
      description
      mailboxType
      mailingAddress {
        id
        city
        stateOrProvince
        theAddress
        zip
      }
      mailingAddressSameAsPhysical
      moniker
      name
      petPolicy
      physicalAddress {
        id
        city
        stateOrProvince
        theAddress
        zip
      }
      units {
        id
        hasBalcony
        number
        numberOfBathrooms
        numberOfBedrooms
        squareFeet
      }
    }
  `,
});

