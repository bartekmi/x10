// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';

import Group from 'latitude/Group';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';
import Text from 'latitude/Text';
import Button from 'react_lib/Button';


import { type Tenant } from 'entities/Tenant';

type Props = {|
  +tenants: $ReadOnlyArray<Tenant>,
|};
export default function Tenants(props: Props): React.Node {
  const { tenants } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='display'
        children='Tenants'
      />
      <div style={ { height: '500px', wdith: '100%' } }>
        <Table
          data={ tenants }
          getUniqueRowId={ row => row.id }
          columnDefinitions={
            [
              {
                id: 'Name',
                render: (data) => <TextCell value={ data.name } />,
                header: 'Name',
                width: 140,
              },
              {
                id: 'Phone',
                render: (data) => <TextCell value={ data.phone } />,
                header: 'Phone',
                width: 140,
              },
              {
                id: 'Email',
                render: (data) => <TextCell value={ data.email } />,
                header: 'Email',
                width: 140,
              },
              {
                id: 'Action',
                render: (data) =>
                  <Group>
                    <Button
                      label='View'
                    />
                    <Button
                      label='Edit'
                      url={ '/tenants/edit/' + data.dbid }
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

