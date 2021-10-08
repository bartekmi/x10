// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import MultiStacker from 'react_lib/multi/MultiStacker';

import { type Hit } from 'dps/entities/Hit';
import { createDefaultMessage, messageFlexId } from 'dps/entities/Message';
import { userName } from 'dps/entities/User';

import { type EscalationTab_hit } from './__generated__/EscalationTab_hit.graphql';



type Props = {|
  +hit: EscalationTab_hit,
|};
function EscalationTab(props: Props): React.Node {
  const { hit } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='headline'
        children='Ops sent these messages when they escalated this hit'
      />
      <MultiStacker
        items={ hit?.messages }
        itemDisplayFunc={ (data, onChange) => (
          <Group
            flexDirection='column'
          >
            <Group>
              <TextDisplay
                value={ userName(data?.user) }
              />
              <TimestampDisplay
                value={ data?.timestamp }
              />
            </Group>
            <TextDisplay
              value={ data?.text }
            />
            <TextDisplay
              value={ messageFlexId(data) }
            />
          </Group>
        ) }
        addNewItem={ createDefaultMessage }
      />
    </Group>
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
          firstName
          lastName
        }
      }
    }
  `,
});

