// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';

import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Dialog from 'react_lib/modal/Dialog';
import MultiStacker from 'react_lib/multi/MultiStacker';
import StyleControl from 'react_lib/StyleControl';

import { createDefaultDpsMessage, dpsMessageFlexId, dpsMessageShipmentUrl } from 'dps/entities/DpsMessage';
import { type Hit } from 'dps/entities/Hit';

import { type EscalationTab_hit } from './__generated__/EscalationTab_hit.graphql';



type Props = {|
  +hit: EscalationTab_hit,
|};
function EscalationTab(props: Props): React.Node {
  const { hit } = props;

  return (
    <VerticalStackPanel
      gap={ 20 }
    >
      <Group
        alignItems='center'
      >
        <Icon
          iconName='attention'
        />
        <TextDisplay
          value='Ops sent these messages when they escalated this hit'
        />
      </Group>
      <MultiStacker
        items={ hit?.messages }
        itemDisplayFunc={ (data, onChange, inListIndex) => (
          <VerticalStackPanel
            gap={ 4 }
          >
            <Group
              alignItems='center'
              gap={ 20 }
            >
              <Dialog
                title={ data?.user?.name }
                openButton={
                  <Button
                    label={ data?.user?.name }
                    style='link'
                  />
                }
              >
                <StyleControl
                  width={ 350 }
                >
                  <VerticalStackPanel>
                    <DisplayForm>
                      <DisplayField
                        label='Phone'
                      >
                        <TextDisplay
                          value={ data?.user?.phone }
                        />
                      </DisplayField>
                      <DisplayField
                        label='Email'
                      >
                        <TextDisplay
                          value={ data?.user?.email }
                        />
                      </DisplayField>
                      <DisplayField
                        label='Location'
                      >
                        <TextDisplay
                          value={ data?.user?.location }
                        />
                      </DisplayField>
                    </DisplayForm>
                  </VerticalStackPanel>
                </StyleControl>
              </Dialog>
              <TimestampDisplay
                value={ data?.timestamp }
              />
            </Group>
            <TextDisplay
              value={ data?.text }
            />
            <Button
              label={ dpsMessageFlexId(data) }
              url={ dpsMessageShipmentUrl(data) }
            />
          </VerticalStackPanel>
        ) }
        addNewItem={ createDefaultDpsMessage }
      />
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(EscalationTab, {
  hit: graphql`
    fragment EscalationTab_hit on Hit {
      id
      messages {
        id
        coreShipmentId
        text
        timestamp
        user {
          id
          toStringRepresentation
          email
          location
          name
          phone
        }
      }
    }
  `,
});

