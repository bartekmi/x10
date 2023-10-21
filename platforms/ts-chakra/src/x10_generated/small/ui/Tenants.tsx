import { gql } from '@apollo/client';
import { Flex, Heading } from '@chakra-ui/react';
import * as React from 'react';

import Button from 'react_lib/chakra_wrappers/Button';
import TextDisplay from 'react_lib/display/TextDisplay';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Table from 'react_lib/table/Table';
import x10toString from 'react_lib/utils/x10toString';

import { type Tenant } from 'x10_generated/small/entities/Tenant';



type Props = {
  readonly tenants: Tenant,
};
export default function Tenants(props: Props): React.JSX.Element {
  const { tenants } = props;

  return (
    <VerticalStackPanel>
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children='Tenants'
      />
      <Table
        data={ tenants }
        columns={
          [
            {
              id: '_0',
              Header: 'Name',
              width: 140,
              accessor: (data) => data?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_1',
              Header: 'Phone',
              width: 140,
              accessor: (data) => data?.phone,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_2',
              Header: 'Email',
              width: 140,
              accessor: (data) => data?.email,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_3',
              Header: 'Country',
              width: 140,
              accessor: (data) => data?.permanentMailingAddress?.country,
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
                  />
                  <Button
                    label='Edit'
                    url={ '/tenants/edit/' + x10toString(value?.id) }
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
    fragment Tenants_Tenants on Tenant {
      id
      email
      name
      permanentMailingAddress {
        id
        country {
          id
        }
      }
      phone
    }
  `

