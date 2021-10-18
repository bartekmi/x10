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
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import x10toString from 'react_lib/utils/x10toString';

import { type Hit } from 'dps/entities/Hit';
import { matchInfoIsAddressMatch, matchInfoIsNameMatch, MatchTypeEnumPairs } from 'dps/entities/MatchInfo';
import hasAddressMatches from 'dps/hasAddressMatches';
import hasNameMatches from 'dps/hasNameMatches';
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
          gap={ 52 }
        >
          <StyleControl
            borderColor={ hasNameMatches(hit?.matches) ? '#FFA07A' : null }
          >
            <DisplayField
              label='Name'
            >
              <TextDisplay
                value={ hit?.companyEntity?.name }
              />
            </DisplayField>
          </StyleControl>
          <StyleControl
            borderColor={ hasAddressMatches(hit?.matches) ? '#FFD700' : null }
          >
            <DisplayField
              label='Address'
            >
              <TextDisplay
                value={ hit?.companyEntity?.physicalAddress?.address }
              />
            </DisplayField>
          </StyleControl>
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
                accessor: (data) => data,
                Cell: ({ value }) =>
                  <StyleControl
                    fillColor={ matchInfoIsNameMatch(value) ? '#FFA07A' : null }
                  >
                    <TextDisplay
                      value={ value?.name }
                    />
                  </StyleControl>
                ,
              },
              {
                id: '_3',
                Header: 'Address',
                width: 140,
                accessor: (data) => data,
                Cell: ({ value }) =>
                  <StyleControl
                    fillColor={ matchInfoIsAddressMatch(value) ? '#FFD700' : null }
                  >
                    <TextDisplay
                      value={ value?.address }
                    />
                  </StyleControl>
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
          expandedContentFunc={ (data) => (
            <Group
              flexDirection='column'
            >
              <TextDisplay
                weight='bold'
                value='Match details'
              />
              <DisplayForm>
                <DisplayField
                  label='Reason Listed'
                >
                  <TextDisplay
                    value={ data?.reasonListed }
                  />
                </DisplayField>
                <Separator/>
                <DisplayField
                  label='Comments'
                >
                  <TextDisplay
                    value={ data?.comments }
                  />
                </DisplayField>
              </DisplayForm>
              <Separator/>
              <Text
                scale='display'
                weight='bold'
                children='TBD - Sources'
              />
            </Group>
          ) }
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
        addressMatchScore
        comments
        matchType
        name
        nameMatchScore
        number
        reasonListed
        score
      }
    }
  `,
});

