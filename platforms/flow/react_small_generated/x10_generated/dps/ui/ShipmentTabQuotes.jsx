// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';

import { quoteFlexId, quoteUrl, type Quote } from 'dps/entities/Quote';
import { TransportationModeEnumPairs } from 'dps/sharedEnums';

import { type ShipmentTabQuotes_quotes } from './__generated__/ShipmentTabQuotes_quotes.graphql';



type Props = {|
  +quotes: ShipmentTabQuotes_quotes,
|};
function ShipmentTabQuotes(props: Props): React.Node {
  const { quotes } = props;

  return (
    <VerticalStackPanel>
      <Text
        scale='title'
        weight='bold'
        children='Quotes'
      />
      <Table
        data={ quotes }
        columns={
          [
            {
              id: '_0',
              Header: 'Id',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Button
                  label={ quoteFlexId(value) }
                  url={ quoteUrl(value) }
                />
              ,
            },
            {
              id: '_1',
              Header: 'Client Name',
              width: 140,
              accessor: (data) => data?.client?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_2',
              Header: 'Shipment Name',
              width: 140,
              accessor: (data) => data?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_3',
              Header: 'Mode',
              width: 140,
              accessor: (data) => data?.transportationMode,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  hideLabelIfIconPresent={ true }
                  options={ TransportationModeEnumPairs }
                />
              ,
            },
            {
              id: '_4',
              Header: 'From',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <TextDisplay
                    value={ value?.departurePort?.name }
                  />
                  <TextDisplay
                    value={ value?.departurePort?.country_name }
                  />
                </VerticalStackPanel>
              ,
            },
            {
              id: '_5',
              Header: 'To',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <VerticalStackPanel
                  gap={ 0 }
                >
                  <TextDisplay
                    value={ value?.arrivalPort?.name }
                  />
                  <TextDisplay
                    value={ value?.arrivalPort?.country_name }
                  />
                </VerticalStackPanel>
              ,
            },
            {
              id: '_6',
              Header: 'Status',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <StyleControl
                  paddingTop={ 5 }
                  paddingRight={ 20 }
                  paddingBottom={ 5 }
                  paddingLeft={ 20 }
                  fillColor='#b22222'
                >
                  <TextDisplay
                    weight='bold'
                    textColor='white'
                    value={ value?.status }
                  />
                </StyleControl>
              ,
            },
          ]
        }
      />
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(ShipmentTabQuotes, {
  quotes: graphql`
    fragment ShipmentTabQuotes_quotes on Quote @relay(plural: true) {
      id
      arrivalPort {
        id
        toStringRepresentation
        country_name
        name
      }
      client {
        id
        toStringRepresentation
        name
      }
      dbid
      departurePort {
        id
        toStringRepresentation
        country_name
        name
      }
      name
      status
      transportationMode
    }
  `,
});

