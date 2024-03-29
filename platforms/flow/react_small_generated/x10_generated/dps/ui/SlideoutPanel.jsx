// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';

import DateDisplay from 'react_lib/display/DateDisplay';
import EnumDisplay from 'react_lib/display/EnumDisplay';
import TextDisplay from 'react_lib/display/TextDisplay';
import TimestampDisplay from 'react_lib/display/TimestampDisplay';
import Expander from 'react_lib/Expander';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import Button from 'react_lib/latitude_wrappers/Button';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';
import { getDate } from 'react_lib/type_helpers/dateFunctions';
import toEnum from 'react_lib/utils/toEnum';
import toNum from 'react_lib/utils/toNum';

import { createDefaultDpsAttachment } from 'dps/entities/DpsAttachment';
import { type Hit } from 'dps/entities/Hit';
import { createDefaultHitEdit } from 'dps/entities/HitEdit';
import { createDefaultOldHit } from 'dps/entities/OldHit';
import { ReasonForClearanceEnumPairs } from 'dps/sharedEnums';
import AttachmentComponent from 'dps/ui/AttachmentComponent';

import { type SlideoutPanel_hit } from './__generated__/SlideoutPanel_hit.graphql';



type Props = {|
  +hit: SlideoutPanel_hit,
|};
function SlideoutPanel(props: Props): React.Node {
  const { hit } = props;

  return (
    <StyleControl
      width={ 520 }
    >
      <VerticalStackPanel>
        <Expander
          headerFunc={ () => (
            <Text
              scale='title'
              weight='bold'
              children='Overview'
            />
          ) }
        >
          <DisplayForm>
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
        </Expander>
        <Separator/>
        <Expander
          headerFunc={ () => (
            <Text
              scale='title'
              weight='bold'
              children='Compliance status'
            />
          ) }
        >
          <Group
            justifyContent='space-between'
          >
            <Group
              alignItems='center'
            >
              <TextDisplay
                weight='bold'
                value='Denied party screening'
              />
              <Icon
                iconName='attention'
                color='red40'
              />
            </Group>
            <TextDisplay
              value='Detected as a hit, waiting for review'
            />
          </Group>
        </Expander>
        <Separator/>
        <Expander
          headerFunc={ () => (
            <Text
              scale='title'
              weight='bold'
              children='Denied party screening records'
            />
          ) }
        >
          <MultiStacker
            items={ hit?.oldHits }
            itemDisplayFunc={ (data, onChange, inListIndex) => (
              <DisplayForm>
                <DisplayField
                  label='Screened as a hit'
                >
                  <TimestampDisplay
                    value={ data?.createdAt }
                  />
                </DisplayField>
                <DisplayField
                  label={ toEnum(data?.status) == "denied" ? 'Denied time' : 'Clearance time' }
                >
                  <TimestampDisplay
                    value={ data?.resolutionTimestamp }
                  />
                </DisplayField>
                <DisplayField
                  label={ toEnum(data?.status) == "denied" ? 'Denied by' : 'Cleared by' }
                >
                  <TextDisplay
                    value={ data?.resolvedBy?.name }
                  />
                </DisplayField>
                <DisplayField
                  label='Whitelisting time'
                >
                  <Group
                    alignItems='center'
                  >
                    <DateDisplay
                      value={ getDate(data?.resolutionTimestamp) }
                    />
                    <TextDisplay
                      value='―'
                    />
                    <DateDisplay
                      value={ getDate(data?.whiteListUntil) }
                    />
                  </Group>
                </DisplayField>
                <StyleControl
                  visible={ toEnum(data?.status) != "denied" }
                >
                  <DisplayField
                    label='Reason for clearance'
                  >
                    <EnumDisplay
                      value={ data?.reasonForClearance }
                      options={ ReasonForClearanceEnumPairs }
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
                    itemDisplayFunc={ (data, onChange, inListIndex) => (
                      <AttachmentComponent dpsAttachment={ data }/>
                    ) }
                    layout='wrap'
                    addNewItem={ createDefaultDpsAttachment }
                  />
                </StyleControl>
                <StyleControl
                  visible={ toNum(data?.changeLog.length) > toNum(0) }
                >
                  <VerticalStackPanel>
                    <Separator
                      label='Change log'
                    />
                    <MultiStacker
                      items={ data?.changeLog }
                      itemDisplayFunc={ (data, onChange, inListIndex) => (
                        <Group
                          alignItems='center'
                        >
                          <TextDisplay
                            weight='bold'
                            value={ data?.user?.name }
                          />
                          <TextDisplay
                            value='updated'
                          />
                          <TextDisplay
                            weight='bold'
                            value={ data?.editedFields }
                          />
                          <TextDisplay
                            value='on'
                          />
                          <TimestampDisplay
                            value={ data?.timestamp }
                          />
                        </Group>
                      ) }
                      layout='verticalCompact'
                      addNewItem={ createDefaultHitEdit }
                    />
                  </VerticalStackPanel>
                </StyleControl>
              </DisplayForm>
            ) }
            addNewItem={ createDefaultOldHit }
          />
        </Expander>
      </VerticalStackPanel>
    </StyleControl>
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
          ...AttachmentComponent_dpsAttachment
        }
        changeLog {
          id
          editedFields
          timestamp
          user {
            id
            toStringRepresentation
            name
          }
        }
        createdAt
        notes
        reasonForClearance
        resolutionTimestamp
        resolvedBy {
          id
          toStringRepresentation
          name
        }
        status
        whiteListUntil
      }
    }
  `,
});

