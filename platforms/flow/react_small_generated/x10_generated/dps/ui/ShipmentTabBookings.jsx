// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Text from 'latitude/Text';

import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Table from 'react_lib/table/Table';
import { getDate } from 'react_lib/type_helpers/dateFunctions';

import { BookingStgeEnumPairs, bookingTransportationMode, type Booking } from 'dps/entities/Booking';
import { clientUrl } from 'dps/entities/Client';
import { shipmentUrl } from 'dps/entities/Shipment';
import { TransportationModeEnumPairs } from 'dps/sharedEnums';

import { type ShipmentTabBookings_bookings } from './__generated__/ShipmentTabBookings_bookings.graphql';



type Props = {|
  +bookings: ShipmentTabBookings_bookings,
|};
function ShipmentTabBookings(props: Props): React.Node {
  const { bookings } = props;

  return (
    <VerticalStackPanel>
      <Text
        scale='title'
        weight='bold'
        children='Bookings'
      />
      <Table
        data={ bookings }
        columns={
          [
            {
              id: '_0',
              Header: 'Name',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <TextDisplay
                    value={ value?.name }
                  />
                  <Button
                    label={ value?.shipment?.flexId }
                    url={ shipmentUrl(value?.shipment) }
                  />
                </VerticalStackPanel>
              ,
            },
            {
              id: '_1',
              Header: 'Mode',
              width: 140,
              accessor: (data) => bookingTransportationMode(data),
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ TransportationModeEnumPairs }
                />
              ,
            },
            {
              id: '_2',
              Header: 'Cargo Ready Date',
              width: 140,
              accessor: (data) => data?.cargo_ready_date,
              Cell: ({ value }) =>
                <DateDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_3',
              Header: 'Shipper',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Button
                  label={ value?.shipper_entity?.name }
                  url={ clientUrl(value?.shipper_entity?.company?.client) }
                />
              ,
            },
            {
              id: '_4',
              Header: 'Consignee',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Button
                  label={ value?.consignee_entity?.name }
                  url={ clientUrl(value?.consignee_entity?.company?.client) }
                />
              ,
            },
            {
              id: '_5',
              Header: 'Weight (kg)',
              width: 140,
              accessor: (data) => data?.cargo?.metric_weight,
              Cell: ({ value }) =>
                <FloatDisplay
                  value={ value }
                  decimalPlaces={ 2 }
                />
              ,
            },
            {
              id: '_6',
              Header: 'volume (cm)',
              width: 140,
              accessor: (data) => data?.cargo?.metric_volume,
              Cell: ({ value }) =>
                <FloatDisplay
                  value={ value }
                  decimalPlaces={ 3 }
                />
              ,
            },
            {
              id: '_7',
              Header: 'Creation Date',
              width: 140,
              accessor: (data) => getDate(data?.createdAt),
              Cell: ({ value }) =>
                <DateDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_8',
              Header: 'Status',
              width: 140,
              accessor: (data) => data?.booking_stage,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ BookingStgeEnumPairs }
                />
              ,
            },
          ]
        }
      />
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ShipmentTabBookings, {
  bookings: graphql`
    fragment ShipmentTabBookings_bookings on Booking @relay(plural: true) {
      id
      air
      booking_stage
      cargo {
        id
        metric_volume
        metric_weight
      }
      cargo_ready_date
      consignee_entity {
        id
        toStringRepresentation
        company {
          id
          client {
            id
            dbid
          }
        }
        name
      }
      createdAt
      name
      ocean_fcl
      ocean_lcl
      shipment {
        id
        dbid
        flexId
      }
      shipper_entity {
        id
        toStringRepresentation
        company {
          id
          client {
            id
            dbid
          }
        }
        name
      }
      truck_ftl
      truck_ltl
    }
  `,
});

