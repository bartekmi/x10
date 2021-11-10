// TEAM: compliance
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
            id: 0,
            label: 'Hit details',
            displayFunc: () =>
              <HitDetailsTab hit={ hit }/>
            ,
          },
          {
            id: 1,
            label: 'Shipment(' + x10toString(hit?.shipments.length) + ')',
            displayFunc: () =>
              <ShipmentTab hit={ hit }/>
            ,
          },
          {
            id: 2,
            label: 'Escalation(' + x10toString(hit?.messages.length) + ')',
            displayFunc: () =>
              <EscalationTab hit={ hit }/>
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
      }
      shipments {
        id
      }
      ...HitDetailsTab_hit
      ...ShipmentTab_hit
      ...EscalationTab_hit
    }
  `,
});

