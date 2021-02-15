// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import Table from 'react_lib/table/Table';

import { ClientPrimaryShipmentRoleEnumPairs, ClientSegmentEnumPairs, ClientStatusEnumPairs, type Client } from 'client_page/entities/Client';

import { type Clients_clients } from './__generated__/Clients_clients.graphql';



type Props = {|
  +clients: Clients_clients,
|};
function Clients(props: Props): React.Node {
  const { clients } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='display'
        children='Clients'
      />
      <Table
        data={ clients }
        columns={
          [
            {
              id: 'Legal Name',
              accessor: (data) => data?.company?.primaryEntity?.legalName,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
              Header: 'Legal Name',
              width: 140,
            },
            {
              id: 'Status',
              accessor: (data) => data?.status,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ ClientStatusEnumPairs }
                />
              ,
              Header: 'Status',
              width: 140,
            },
            {
              id: 'Segment',
              accessor: (data) => data?.segment,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ ClientSegmentEnumPairs }
                />
              ,
              Header: 'Segment',
              width: 140,
            },
            {
              id: 'Primary Shipment Role',
              accessor: (data) => data?.primaryShipmentRole,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ ClientPrimaryShipmentRoleEnumPairs }
                />
              ,
              Header: 'Primary Shipment Role',
              width: 140,
            },
            {
              id: 'Action',
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group>
                  <Button
                    label='View'
                    url={ '/clients/view/' + value?.id }
                  />
                </Group>
              ,
              Header: 'Action',
              width: 140,
            },
          ]
        }
      />
    </Group>
  );
}

// $FlowExpectedError
export default createFragmentContainer(Clients, {
  clients: graphql`
    fragment Clients_clients on Client @relay(plural: true) {
      id
      company {
        id
        primaryEntity {
          id
          legalName
        }
      }
      primaryShipmentRole
      segment
      status
    }
  `,
});

