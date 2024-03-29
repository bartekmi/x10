// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Table from 'react_lib/table/Table';
import x10toString from 'react_lib/utils/x10toString';

import { UrgencyEnumPairs, type Hit } from 'dps/entities/Hit';

import { type Hits_hits } from './__generated__/Hits_hits.graphql';



type Props = {|
  +hits: Hits_hits,
|};
function Hits(props: Props): React.Node {
  const { hits } = props;

  return (
    <VerticalStackPanel>
      <Text
        scale='display'
        weight='bold'
        children='Hits'
      />
      <Table
        data={ hits }
        columns={
          [
            {
              id: '_0',
              Header: 'Name',
              width: 140,
              accessor: (data) => data?.companyEntity?.name,
              Cell: ({ value }) =>
                <TextDisplay
                  value={ value }
                />
              ,
            },
            {
              id: '_1',
              Header: 'Urgency',
              width: 140,
              accessor: (data) => data?.urgency,
              Cell: ({ value }) =>
                <EnumDisplay
                  value={ value }
                  options={ UrgencyEnumPairs }
                />
              ,
            },
            {
              id: '_2',
              Header: 'Go-To',
              width: 140,
              accessor: (data) => data,
              Cell: ({ value }) =>
                <Group
                  alignItems='center'
                >
                  <Button
                    label='Tabs'
                    url={ '/hits/' + x10toString(value?.id) }
                  />
                  <Button
                    label='Panel'
                    url={ '/hits/panel/' + x10toString(value?.id) }
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
export default createFragmentContainer(Hits, {
  hits: graphql`
    fragment Hits_hits on Hit @relay(plural: true) {
      id
      companyEntity {
        id
        toStringRepresentation
        name
      }
      urgency
    }
  `,
});

