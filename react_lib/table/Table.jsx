// @flow

import * as React from 'react';
import { useTable } from "react-table";

import Group from "latitude/Group";

import "./Table.css";

type ColumnDef = {|
  +id: string | number,
  +Header: ?string,
  +width?: number,
  +accessor: (any) => any,
  +Cell: (any) => React.Node,
|};

type Props = {|
  +columns: $ReadOnlyArray<ColumnDef>,
  +data: any,
  +expandedContentFunc?: (any) => React.Node,
|};
export default function Table({ columns, data, expandedContentFunc }: Props): React.Node {
  // Use the useTable Hook to send the columns and data to build the table
  const {
    getTableProps, // table props from react-table
    getTableBodyProps, // table body props from react-table
    headerGroups, // headerGroups, if your table has groupings
    rows, // rows for the table based on the data passed
    prepareRow // Prepare the row (this function needs to be called for each row before getting the row props)
  } = useTable({
    columns,
    data
  });

  let [expandedRowId, setExpandedRowId] = React.useState(null);
  if (!expandedContentFunc) {
    setExpandedRowId = null;
  }

  function renderRows() {
    const rowArray = [];
  
    for (let ii = 0; ii < rows.length; ii++) {
      const row = rows[ii];
      prepareRow(row);
  
      const rowExpanded = expandedContentFunc != null && row.id == expandedRowId;
      rowArray.push(renderRow(row, rowExpanded));
  
      if (rowExpanded && expandedContentFunc) {
        rowArray.push(
          <tr className="expanded" key="expanded">
            <td colSpan={row.cells.length}>{expandedContentFunc(row.original)}</td>
          </tr>
        );
      }
    }
  
    return rowArray;
  }

  function renderRow(row, rowExpanded) {
    let rowProps = row.getRowProps();
  
    if (setExpandedRowId) {
      rowProps = {
        ...rowProps,
        onClick: () => {
          if (setExpandedRowId)
            setExpandedRowId(expandedRowId == row.id ? null : row.id);
        },
        className: rowExpanded ? "expanded clickable" : "clickable"
      }
    }
  
    return (
      <tr {...rowProps}>
        {row.cells.map(cell => {
          return <td {...cell.getCellProps()}>{cell.render("Cell")}</td>;
        })}
      </tr>
    );
  }
  
  /* 
    Render the UI for your table
    - react-table doesn't have UI, it's headless. We just need to put the react-table props from the Hooks, 
      and it will do its magic automatically
  */
  return (
    <table {...getTableProps()}>
      <thead>
        {headerGroups.map(headerGroup => (
          <tr {...headerGroup.getHeaderGroupProps()}>
            {headerGroup.headers.map(column => (
              <th {...column.getHeaderProps()}>{column.render("Header")}</th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody {...getTableBodyProps()}>
        {renderRows()}
      </tbody>
    </table>
  );
}
