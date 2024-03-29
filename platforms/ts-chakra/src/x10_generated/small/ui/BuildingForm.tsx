// Auto-generated by x10 - do not edit
import { gql } from '@apollo/client';
import { Heading } from '@chakra-ui/react';
import * as React from 'react';

import CalendarDateInput from 'react_lib/chakra_wrappers/CalendarDateInput';
import Checkbox from 'react_lib/chakra_wrappers/Checkbox';
import FloatInput from 'react_lib/chakra_wrappers/FloatInput';
import IntInput from 'react_lib/chakra_wrappers/IntInput';
import RadioGroup from 'react_lib/chakra_wrappers/RadioGroup';
import SelectInput from 'react_lib/chakra_wrappers/SelectInput';
import TextareaInput from 'react_lib/chakra_wrappers/TextareaInput';
import TextInput from 'react_lib/chakra_wrappers/TextInput';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import FormErrorDisplay from 'react_lib/form/FormErrorDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSection from 'react_lib/form/FormSection';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import Group from 'react_lib/Group';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import MultiStacker from 'react_lib/multi/MultiStacker';
import StyleControl from 'react_lib/StyleControl';
import x10toString from 'react_lib/utils/x10toString';

import { AppContext } from 'SmallAppContext';
import { addressSecondAddressLine } from 'x10_generated/small/entities/Address';
import { buildingAgeInYears, buildingApplicableWhenForMailingAddress, buildingCalculateErrors, MailboxTypeEnumPairs, PetPolicyEnumPairs, type Building } from 'x10_generated/small/entities/Building';
import { countryToStringRepresentation } from 'x10_generated/small/entities/Country';
import { createDefaultUnit, NumberOfBathroomsEnumPairs } from 'x10_generated/small/entities/Unit';



