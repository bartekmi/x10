// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';
import toNum from 'react_lib/utils/toNum';

import { createDefaultAttachment } from 'dps/entities/Attachment';
import { type Hit } from 'dps/entities/Hit';
import { createDefaultOldHit } from 'dps/entities/OldHit';
import { userName } from 'dps/entities/User';
import { ReasonForCleranceEnumPairs } from 'dps/sharedEnums';

import { type SlideoutPanel_hit } from './__generated__/SlideoutPanel_hit.graphql';



type Props = {|
  +hit: SlideoutPanel_hit,
|};
function SlideoutPanel(props: Props): React.Node {
  const { hit } = props;

  return (
    <Group
      flexDirection='column'
    >
      <DisplayForm>
        <Text
          scale='headline'
          weight='bold'
          children='Overview'
        />
        <DisplayField
          label='Primary Contact'
        >
          <TextDisplay
            value={ hit?.companyEntity?.primaryContact }
          />
        </DisplayField>
        <DisplayField
          label='Primary Contact Email'
        >
          <TextDisplay
            value={ hit?.companyEntity?.primaryContactEmail }
          />
        </DisplayField>
        <DisplayField
          label='Main Number'
        >
          <TextDisplay
            value={ hit?.companyEntity?.mainNumber }
          />
        </DisplayField>
        <DisplayField
          label='Segment'
        >
          <TextDisplay
            value={ hit?.companyEntity?.segment }
          />
        </DisplayField>
        <DisplayField
          label='Website'
        >
          <Button
            label={ hit?.companyEntity?.website }
            url={ hit?.companyEntity?.website }
          />
        </DisplayField>
        <DisplayField
          label='Address'
        >
          <TextDisplay
            value={ hit?.companyEntity?.physicalAddress?.address }
          />
        </DisplayField>
      </DisplayForm>
      <Separator/>
      <Text
        scale='headline'
        weight='bold'
        children='Compliance status'
      />
      <Group
        justifyContent='space-between'
      >
        <Group
          alignItems='center'
        >
          <Icon
            iconName='attention'
          />
          <TextDisplay
            weight='bold'
            value='Denied party screening'
          />
        </Group>
        <TextDisplay
          value='Detected as a hit, waiting for review'
        />
      </Group>
      <Separator/>
      <Text
        scale='headline'
        weight='bold'
        children='Denied party screening records'
      />
      <MultiStacker
        items={ hit?.oldHits }
        itemDisplayFunc={ (data, onChange) => (
          <DisplayForm>
            <DisplayField
              label='Screened as a hit'
            >
              <TimestampDisplay
                value={ data?.createdAt }
              />
            </DisplayField>
            <DisplayField
              label={ data?.status == "denied" ? 'Denied time' : 'Clearance time' }
            >
              <TimestampDisplay
                value={ data?.resolutionTimestamp }
              />
            </DisplayField>
            <DisplayField
              label={ data?.status == "denied" ? 'Denied by' : 'Cleared by' }
            >
              <TextDisplay
                value={ userName(data?.resolvedBy) }
              />
            </DisplayField>
            <StyleControl
              visible={ data?.status != "denied" }
            >
              <DisplayField
                label='Reason for clearance'
              >
                <EnumDisplay
                  value={ data?.reasonForClearance }
                  options={ ReasonForCleranceEnumPairs }
                />
              </DisplayField>
            </StyleControl>
            <DisplayField
              label='Notes'
            >
              <TextDisplay
                value={ data?.notes }
              />
            </DisplayField>
            <StyleControl
              visible={ toNum(data?.attachments.length) > toNum(0) }
            >
              <MultiStacker
                items={ data?.attachments }
                itemDisplayFunc={ (data, onChange) => (
                  <Button
                    label={ data?.filename }
                    url={ data?.url }
                  />
                ) }
                addNewItem={ createDefaultAttachment }
              />
            </StyleControl>
          </DisplayForm>
        ) }
        addNewItem={ createDefaultOldHit }
      />
    </Group>
  );
}

// $FlowExpectedError
export default createFragmentContainer(SlideoutPanel, {
  hit: graphql`
    fragment SlideoutPanel_hit on Hit {
      id
      companyEntity {
        id
        toStringRepresentation
        mainNumber
        physicalAddress {
          id
          address
        }
        primaryContact
        primaryContactEmail
        segment
        website
      }
      oldHits {
        id
        attachments {
          id
          filename
          url
        }
        createdAt
        notes
        reasonForClearance
        resolutionTimestamp
        resolvedBy {
          id
          toStringRepresentation
          firstName
          lastName
        }
        status
      }
    }
  `,
});

