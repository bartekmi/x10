import { gql } from '@apollo/client';
import { Flex, Heading } from '@chakra-ui/react';
import * as React from 'react';

import Button from 'react_lib/chakra_wrappers/Button';
import DateDisplay from 'react_lib/display/DateDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import x10toString from 'react_lib/utils/x10toString';

import { type Move } from 'x10_generated/small/entities/Move';



type Props = {
  readonly moves: Move,
};
export default function Moves(props: Props): React.JSX.Element {
  const { moves } = props;

  return (
    <VerticalStackPanel>
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children='Moves'
      />
      <Table
        data={ moves }
        columns={
          [
            {
              id: '_0',
              Header: 'Date',
              width: 140,
              accessor: (data) => data?.date,
              Cell: ({ value }) =>
                <DateDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_1',
              Header: 'From',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <StyleControl
                  maxWidth={ 350 }
                >
                  <TextDisplay
                    value={ value?.from?.name }
                  />
                </StyleControl>
              ,
            },
            {
              id: '_2',
              Header: 'To',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <StyleControl
                  maxWidth={ 350 }
                >
                  <TextDisplay
                    value={ value?.to?.name }
                  />
                </StyleControl>
              ,
            },
            {
              id: '_3',
              Header: 'Tenant',
              width: 140,
              accessor: (data) => data?.tenant?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_4',
              Header: 'Action',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Flex
                  alignItems='center'
                >
                  <Button
                    label='View'
                    url={ '/moves/view/' + x10toString(value?.id) }
                  />
                  <Button
                    label='Edit'
                    url={ '/moves/edit/' + x10toString(value?.id) }
                  />
                </Flex>
              ,
            },
          ]
        }
      />
    </VerticalStackPanel>
  );
}

  gql`
    fragment Moves_Moves on Move {
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
  `

