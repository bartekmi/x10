// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { type BuildingView_building } from './__generated__/BuildingView_building.graphql';
import { buildingAgeInYears, MailboxTypeEnumPairs, PetPolicyEnumPairs, type Building } from 'entities/Building';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import SelectInput from 'latitude/select/SelectInput';
import TextareaInput from 'latitude/TextareaInput';
import * as React from 'react';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import CalendarDateInput from 'react_lib/latitude_wrappers/CalendarDateInput';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import { createFragmentContainer, graphql } from 'react-relay';


type Props = {|
  +building: BuildingView_building,
|};
function BuildingView(props: Props): React.Node {
  const { building } = props;

  return (
    <DisplayForm>
      <Group
        flexDirection='column'
      >
        <Group
          gap={ 40 }
        >
          <DisplayField
            toolTip='A short and memorable name of the Building'
            label='Name'
            maxWidth={ 350 }
          >
            <TextInput
              value={ building.name }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            toolTip='Description for advertising purposes'
            label='Description'
          >
            <TextareaInput
              value={ building.description }
              onChange={ () => { } }
              rows={ 3 }
              readOnly={ true }
            />
          </DisplayField>
        </Group>
        <Group
          gap={ 40 }
        >
          <DisplayField
            label='Date Of Occupancy'
          >
            <CalendarDateInput
              value={ building.dateOfOccupancy }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            toolTip='Strongly affects annual propery maintenance costs.'
            label='Age In Years'
          >
            <FloatInput
              value={ buildingAgeInYears(building) }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            label='Mailbox Type'
          >
            <SelectInput
              value={ building.mailboxType }
              onChange={ () => { } }
              options={ MailboxTypeEnumPairs }
            />
          </DisplayField>
          <DisplayField
            label='Pet Policy'
          >
            <SelectInput
              value={ building.petPolicy }
              onChange={ () => { } }
              options={ PetPolicyEnumPairs }
            />
          </DisplayField>
        </Group>
      </Group>
    </DisplayForm>
  );
}

// $FlowExpectedError
export default createFragmentContainer(BuildingView, {
  building: graphql`
    fragment BuildingView_building on Building {
      id
      dateOfOccupancy
      description
      mailboxType
      name
      petPolicy
    }
  `,
});

