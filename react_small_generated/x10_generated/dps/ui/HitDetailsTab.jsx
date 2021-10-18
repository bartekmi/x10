// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Separator from 'react_lib/Separator';
import Table from 'react_lib/table/Table';
import x10toString from 'react_lib/utils/x10toString';

import { type Hit } from 'dps/entities/Hit';
import { MatchTypeEnumPairs } from 'dps/entities/MatchInfo';
import ClearanceForm from 'dps/ui/ClearanceForm';

import { type HitDetailsTab_hit } from './__generated__/HitDetailsTab_hit.graphql';



type Props = {|
  +hit: HitDetailsTab_hit,
|};
function HitDetailsTab(props: Props): React.Node {
  const { hit } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Text
        scale='headline'
        weight='bold'
        children='Match details'
      />
      <Group
        alignItems='center'
      >
        <Icon
          iconName='attention'
        />
        <Text
          scale='title'
          weight='bold'
          children={ 'Please review ' + x10toString(hit?.matches.length) + ' matches' }
        />
      </Group>
      <DisplayForm>
        <Text
          scale='title'
          weight='bold'
          children='Company information'
        />
        <Group
          alignItems='center'
        >
          <DisplayField
            label='Name'
          >
            <TextDisplay
              value={ hit?.companyEntity?.name }
            />
          </DisplayField>
          <DisplayField
            label='Address'
          >
            <TextDisplay
              value={ hit?.companyEntity?.physicalAddress?.address }
            />
          </DisplayField>
        </Group>
      </DisplayForm>
      <Separator/>
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Match information'
          />
        ) }
      >
        <Table
          data={ hit?.matches }
          columns={
            [
              {
                id: '_0',
                Header: 'No.',
                width: 140,
                accessor: (data) => data?.number,
                Cell: ({ value }) =>
                  <FloatDisplay
                    value={ value }
                  />
                ,
              },
              {
                id: '_1',
                Header: 'Score',
                width: 140,
                accessor: (data) => data?.score,
                Cell: ({ value }) =>
                  <FloatDisplay
                    value={ value }
                  />
                ,
              },
              {
                id: '_2',
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
                id: '_3',
                Header: 'Address',
                width: 140,
                accessor: (data) => data?.address,
                Cell: ({ value }) =>
                  <TextDisplay
                    value={ value }
                  />
                ,
              },
              {
                id: '_4',
                Header: 'Type',
                width: 140,
                accessor: (data) => data?.matchType,
                Cell: ({ value }) =>
                  <EnumDisplay
                    value={ value }
                    options={ MatchTypeEnumPairs }
                  />
                ,
              },
            ]
          }
        />
      </Expander>
      <Separator/>
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Clearance'
          />
        ) }
      >
        <ClearanceForm hit={ hit }/>
      </Expander>
    </Group>
  );
}

// $FlowExpectedError
export default createFragmentContainer(HitDetailsTab, {
  hit: graphql`
    fragment HitDetailsTab_hit on Hit {
      id
      companyEntity {
        id
        toStringRepresentation
        name
        physicalAddress {
          id
          address
        }
      }
      matches {
        id
        address
        comments
        matchType
        name
        number
        reasonListed
        score
      }
    }
  `,
});

