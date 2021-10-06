// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Text from 'latitude/Text';

import { type Message } from 'dps/entities/Message';

import { type EscalationTab_messages } from './__generated__/EscalationTab_messages.graphql';



type Props = {|
  +messages: EscalationTab_messages,
|};
function EscalationTab(props: Props): React.Node {
  const { messages } = props;

  return (
    <Text
      scale='display'
      children='TBD Escalations'
    />
  );
}

// $FlowExpectedError
export default createFragmentContainer(EscalationTab, {
  messages: graphql`
    fragment EscalationTab_messages on Message @relay(plural: true) {
      id
    }
  `,
});

