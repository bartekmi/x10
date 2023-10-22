import { gql } from '@apollo/client';
import { Flex } from '@chakra-ui/react';
import * as React from 'react';

import TextareaInput from 'react_lib/chakra_wrappers/TextareaInput';
import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';

import { AppContext } from 'SmallAppContext';
import { buildingAgeInYears, MailboxTypeEnumPairs, PetPolicyEnumPairs, type Building } from 'x10_generated/small/entities/Building';



type Props = {
  readonly building?: Building,
};
export default function BuildingView(props: Props): React.JSX.Element {
  const { building } = props;
  const appContext = React.useContext(AppContext);

  return (
    <DisplayForm>
      <VerticalStackPanel>
        <Flex
          gap={ 40 }
        >
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
          <DisplayField
            toolTip='Description for advertising purposes'
            label='Description'
          >
            <TextareaInput
              value={ building?.description }
              readOnly={ true }
            />
          </DisplayField>
        </Flex>
        <Flex
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
        </Flex>
      </VerticalStackPanel>
    </DisplayForm>
  );
}

  gql`
    fragment BuildingView_Building on Building {
      id
      dateOfOccupancy
      description
      mailboxType
      name
      petPolicy
    }
  `

