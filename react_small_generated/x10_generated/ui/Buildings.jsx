// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';

import { addressSecondAddressLine } from 'entities/Address';
import { buildingAgeInYears } from 'entities/Building';
import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';
import Table from 'latitude/table/Table';
import TextCell from 'latitude/table/TextCell';
import Text from 'latitude/Text';
import TextInput from 'latitude/TextInput';
import Button from 'react_lib/Button';
import Separator from 'react_lib/Separator';
import isBlank from 'react_lib/utils/isBlank';
import VisibilityControl from 'react_lib/VisibilityControl';


import { type Building } from 'entities/Building';

type Props = {|
  +buildings: $ReadOnlyArray<Building>,
|};
export default function Buildings(props: Props): React.Node {
  const { buildings } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='display'
        children='Buildings'
      />
      <Separator/>
      <div style={ { height: '500px', wdith: '100%' } }>
        <Table
          data={ buildings }
          getUniqueRowId={ row => row.id }
          columnDefinitions={
            [
              {
                id: 'Name',
                render: (data) =>
                  <Group>
                    <TextInput
                      value={ data.name }
                      onChange={ () => { } }
                      readOnly={ true }
                    />
                    <VisibilityControl
                      visible={ !isBlank(data.description) }
                    >
                      <HelpTooltip
                        text={ data.description }
                      />
                    </VisibilityControl>
                  </Group>
                ,
                header: 'Name',
                width: 200,
              },
              {
                id: 'The Address',
                render: (data) => <TextCell value={ data.physicalAddress?.theAddress } />,
                header: 'The Address',
                width: 140,
              },
              {
                id: 'City / Province',
                render: (data) => <TextCell value={ addressSecondAddressLine(data.physicalAddress) } />,
                header: 'City / Province',
                width: 140,
              },
              {
                id: 'Age In Years',
                render: (data) => <TextCell value={ buildingAgeInYears(data) } />,
                header: 'Age In Years',
                width: 140,
              },
              {
                id: 'Pet Policy',
                render: (data) => <TextCell value={ data.petPolicy } />,
                header: 'Pet Policy',
                width: 140,
              },
              {
                id: 'Action',
                render: (data) =>
                  <Group>
                    <Button
                      label='View'
                    />
                    <Button
                      label='Edit'
                      url={ '/buildings/edit/' + data.dbid }
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

