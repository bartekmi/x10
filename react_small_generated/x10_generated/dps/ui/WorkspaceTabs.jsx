// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import TabbedPane from 'react_lib/tab/TabbedPane';
import x10toString from 'react_lib/utils/x10toString';

import { type Hit } from 'dps/entities/Hit';
import EscalationTab from 'dps/ui/EscalationTab';
import HitDetailsTab from 'dps/ui/HitDetailsTab';
import ShipmentTab from 'dps/ui/ShipmentTab';

import { type WorkspaceTabs_hit } from './__generated__/WorkspaceTabs_hit.graphql';



type Props = {|
  +hit: WorkspaceTabs_hit,
|};
function WorkspaceTabs(props: Props): React.Node {
  const { hit } = props;

  return (
    <TabbedPane
      tabs={
        [
          {
            id: 'Hit details',
            label: 'Hit details',
            displayFunc: () =>
              <HitDetailsTab hit={ hit }/>
            ,
          },
          {
            label: 'Shipment ' + x10toString(hit?.shipments?.count),
            displayFunc: () =>
              <ShipmentTab shipment={ hit.shipments }/>
            ,
          },
          {
            label: 'Escalation ' + x10toString(hit?.messages?.count),
            displayFunc: () =>
              <EscalationTab message={ hit.messages }/>
            ,
          },
        ]
      }
    />
  );
}

// $FlowExpectedError
export default createFragmentContainer(WorkspaceTabs, {
  hit: graphql`
    fragment WorkspaceTabs_hit on Hit {
      id
      messages {
        id
        ...EscalationTab_messages
      }
      shipments {
        id
        toStringRepresentation
        ...ShipmentTab_shipments
      }
      ...HitDetailsTab_hit
    }
  `,
});

