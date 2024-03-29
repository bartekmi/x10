// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';

import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import toEnum from 'react_lib/utils/toEnum';

import { clientUrl } from 'dps/entities/Client';
import { portCityAndCountry } from 'dps/entities/Port';
import { shipmentModeSubtext, shipmentUrl, type Shipment } from 'dps/entities/Shipment';
import { TransportationModeEnumPairs } from 'dps/sharedEnums';

import { type ShipmentTabShipments_shipments } from './__generated__/ShipmentTabShipments_shipments.graphql';



type Props = {|
  +shipments: ShipmentTabShipments_shipments,
|};
function ShipmentTabShipments(props: Props): React.Node {
  const { shipments } = props;

  return (
    <VerticalStackPanel>
      <Text
        scale='title'
        weight='bold'
        children='Shipments'
      />
      <Table
        data={ shipments }
        columns={
          [
            {
              id: '_0',
              Header: 'Id',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group
                  alignItems='center'
                >
                  <Button
                    label={ value?.flexId }
                    url={ shipmentUrl(value) }
                  />
                  <StyleControl
                    visible={ toEnum(value?.priority) == "high" }
                  >
                    <Icon
                      iconName='lightning'
                    />
                  </StyleControl>
                </Group>
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
                  url={ clientUrl(value?.consignee?.company?.client) }
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
                  url={ clientUrl(value?.shipper?.company?.client) }
                />
              ,
            },
            {
              id: '_4',
              Header: 'Mode',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <EnumDisplay
                    value={ value?.transportationMode }
                    hideLabelIfIconPresent={ true }
                    options={ TransportationModeEnumPairs }
                  />
                  <TextDisplay
                    value={ shipmentModeSubtext(value) }
                  />
                </VerticalStackPanel>
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
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <DateDisplay
                    value={ value?.actualDepartureDate }
                  />
                  <TextDisplay
                    value={ portCityAndCountry(value?.departurePort) }
                  />
                </VerticalStackPanel>
              ,
            },
            {
              id: '_9',
              Header: 'Arrives',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <DateDisplay
                    value={ value?.arrivalDate }
                  />
                  <TextDisplay
                    value={ portCityAndCountry(value?.arrivalPort) }
                  />
                </VerticalStackPanel>
              ,
            },
            {
              id: '_10',
              Header: 'Due Date',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <TextDisplay
                    value={ value?.dueDateTask }
                  />
                  <DateDisplay
                    weight='bold'
                    value={ value?.dueDate }
                  />
                </VerticalStackPanel>
              ,
            },
          ]
        }
      />
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ShipmentTabShipments, {
  shipments: graphql`
    fragment ShipmentTabShipments_shipments on Shipment @relay(plural: true) {
      id
      actualDepartureDate
      arrivalDate
      arrivalPort {
        id
        toStringRepresentation
        city
        country_name
      }
      cargoReadyDate
      consignee {
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
      customs
      dbid
      departurePort {
        id
        toStringRepresentation
        city
        country_name
      }
      dueDate
      dueDateTask
      flexId
      isLcl
      isLtl
      name
      priority
      shipper {
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
      status
      transportationMode
    }
  `,
});

