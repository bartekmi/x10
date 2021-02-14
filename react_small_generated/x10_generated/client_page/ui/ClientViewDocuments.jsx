// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';

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
    <div style={ { height: '500px', wdith: '100%' } }>
      <Table
        data={ companyEntity?.documents }
        getUniqueRowId={ row => row.id }
        useFullWidth={ true }
        columnDefinitions={
          [
            {
              id: 'Name',
              render: (data) =>
                <Group
                  flexDirection='column'
                >
                  <EnumDisplay
                    value={ data?.documentType }
                    options={ DocumentTypeEnumPairs }
                  />
                  <TextDisplay
                    value={ data?.fileName }
                  />
                  <TextDisplay
                    value={ data?.name }
                  />
                </Group>
              ,
              header: 'Name',
              width: 140,
            },
            {
              id: 'Name',
              render: (data) => <TextCell value={ userName(data?.uploadedBy) } />,
              header: 'Name',
              width: 140,
            },
            {
              id: 'Uploaded Timestamp',
              render: (data) => <TextCell value={ data?.uploadedTimestamp } />,
              header: 'Uploaded Timestamp',
              width: 140,
            },
          ]
        }
      />
    </div>
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
          firstName
          lastName
        }
        uploadedTimestamp
      }
    }
  `,
});
