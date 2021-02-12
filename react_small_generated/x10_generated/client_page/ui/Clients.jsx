// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';
import Text from 'latitude/Text';

import Button from 'react_lib/latitude_wrappers/Button';

import { type Client } from 'client_page/entities/Client';

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
      <div style={ { height: '500px', wdith: '100%' } }>
        <Table
          data={ clients }
          getUniqueRowId={ row => row.id }
          useFullWidth={ true }
          columnDefinitions={
            [
              {
                id: 'Legal Name',
                render: (data) => <TextCell value={ data.company?.primaryEntity?.legalName } />,
                header: 'Legal Name',
                width: 140,
              },
              {
                id: 'Status',
                render: (data) => <TextCell value={ data.status } />,
                header: 'Status',
                width: 140,
              },
              {
                id: 'Segment',
                render: (data) => <TextCell value={ data.segment } />,
                header: 'Segment',
                width: 140,
              },
              {
                id: 'Primary Shipment Role',
                render: (data) => <TextCell value={ data.primaryShipmentRole } />,
                header: 'Primary Shipment Role',
                width: 140,
              },
              {
                id: 'Action',
                render: (data) =>
                  <Group>
                    <Button
                      label='View'
                      url={ '/clients/view/' + data.id }
                    />
                  </Group>
                ,
                header: 'Action',
                width: 140,
              },
            ]
          }
        />
      </div>
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
