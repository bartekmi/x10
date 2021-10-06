// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import FloatDisplay from 'react_lib/display/FloatDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import x10toString from 'react_lib/utils/x10toString';

import { type Hit } from 'dps/entities/Hit';
import { createDefaultMatchInfo } from 'dps/entities/MatchInfo';

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
        scale='display'
        children='Match details'
      />
      <Text
        scale='headline'
        children={ 'Please review ' + x10toString(hit?.matches.length) + ' matches' }
      />
      <Group>
        <Group
          flexDirection='column'
        >
          <Text
            scale='title'
            children='Company information'
          />
          <TextDisplay
            value={ hit?.companyEntity?.name }
          />
          <TextDisplay
            value={ hit?.companyEntity?.primaryContact }
          />
          <TextDisplay
            value={ hit?.companyEntity?.mainNumber }
          />
          <TextDisplay
            value={ hit?.companyEntity?.address }
          />
        </Group>
        <Separator
          orientation='vertical'
        />
        <MultiStacker
          items={ hit?.matches }
          itemDisplayFunc={ (data, onChange) => (
            <Group
              flexDirection='column'
            >
              <Text
                scale='title'
                children='Source information'
              />
              <FloatDisplay
                value={ data?.score }
              />
              <TextDisplay
                value={ data?.sourceList }
              />
              <TextDisplay
                value={ data?.ids }
              />
              <TextDisplay
                value={ data?.name }
              />
              <TextDisplay
                value={ data?.address }
              />
              <TextDisplay
                value={ data?.type }
              />
              <TextDisplay
                value={ data?.title }
              />
              <TextDisplay
                value={ data?.programs }
              />
            </Group>
          ) }
          addNewItem={ createDefaultMatchInfo }
        />
      </Group>
      <Separator/>
      <Text
        scale='display'
        children='Suggested resources'
      />
      <Separator/>
      <Text
        scale='display'
        children='Clearance'
      />
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
        address
        mainNumber
        name
        primaryContact
      }
      matches {
        id
        address
        ids
        name
        programs
        score
        sourceList
        title
        type
      }
    }
  `,
});

