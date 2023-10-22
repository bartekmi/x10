import * as React from 'react';
import { Text } from '@chakra-ui/react'

import {
  Table as ChakraTable,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
} from '@chakra-ui/react'

type ColumnDef = {
  readonly id: string | number,
  readonly Header?: string,
  readonly width?: number,
  readonly accessor: (row: any) => any,
  readonly Cell: (value: any) => React.JSX.Element,
};

type Props = {
  readonly columns: ColumnDef[],
  readonly data?: any[],
};

export default function Table({ columns, data }: Props): React.JSX.Element {
  if (data == null) {
    return (<Text>No Data</Text>);
  }

  return (
    <ChakraTable>
      <Thead>
        <Tr>
          {columns.map(col => <Th>{col.Header}</Th>)}
        </Tr>
      </Thead>
      <Tbody> 
        {
          data.map(row => <Tr>
            { 
              columns.map(col => <Td> 
                { col.Cell(col.accessor(row)) } 
              </Td>)
            }
          </Tr>)
        } 
      </Tbody>
    </ChakraTable>
  );
}
