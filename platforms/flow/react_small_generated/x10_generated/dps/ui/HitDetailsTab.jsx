// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import Embed from 'react_lib/Embed';
import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';
import Table from 'react_lib/table/Table';
import toEnum from 'react_lib/utils/toEnum';
import x10toString from 'react_lib/utils/x10toString';

import { type Hit } from 'dps/entities/Hit';
import { matchInfoIsAddressMatch, matchInfoIsNameMatch, MatchTypeEnumPairs } from 'dps/entities/MatchInfo';
import { createDefaultMatchInfoSource } from 'dps/entities/MatchInfoSource';
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
    <VerticalStackPanel>
      <Text
        scale='headline'
        weight='bold'
        children='Match details'
      />
      <StyleControl
        visible={ toEnum(hit?.status) != "cleared" }
        padding={ 10 }
        borderColor='orange'
      >
        <Group
          alignItems='center'
        >
          <Icon
            iconName='attention'
            color='orange30'
          />
          <Text
            scale='title'
            weight='bold'
            children={ 'Please review ' + x10toString(hit?.matches.length) + ' matches' }
          />
        </Group>
      </StyleControl>
      <StyleControl
        visible={ toEnum(hit?.status) == "cleared" }
        padding={ 10 }
        borderColor='green'
      >
        <Group
          alignItems='center'
        >
          <Icon
            iconName='check'
            color='green40'
          />
          <Text
            scale='title'
            weight='bold'
            children={ 'You have reviewed ' + x10toString(hit?.matches.length) + ' matches' }
          />
        </Group>
      </StyleControl>
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
            borderColor={ hasNameMatches(hit?.matches) && hit?.user == null ? '#FFA07A' : null }
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
        <StyleControl
          visible={ hit?.user != null }
        >
          <Group
            alignItems='center'
            gap={ 52 }
          >
            <StyleControl
              borderColor={ hasNameMatches(hit?.matches) ? '#FFA07A' : null }
            >
              <DisplayField
                label='User name'
              >
                <TextDisplay
                  value={ hit?.user?.name }
                />
              </DisplayField>
            </StyleControl>
            <DisplayField
              label='User email'
            >
              <TextDisplay
                value={ hit?.user?.email }
              />
            </DisplayField>
          </Group>
        </StyleControl>
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
            <VerticalStackPanel>
              <TextDisplay
                weight='bold'
                value='Match details'
              />
              <DisplayForm
                gap={ 8 }
              >
                <Group
                  alignItems='center'
                  gap={ 40 }
                >
                  <DisplayField
                    label='Name'
                  >
                    <TextDisplay
                      value={ data?.name }
                    />
                  </DisplayField>
                  <DisplayField
                    label='Address'
                  >
                    <TextDisplay
                      value={ data?.address }
                    />
                  </DisplayField>
                  <DisplayField
                    label='Match Type'
                  >
                    <EnumDisplay
                      value={ data?.matchType }
                      options={ MatchTypeEnumPairs }
                    />
                  </DisplayField>
                  <StyleControl
                    visible={ toEnum(data?.matchType) == toEnum("individual") }
                  >
                    <Group
                      alignItems='center'
                      gap={ 40 }
                    >
                      <DisplayField
                        label='Date of birth'
                      >
                        <TextDisplay
                          value={ data?.dob }
                        />
                      </DisplayField>
                      <DisplayField
                        label='Gender'
                      >
                        <TextDisplay
                          value={ data?.gender }
                        />
                      </DisplayField>
                      <DisplayField
                        label='Id Numbers'
                      >
                        <TextDisplay
                          value={ data?.idNumbers }
                        />
                      </DisplayField>
                    </Group>
                  </StyleControl>
                </Group>
                <Separator/>
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
              <TextDisplay
                value='Related resource'
              />
              <MultiStacker
                items={ data?.sources }
                itemDisplayFunc={ (data, onChange, inListIndex) => (
                  <Embed
                    url={ data?.url }
                  />
                ) }
                layout='wrap'
                addNewItem={ createDefaultMatchInfoSource }
              />
              <Group
                alignItems='center'
              >
                <TextDisplay
                  value='If the content of some of the previews is not showing, consider installing'
                />
                <Button
                  label='this Chrome extension'
                  url='https://chrome.google.com/webstore/detail/ignore-x-frame-headers/gleekbfjekiniecknbkamfmkohkpodhe'
                />
              </Group>
            </VerticalStackPanel>
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
    </VerticalStackPanel>
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
        dob
        gender
        idNumbers
        matchType
        name
        nameMatchScore
        number
        reasonListed
        score
        sources {
          id
          url
        }
      }
      status
      user {
        id
        toStringRepresentation
        email
        name
      }
      ...ClearanceForm_hit
    }
  `,
});

