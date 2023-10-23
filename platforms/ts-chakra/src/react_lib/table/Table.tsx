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

export default function Table(props: Props): React.JSX.Element {
  const { columns, data } = props;
  if (data == null) {
    return (<Text>No Data</Text>);
  }

  function tdContents(row: any, col: ColumnDef) {
    const data = col.accessor(row);
    return data == null ?
      null :
      col.Cell(col.accessor(row)) 
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
                { tdContents(row, col) }  
              </Td>)
            }
          </Tr>)
        } 
      </Tbody>
    </ChakraTable>
  );
}
