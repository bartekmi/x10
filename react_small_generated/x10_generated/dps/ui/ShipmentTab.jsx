// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';

import { type Hit } from 'dps/entities/Hit';
import ShipmentTabBookings from 'dps/ui/ShipmentTabBookings';
import ShipmentTabQuotes from 'dps/ui/ShipmentTabQuotes';
import ShipmentTabShipments from 'dps/ui/ShipmentTabShipments';

import { type ShipmentTab_hit } from './__generated__/ShipmentTab_hit.graphql';



type Props = {|
  +hit: ShipmentTab_hit,
|};
function ShipmentTab(props: Props): React.Node {
  const { hit } = props;

  return (
    <VerticalStackPanel
      gap={ 32 }
    >
      <ShipmentTabShipments shipments={ hit.shipments }/>
      <ShipmentTabQuotes quotes={ hit.quotes }/>
      <ShipmentTabBookings bookings={ hit.bookings }/>
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ShipmentTab, {
  hit: graphql`
    fragment ShipmentTab_hit on Hit {
      id
      bookings {
        id
        ...ShipmentTabBookings_bookings
      }
      quotes {
        id
        ...ShipmentTabQuotes_quotes
      }
      shipments {
        id
        ...ShipmentTabShipments_shipments
      }
    }
  `,
});

