// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { addressSecondAddressLine } from 'small/entities/Address';
import { buildingAgeInYears, PetPolicyEnumPairs, type Building } from 'small/entities/Building';

import { type Buildings_buildings } from './__generated__/Buildings_buildings.graphql';



type Props = {|
  +buildings: Buildings_buildings,
|};
function Buildings(props: Props): React.Node {
  const { buildings } = props;

  return (
    <VerticalStackPanel>
      <Text
        scale='display'
        weight='bold'
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
                <Group
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
                </Group>
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
                <Group
                  alignItems='center'
                >
                  <Button
                    label='View'
                  />
                  <Button
                    label='Edit'
                    url={ '/buildings/edit/' + x10toString(value?.id) }
                  />
                </Group>
              ,
            },
          ]
        }
      />
    </VerticalStackPanel>
  );
}

// $FlowExpectedError
export default createFragmentContainer(Buildings, {
  buildings: graphql`
    fragment Buildings_buildings on Building @relay(plural: true) {
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
  `,
});

