// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import toNum from 'react_lib/utils/toNum';
import VisibilityControl from 'react_lib/VisibilityControl';

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
          scale='display'
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
            label={ hit?.website }
            url={ hit?.website }
          />
        </DisplayField>
        <DisplayField
          label='Website'
        >
          <TextDisplay
            value={ hit?.companyEntity?.website }
          />
        </DisplayField>
        <DisplayField
          label='Address'
        >
          <TextDisplay
            value={ hit?.companyEntity?.address }
          />
        </DisplayField>
      </DisplayForm>
      <Separator/>
      <Text
        scale='display'
        children='Compliance status'
      />
      <Group
        justifyContent='space-between'
      >
        <TextDisplay
          weight='bold'
          value='Denied party screening'
        />
        <TextDisplay
          value='Detected as a hit, waiting for review'
        />
      </Group>
      <Text
        scale='display'
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
            <DisplayField>
              <TimestampDisplay
                value={ data?.resolutionTimestamp }
              />
            </DisplayField>
            <DisplayField>
              <TextDisplay
                value={ userName(data?.resolvedBy) }
              />
            </DisplayField>
            <VisibilityControl
              visible={ false }
            >
              <DisplayField
                label='Reason for clearance'
              >
                <EnumDisplay
                  value={ data?.reasonForClearance }
                  options={ ReasonForCleranceEnumPairs }
                />
              </DisplayField>
            </VisibilityControl>
            <DisplayField
              label='Notes'
            >
              <TextDisplay
                value={ data?.notes }
              />
            </DisplayField>
            <VisibilityControl
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
            </VisibilityControl>
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
        address
        mainNumber
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
      }
    }
  `,
});

