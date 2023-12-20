// Auto-generated by x10 - do not edit
import { gql } from '@apollo/client';
import * as React from 'react';

import Checkbox from 'react_lib/chakra_wrappers/Checkbox';
import TextareaInput from 'react_lib/chakra_wrappers/TextareaInput';
import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import FormSection from 'react_lib/form/FormSection';
import Group from 'react_lib/Group';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import MultiStacker from 'react_lib/multi/MultiStacker';
import StyleControl from 'react_lib/StyleControl';
import x10toString from 'react_lib/utils/x10toString';

import { AppContext } from 'SmallAppContext';
import { buildingAgeInYears, MailboxTypeEnumPairs, PetPolicyEnumPairs, type Building } from 'x10_generated/small/entities/Building';
import { NumberOfBathroomsEnumPairs } from 'x10_generated/small/entities/Unit';



type Props = {
  readonly building?: Building,
};
export default function BuildingView(props: Props): React.JSX.Element {
  const { building } = props;
  const appContext = React.useContext(AppContext);

  return (
    <DisplayForm>
      <VerticalStackPanel>
        <FormSection
          label='Building Details'
        >
          <Group
            gap={ 40 }
          >
            <DisplayField
              label='Moniker'
            >
              <TextDisplay
                value={ building?.moniker }
              />
            </DisplayField>
            <StyleControl
              maxWidth={ 350 }
            >
              <DisplayField
                toolTip='A short and memorable name of the Building'
                label='Name'
              >
                <TextDisplay
                  value={ building?.name }
                />
              </DisplayField>
            </StyleControl>
          </Group>
          <DisplayField
            toolTip='Description for advertising purposes'
            label='Description'
          >
            <TextareaInput
              value={ building?.description }
              readOnly={ true }
            />
          </DisplayField>
          <Group
            gap={ 40 }
          >
            <DisplayField
              label='Date Of Occupancy'
            >
              <DateDisplay
                value={ building?.dateOfOccupancy }
              />
            </DisplayField>
            <DisplayField
              toolTip='Strongly affects annual propery maintenance costs.'
              label='Age In Years'
            >
              <FloatDisplay
                value={ buildingAgeInYears(appContext, building) }
              />
            </DisplayField>
            <DisplayField
              label='Mailbox Type'
            >
              <EnumDisplay
                value={ building?.mailboxType }
                options={ MailboxTypeEnumPairs }
              />
            </DisplayField>
            <DisplayField
              label='Pet Policy'
            >
              <EnumDisplay
                value={ building?.petPolicy }
                options={ PetPolicyEnumPairs }
              />
            </DisplayField>
          </Group>
        </FormSection>
        <FormSection
          label='Physical Address'
        >
          <DisplayField
            label='The Address'
          >
            <TextDisplay
              value={ building?.physicalAddress?.theAddress }
            />
          </DisplayField>
          <TextDisplay
            value={ x10toString(building?.physicalAddress?.city) + ', ' + x10toString(building?.physicalAddress?.stateOrProvince) + '   ' + x10toString(building?.physicalAddress?.zip) }
          />
          <TextDisplay
            value={ building?.physicalAddress?.country?.name }
          />
        </FormSection>
        <FormSection
          label='Units'
        >
          <MultiStacker
            items={ building?.units }
            itemDisplayFunc={ (data, onChange, inListIndex) => (
              <VerticalStackPanel>
                <Group
                  align='center'
                >
                  <DisplayField
                    toolTip='Unit number corresponding to mailing address'
                    label='Number'
                  >
                    <TextDisplay
                      value={ data?.number }
                    />
                  </DisplayField>
                  <DisplayField
                    label='Square Feet'
                  >
                    <FloatDisplay
                      value={ data?.squareFeet }
                    />
                  </DisplayField>
                  <DisplayField
                    label='Unit has Blacony?'
                  >
                    <Checkbox
                      checked={ data?.hasBalcony }
                      disabled={ true }
                    />
                  </DisplayField>
                </Group>
                <Group
                  align='center'
                >
                  <DisplayField
                    label='Number Of Bedrooms'
                  >
                    <FloatDisplay
                      value={ data?.numberOfBedrooms }
                    />
                  </DisplayField>
                  <DisplayField
                    label='Number Of Bathrooms'
                  >
                    <EnumDisplay
                      value={ data?.numberOfBathrooms }
                      options={ NumberOfBathroomsEnumPairs }
                    />
                  </DisplayField>
                </Group>
              </VerticalStackPanel>
            ) }
          />
        </FormSection>
      </VerticalStackPanel>
    </DisplayForm>
  );
}

export const BUILDINGVIEW_BUILDING_FRAGMENT = gql`
  fragment BuildingView_Building on Building {
    id
    dateOfOccupancy
    description
    mailboxType
    moniker
    name
    petPolicy
    physicalAddress {
      id
      city
      country {
        id
        name
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

