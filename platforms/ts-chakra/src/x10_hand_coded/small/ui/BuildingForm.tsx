import * as React from 'react';
import { gql } from '@apollo/client';

import { Flex, Textarea as TextareaInput } from '@chakra-ui/react'

import TextDisplay from '../../../react_lib/display/TextDisplay';
import FormField from '../../../react_lib/form/FormField';
import EditForm from '../../../react_lib/form/FormProvider';
import FormSection from '../../../react_lib/form/FormSection';
import FormSubmitButton from '../../../react_lib/form/FormSubmitButton';
import TextInput from '../../../react_lib/chakra_wrappers/TextInput';
import VerticalStackPanel from '../../../react_lib/layout/VerticalStackPanel';
import StyleControl from '../../../react_lib/StyleControl';

import { buildingErrorName } from '../entities/Building';

import { BuildingForm_BuildingFragment } from "../../../__generated__/graphql";

type Props = {
  readonly building: BuildingForm_BuildingFragment,
  readonly onChange: (building: BuildingForm_BuildingFragment) => void,
};
export default function BuildingForm(props: Props): React.JSX.Element {
  const { building, onChange } = props;

  return (
    <EditForm
      context={ { errors: [buildingErrorName(building.name)] } }
    >
      <VerticalStackPanel>
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
              onChange={ (e) => {
                onChange({ ...building, description: e.target.value })
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
                building: {
                  id: building.id,
                  physicalAddress: building.physicalAddress,
                  name: building.name,
                  description: building.description,
                }
              }
            }
            label='Save'
          />
        </Flex>
      </VerticalStackPanel>
    </EditForm>
  );
}

type StatefulProps = {
  readonly building: BuildingForm_BuildingFragment,
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
      description
      moniker
      name
      physicalAddress {
        id
        city
        stateOrProvince
        theAddress
        zip
      }
    }
  }
`;

gql`
    fragment BuildingForm_building on Building {
      id
      description
      moniker
      name
      physicalAddress {
        id
        city
        stateOrProvince
        theAddress
        zip
      }
    }
  `
