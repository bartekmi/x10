// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import * as React from 'react';

import Button from 'latitude/button/Button';
import Group from 'latitude/Group';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';
import Text from 'latitude/Text';
import Separator from 'react_lib/Separator';


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
      <Separator/>
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
                    text='View'
                  />
                  <Button
                    text='Edit'
                  />
                </Group>
              ,
              header: 'Action',
              width: 140,
            },
          ]
        }
      />
    </Group>
  );
}

