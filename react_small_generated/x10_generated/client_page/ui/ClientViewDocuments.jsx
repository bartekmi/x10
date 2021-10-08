// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import Table from 'react_lib/table/Table';

import { type CompanyEntity } from 'client_page/entities/CompanyEntity';
import { DocumentTypeEnumPairs } from 'client_page/entities/Document';
import { userName } from 'client_page/entities/User';

import { type ClientViewDocuments_companyEntity } from './__generated__/ClientViewDocuments_companyEntity.graphql';



type Props = {|
  +companyEntity: ClientViewDocuments_companyEntity,
|};
function ClientViewDocuments(props: Props): React.Node {
  const { companyEntity } = props;

  return (
    <Table
      data={ companyEntity?.documents }
      columns={
        [
          {
            id: 0,
            Header: 'Name',
            width: 140,
            accessor: (data) => data,
            Cell: ({ value }) =>
              <Group
                flexDirection='column'
                gap={ 0 }
              >
                <EnumDisplay
                  value={ value?.documentType }
                  options={ DocumentTypeEnumPairs }
                />
                <TextDisplay
                  value={ value?.fileName }
                />
                <TextDisplay
                  value={ value?.name }
                />
              </Group>
            ,
          },
          {
            id: 1,
            Header: 'Uploaded By',
            width: 140,
            accessor: (data) => userName(data?.uploadedBy),
            Cell: ({ value }) =>
              <TextDisplay
                value={ value }
              />
            ,
          },
          {
            id: 2,
            Header: 'Uploaded',
            width: 140,
            accessor: (data) => data?.uploadedTimestamp,
            Cell: ({ value }) =>
              <TimestampDisplay
                value={ value }
              />
            ,
          },
        ]
      }
    />
  );
}

// $FlowExpectedError
export default createFragmentContainer(ClientViewDocuments, {
  companyEntity: graphql`
    fragment ClientViewDocuments_companyEntity on CompanyEntity {
      id
      documents {
        id
        documentType
        fileName
        name
        uploadedBy {
          id
          toStringRepresentation
          firstName
          lastName
        }
        uploadedTimestamp
      }
    }
  `,
});

