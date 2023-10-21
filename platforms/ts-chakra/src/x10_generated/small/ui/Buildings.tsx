import { gql } from '@apollo/client';
import { Flex, Heading } from '@chakra-ui/react';
import * as React from 'react';

import Button from 'react_lib/chakra_wrappers/Button';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import HelpTooltip from 'react_lib/form/HelpTooltip';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { addressSecondAddressLine } from 'x10_generated/small/entities/Address';
import { buildingAgeInYears, PetPolicyEnumPairs, type Building } from 'x10_generated/small/entities/Building';

import { Buildings_BuildingsFragment } from '__generated__/graphql';



type Props = {
  readonly buildings: Buildings_BuildingsFragment,
};
function Buildings(props: Props): React.JSX.Element {
  const { buildings } = props;

  return (
    <VerticalStackPanel>
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children='Buildings'
      />
      <Table
        data={ buildings }
        columns={
          [
            {
              id: '_0',
              Header: 'Name',
              width: 200,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Flex
                  alignItems='center'
                >
                  <StyleControl
                    maxWidth={ 350 }
                  >
                    <TextDisplay
                      value={ value?.name }
                    />
                  </StyleControl>
                  <StyleControl
                    visible={ !isBlank(value?.description) }
                  >
                    <HelpTooltip
                      text={ value?.description }
                    />
                  </StyleControl>
                </Flex>
              ,
            },
            {
              id: '_1',
              Header: 'The Address',
              width: 140,
              accessor: (data) => data?.physicalAddress?.theAddress,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_2',
              Header: 'City / Province',
              width: 140,
              accessor: (data) => addressSecondAddressLine(data?.physicalAddress),
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_3',
              Header: 'Age In Years',
              width: 140,
              accessor: (data) => buildingAgeInYears(data),
              Cell: ({ value }) =>
                <FloatDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_4',
              Header: 'Pet Policy',
              width: 140,
              accessor: (data) => data?.petPolicy,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ PetPolicyEnumPairs }
                />
              ,
            },
            {
              id: '_5',
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
                    url={ '/buildings/edit/' + x10toString(value?.id) }
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
    fragment Buildings_Buildings on Building {
      id
      dateOfOccupancy
      description
      name
      petPolicy
      physicalAddress {
        id
        city
        stateOrProvince
        theAddress
      }
    }
  `

