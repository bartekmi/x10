// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { type Moves_moves } from './__generated__/Moves_moves.graphql';
import { type Move } from 'entities/Move';
import Group from 'latitude/Group';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';
import Text from 'latitude/Text';
import * as React from 'react';
import Button from 'react_lib/Button';
import { createFragmentContainer, graphql } from 'react-relay';


type Props = {|
  +moves: Moves_moves,
|};
function Moves(props: Props): React.Node {
  const { moves } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='display'
        children='Moves'
      />
      <div style={ { height: '500px', wdith: '100%' } }>
        <Table
          data={ moves }
          getUniqueRowId={ row => row.id }
          columnDefinitions={
            [
              {
                id: 'Date',
                render: (data) => <TextCell value={ data.date } />,
                header: 'Date',
                width: 140,
              },
              {
                id: 'From',
                render: (data) => <TextCell value={ data.from?.name } />,
                header: 'From',
                width: 140,
              },
              {
                id: 'From',
                render: (data) => <TextCell value={ data.to?.name } />,
                header: 'From',
                width: 140,
              },
              {
                id: 'Tenant',
                render: (data) => <TextCell value={ data.tenant?.name } />,
                header: 'Tenant',
                width: 140,
              },
              {
                id: 'Action',
                render: (data) =>
                  <Group>
                    <Button
                      label='View'
                      url={ '/moves/view/' + data.id }
                    />
                    <Button
                      label='Edit'
                      url={ '/moves/edit/' + data.id }
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
export default createFragmentContainer(Moves, {
  moves: graphql`
    fragment Moves_moves on Move @relay(plural: true) {
      id
      date
      from {
        id
        name
      }
      tenant {
        id
        name
      }
      to {
        id
        name
      }
    }
  `,
});