type Props = {
  readonly building: Building,
  readonly onChange: (building: Building) => void,
};
function BuildingForm(props: Props): React.JSX.Element {
  const { building, onChange } = props;
  const appContext = React.useContext(AppContext);

  return (
    <FormProvider
      context={ { errors: buildingCalculateErrors(appContext, building) } }
    >
      <Heading
        as='h1'
        size='2xl'
        children={ 'Editing Building in: ' + x10toString(addressSecondAddressLine(appContext, building?.physicalAddress)) }
      />
      <FormSection
        label='Building Info'
      >
        <FormField
          editorFor='moniker'
          label='Moniker'
        >
          <TextDisplay
            value={ building?.moniker }
          />
        </FormField>
        <StyleControl
          maxWidth={ 350 }
        >
          <FormField
            editorFor='name'
            indicateRequired={ true }
            toolTip='A short and memorable name of the Building'
            label='Name'
          >
            <TextInput
              value={ building?.name }
              onChange={ (value) => {
                onChange({ ...building, name: value })
              } }
              placeholder='Name of building'
            />
          </FormField>
        </StyleControl>
        <FormField
          editorFor='description'
          toolTip='Description for advertising purposes'
          label='Description'
        >
          <TextareaInput
            value={ building?.description }
            onChange={ (value) => {
              onChange({ ...building, description: value })
            } }
          />
        </FormField>
      </FormSection>
      <FormSection
        label='Physical Address'
      >
        <FormField
          editorFor='physicalAddress.theAddress'
          indicateRequired={ true }
          label='The Address'
        >
          <TextInput
            value={ building?.physicalAddress?.theAddress }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.theAddress = value;
              onChange(newObj);
            } }
          />
        </FormField>
        <StyleControl
          maxWidth={ 400 }
        >
          <FormField
            editorFor='physicalAddress.city'
            indicateRequired={ true }
            label='City'
          >
            <TextInput
              value={ building?.physicalAddress?.city }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.city = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <StyleControl
          maxWidth={ 250 }
        >
          <FormField
            editorFor='physicalAddress.stateOrProvince'
            indicateRequired={ true }
            label='State Or Province'
          >
            <TextInput
              value={ building?.physicalAddress?.stateOrProvince }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.stateOrProvince = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <StyleControl
          maxWidth={ 150 }
        >
          <FormField
            editorFor='physicalAddress.zip'
            indicateRequired={ true }
            label='Zip or Postal Code'
          >
            <TextInput
              value={ building?.physicalAddress?.zip }
              onChange={ (value) => {
                let newObj = JSON.parse(JSON.stringify(building));
                newObj.physicalAddress.zip = value;
                onChange(newObj);
              } }
            />
          </FormField>
        </StyleControl>
        <FormField
          editorFor='physicalAddress.country'
          indicateRequired={ true }
          label='Country'
        >
          <AssociationEditor
            id={ building?.physicalAddress?.country?.id }
            onChange={ (value) => {
              let newObj = JSON.parse(JSON.stringify(building));
              newObj.physicalAddress.country = value == null ? undefined : { id: value };
              onChange(newObj);
            } }
            isNullable={ false }
            query={ countriesQuery }
            toStringRepresentation={ countryToStringRepresentation }
          />
        </FormField>
      </FormSection>
      <FormSection
        label='Mailing Address'
      >
        <Checkbox
          checked={ building?.mailingAddressSameAsPhysical }
          onChange={ (value) => {
            onChange({ ...building, mailingAddressSameAsPhysical: value })
          } }
          label='Mailing Address Same as Physical Address'
        />
        <StyleControl
          visible={ buildingApplicableWhenForMailingAddress(appContext, building) }
        >
          <VerticalStackPanel>
            <FormField
              editorFor='mailingAddress.theAddress'
              indicateRequired={ true }
              label='Address or Post Office (PO) Box'
            >
              <TextInput
                value={ building?.mailingAddress?.theAddress }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.theAddress = value;
                  onChange(newObj);
                } }
              />
            </FormField>
            <StyleControl
              maxWidth={ 400 }
            >
              <FormField
                editorFor='mailingAddress.city'
                indicateRequired={ true }
                label='City'
              >
                <TextInput
                  value={ building?.mailingAddress?.city }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.city = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
            </StyleControl>
            <StyleControl
              maxWidth={ 250 }
            >
              <FormField
                editorFor='mailingAddress.stateOrProvince'
                indicateRequired={ true }
                label='State Or Province'
              >
                <TextInput
                  value={ building?.mailingAddress?.stateOrProvince }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.stateOrProvince = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
            </StyleControl>
            <StyleControl
              maxWidth={ 150 }
            >
              <FormField
                editorFor='mailingAddress.zip'
                indicateRequired={ true }
                label='Zip or Postal Code'
              >
                <TextInput
                  value={ building?.mailingAddress?.zip }
                  onChange={ (value) => {
                    let newObj = JSON.parse(JSON.stringify(building));
                    newObj.mailingAddress.zip = value;
                    onChange(newObj);
                  } }
                />
              </FormField>
            </StyleControl>
            <FormField
              editorFor='mailingAddress.country'
              indicateRequired={ true }
              label='Country'
            >
              <AssociationEditor
                id={ building?.mailingAddress?.country?.id }
                onChange={ (value) => {
                  let newObj = JSON.parse(JSON.stringify(building));
                  newObj.mailingAddress.country = value == null ? undefined : { id: value };
                  onChange(newObj);
                } }
                isNullable={ false }
                query={ countriesQuery }
                toStringRepresentation={ countryToStringRepresentation }
              />
            </FormField>
          </VerticalStackPanel>
        </StyleControl>
      </FormSection>
      <FormSection
        label='Other Details'
      >
        <Group
          align='center'
        >
          <FormField
            editorFor='dateOfOccupancy'
            indicateRequired={ true }
            label='Date Of Occupancy'
          >
            <CalendarDateInput
              value={ building?.dateOfOccupancy }
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
            <FloatDisplay
              value={ buildingAgeInYears(appContext, building) }
            />
          </FormField>
        </Group>
        <FormField
          editorFor='mailboxType'
          indicateRequired={ true }
          label='Mailbox Type'
        >
          <RadioGroup
            value={ building?.mailboxType }
            onChange={ (value) => {
              onChange({ ...building, mailboxType: value })
            } }
            options={ MailboxTypeEnumPairs }
          />
        </FormField>
        <StyleControl
          maxWidth={ 300 }
        >
          <FormField
            editorFor='petPolicy'
            label='Pet Policy'
          >
            <SelectInput
              value={ building?.petPolicy }
              onChange={ (value) => {
                onChange({ ...building, petPolicy: value })
              } }
              options={ PetPolicyEnumPairs }
            />
          </FormField>
        </StyleControl>
      </FormSection>
      <FormSection
        label='Units'
      >
        <MultiStacker
          items={ building?.units || [] }
          onChange={ (value) => {
            onChange({ ...building, units: value })
          } }
          itemDisplayFunc={ (data, onChange, inListIndex) => (
            <VerticalStackPanel>
              <Group
                align='center'
              >
                <FormField
                  editorFor='units.number'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  toolTip='Unit number corresponding to mailing address'
                  label='Number'
                >
                  <TextInput
                    value={ data?.number }
                    onChange={ (value) => {
                      onChange({ ...data, number: value })
                    } }
                    placeholder='Unit number'
                  />
                </FormField>
                <FormField
                  editorFor='units.squareFeet'
                  inListIndex={ inListIndex }
                  label='Square Feet'
                >
                  <FloatInput
                    value={ data?.squareFeet }
                    onChange={ (value) => {
                      onChange({ ...data, squareFeet: value })
                    } }
                  />
                </FormField>
                <FormField
                  editorFor='units.hasBalcony'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='Unit has Blacony?'
                >
                  <Checkbox
                    checked={ data?.hasBalcony }
                    onChange={ (value) => {
                      onChange({ ...data, hasBalcony: value })
                    } }
                  />
                </FormField>
              </Group>
              <Group
                align='center'
              >
                <FormField
                  editorFor='units.numberOfBedrooms'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='Number Of Bedrooms'
                >
                  <IntInput
                    value={ data?.numberOfBedrooms }
                    onChange={ (value) => {
                      onChange({ ...data, numberOfBedrooms: value })
                    } }
                  />
                </FormField>
                <FormField
                  editorFor='units.numberOfBathrooms'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='Number Of Bathrooms'
                >
                  <SelectInput
                    value={ data?.numberOfBathrooms }
                    onChange={ (value) => {
                      onChange({ ...data, numberOfBathrooms: value })
                    } }
                    options={ NumberOfBathroomsEnumPairs }
                  />
                </FormField>
              </Group>
            </VerticalStackPanel>
          ) }
          addItemLabel='Add Unit'
          addNewItem={ createDefaultUnit }
        />
      </FormSection>
      <FormErrorDisplay/>
      <Group
        justifyContent='space-between'
      >
        <TextDisplay
          value='* Required'
        />
        <FormSubmitButton
          mutation={ mutation }
          variables={
            {
              building: {
                id: building.id,
                physicalAddress: building.physicalAddress,
                moniker: building.moniker,
                name: building.name,
                description: building.description,
                mailingAddressSameAsPhysical: building.mailingAddressSameAsPhysical,
                mailingAddress: building.mailingAddress,
                dateOfOccupancy: building.dateOfOccupancy,
                mailboxType: building.mailboxType,
                petPolicy: building.petPolicy,
                units: building.units,
              }
            }
          }
          label='Save'
        />
      </Group>
    </FormProvider>
  );
}

type StatefulProps = {
  readonly building: Building,
};
export function BuildingFormStateful(props: StatefulProps): React.JSX.Element {
  const [editedBuilding, setEditedBuilding] = React.useState(props.building);
  return <BuildingForm
    building={ editedBuilding }
    onChange={ setEditedBuilding }
  />
}

const mutation = gql`
  mutation BuildingFormMutation(
    $building: BuildingFormBuildingInput!
  ) {
    buildingFormUpdateBuilding(
      data: $building
    ) {
      id
      dateOfOccupancy
      description
      mailboxType
      mailingAddress {
        id
        city
        country {
          id
        }
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
        country {
          id
        }
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
  }
`;

export const BUILDINGFORM_BUILDING_FRAGMENT = gql`
  fragment BuildingForm_Building on Building {
    id
    dateOfOccupancy
    description
    mailboxType
    mailingAddress {
      id
      city
      country {
        id
      }
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
      country {
        id
      }
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
`

const countriesQuery = gql`
  query BuildingForm_countriesQuery {
    entities: countries {
      id
      name
      code
    }
  }
`;

