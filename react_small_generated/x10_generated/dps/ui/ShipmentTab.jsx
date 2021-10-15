// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';

import { companyEntityUrl } from 'dps/entities/CompanyEntity';
import { type Hit } from 'dps/entities/Hit';
import { portCityAndCountry } from 'dps/entities/Port';
import { shipmentUrl, TransportationModeEnumPairs } from 'dps/entities/Shipment';

import { type ShipmentTab_hit } from './__generated__/ShipmentTab_hit.graphql';



type Props = {|
  +hit: ShipmentTab_hit,
|};
function ShipmentTab(props: Props): React.Node {
  const { hit } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='title'
        weight='bold'
        children='Shipments'
      />
      <Table
        data={ hit?.shipments }
        columns={
          [
            {
              id: '_0',
              Header: 'Id',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Button
                  label={ value?.flexId }
                  url={ shipmentUrl(value) }
                />
              ,
            },
            {
              id: '_1',
              Header: 'Name',
              width: 140,
              accessor: (data) => data?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_2',
              Header: 'Consignee',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Button
                  label={ value?.consignee?.name }
                  url={ companyEntityUrl(value?.consignee) }
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
                  label={ value?.shipper?.name }
                  url={ companyEntityUrl(value?.shipper) }
                />
              ,
            },
            {
              id: '_4',
              Header: 'Mode',
              width: 140,
              accessor: (data) => data?.transportationMode,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ TransportationModeEnumPairs }
                />
              ,
            },
            {
              id: '_5',
              Header: 'Status',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <StyleControl
                  paddingTop={ 5 }
                  paddingRight={ 20 }
                  paddingBottom={ 5 }
                  paddingLeft={ 20 }
                  fillColor='black'
                >
                  <TextDisplay
                    weight='bold'
                    textColor='white'
                    value={ value?.status }
                  />
                </StyleControl>
              ,
            },
            {
              id: '_6',
              Header: 'Customs',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <StyleControl
                  visible={ value?.customs != '' }
                  paddingTop={ 5 }
                  paddingRight={ 20 }
                  paddingBottom={ 5 }
                  paddingLeft={ 20 }
                  fillColor='#b22222'
                >
                  <TextDisplay
                    weight='bold'
                    textColor='white'
                    value={ value?.customs }
                  />
                </StyleControl>
              ,
            },
            {
              id: '_7',
              Header: 'Cargo Ready',
              width: 140,
              accessor: (data) => data?.cargoReadyDate,
              Cell: ({ value }) =>
                <DateDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_8',
              Header: 'Departs',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group
                  flexDirection='column'
                  gap={ 0 }
                >
                  <DateDisplay
                    value={ value?.actualDepartureDate }
                  />
                  <TextDisplay
                    value={ portCityAndCountry(value?.departurePort) }
                  />
                </Group>
              ,
            },
            {
              id: '_9',
              Header: 'Arrives',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group
                  flexDirection='column'
                  gap={ 0 }
                >
                  <DateDisplay
                    value={ value?.arrivalDate }
                  />
                  <TextDisplay
                    value={ portCityAndCountry(value?.arrivalPort) }
                  />
                </Group>
              ,
            },
            {
              id: '_10',
              Header: 'Due Date',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group
                  flexDirection='column'
                  gap={ 0 }
                >
                  <TextDisplay
                    value={ value?.dueDateTask }
                  />
                  <DateDisplay
                    weight='bold'
                    value={ value?.dueDate }
                  />
                </Group>
              ,
            },
          ]
        }
      />
    </Group>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ShipmentTab, {
  hit: graphql`
    fragment ShipmentTab_hit on Hit {
      id
      shipments {
        id
        actualDepartureDate
        arrivalDate
        arrivalPort {
          id
          toStringRepresentation
          city
          countryName
        }
        cargoReadyDate
        consignee {
          id
          toStringRepresentation
          clientId
          name
        }
        customs
        dbid
        departurePort {
          id
          toStringRepresentation
          city
          countryName
        }
        dueDate
        dueDateTask
        flexId
        name
        shipper {
          id
          toStringRepresentation
          clientId
          name
        }
        status
        transportationMode
      }
    }
  `,
});

