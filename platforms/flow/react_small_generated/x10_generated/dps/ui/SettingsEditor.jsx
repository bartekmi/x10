// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Checkbox from 'latitude/Checkbox';
import FloatInput from 'latitude/FloatInput';
import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import Text from 'latitude/Text';
import TextareaInput from 'latitude/TextareaInput';

import TextDisplay from 'react_lib/display/TextDisplay';
import Expander from 'react_lib/Expander';
import FormErrorDisplay from 'react_lib/form/FormErrorDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import SelectInput from 'react_lib/latitude_wrappers/SelectInput';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import TimeInput from 'react_lib/latitude_wrappers/TimeInput';
import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import MultiStacker from 'react_lib/multi/MultiStacker';
import Separator from 'react_lib/Separator';
import StyleControl from 'react_lib/StyleControl';

import { settingsCalculateErrors, type Settings } from 'dps/entities/Settings';
import { createDefaultSettingsAutoAssignment } from 'dps/entities/SettingsAutoAssignment';
import { createDefaultWhitelistDuration } from 'dps/entities/WhitelistDuration';



type Props = {|
  +settings: Settings,
  +onChange: (settings: Settings) => void,
|};
function SettingsEditor(props: Props): React.Node {
  const { settings, onChange } = props;

  return (
    <FormProvider
      value={ { errors: settingsCalculateErrors(settings) } }
    >
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Hit urgency'
          />
        ) }
      >
        <VerticalStackPanel>
          <Group
            alignItems='center'
          >
            <TextDisplay
              weight='bold'
              value='High urgency'
            />
            <Icon
              iconName='dot'
              color='red40'
            />
          </Group>
          <TextDisplay
            value='The hit is of high urgency if ANY of these conditiona are met.'
          />
          <Group
            alignItems='center'
            gap={ 20 }
          >
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='highUrgencyShipments'
                label='Shipments'
              >
                <FloatInput
                  value={ settings?.highUrgencyShipments }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, highUrgencyShipments: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='highUrgencyQuotes'
                label='Quotes'
              >
                <FloatInput
                  value={ settings?.highUrgencyQuotes }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, highUrgencyQuotes: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='highUrgencyBookings'
                label='Bookings'
              >
                <FloatInput
                  value={ settings?.highUrgencyBookings }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, highUrgencyBookings: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='highUrgencyDaysBeforeShipment'
                label='Days before shipment'
              >
                <FloatInput
                  value={ settings?.highUrgencyDaysBeforeShipment }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, highUrgencyDaysBeforeShipment: value })
                  } }
                  prefix='≤'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              marginLeft={ 80 }
            >
              <FormField
                editorFor='highUrgencyEscalated'
                label='Escalated by Operations'
              >
                <Checkbox
                  checked={ settings?.highUrgencyEscalated }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, highUrgencyEscalated: value })
                  } }
                />
              </FormField>
            </StyleControl>
          </Group>
          <Separator/>
          <Group
            alignItems='center'
          >
            <TextDisplay
              weight='bold'
              value='Medium urgency'
            />
            <Icon
              iconName='dot'
              color='orange30'
            />
          </Group>
          <TextDisplay
            value='The hit is of medium urgency if ANY of these conditions are met (excluding hits classified as high urgency)'
          />
          <Group
            alignItems='center'
            gap={ 20 }
          >
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='mediumUrgencyShipments'
                label='Shipments'
              >
                <FloatInput
                  value={ settings?.mediumUrgencyShipments }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, mediumUrgencyShipments: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='mediumUrgencyQuotes'
                label='Quotes'
              >
                <FloatInput
                  value={ settings?.mediumUrgencyQuotes }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, mediumUrgencyQuotes: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='mediumUrgencyBookings'
                label='Bookings'
              >
                <FloatInput
                  value={ settings?.mediumUrgencyBookings }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, mediumUrgencyBookings: value })
                  } }
                  prefix='≥'
                />
              </FormField>
            </StyleControl>
            <StyleControl
              width={ 80 }
            >
              <FormField
                editorFor='mediumUrgencyDaysBeforeShipment'
                label='Days before shipment'
              >
                <FloatInput
                  value={ settings?.mediumUrgencyDaysBeforeShipment }
                  onChange={ (value) => {
                    // $FlowExpectedError
                    onChange({ ...settings, mediumUrgencyDaysBeforeShipment: value })
                  } }
                  prefix='≤'
                />
              </FormField>
            </StyleControl>
          </Group>
          <Separator/>
          <Group
            alignItems='center'
          >
            <TextDisplay
              weight='bold'
              value='Low urgency'
            />
            <Icon
              iconName='dot'
              color='orange20'
            />
          </Group>
          <TextDisplay
            value='The hit is of low urgency if it’s not classified as either high or medium'
          />
        </VerticalStackPanel>
      </Expander>
      <Separator
        color='#4b5564'
      />
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Whitelisting'
          />
        ) }
      >
        <Group
          alignItems='flex-start'
          gap={ 100 }
        >
          <VerticalStackPanel>
            <MultiStacker
              items={ settings?.whitelistDurations }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...settings, whitelistDurations: value })
              } }
              itemDisplayFunc={ (data, onChange, inListIndex) => (
                <Group
                  alignItems='flex-start'
                  gap={ 20 }
                >
                  <StyleControl
                    width={ 80 }
                  >
                    <FormField
                      editorFor='whitelistDurations.value'
                      inListIndex={ inListIndex }
                      indicateRequired={ true }
                      label='# of days'
                    >
                      <FloatInput
                        value={ data?.value }
                        onChange={ (value) => {
                          // $FlowExpectedError
                          onChange({ ...data, value: value })
                        } }
                      />
                    </FormField>
                  </StyleControl>
                  <FormField
                    editorFor='whitelistDurations.label'
                    inListIndex={ inListIndex }
                    indicateRequired={ true }
                    label='Label'
                  >
                    <TextInput
                      value={ data?.label }
                      onChange={ (value) => {
                        // $FlowExpectedError
                        onChange({ ...data, label: value })
                      } }
                    />
                  </FormField>
                </Group>
              ) }
              layout='verticalCompact'
              addNewItem={ createDefaultWhitelistDuration }
            />
            <FormErrorDisplay
              paths='whitelistDurations'
            />
          </VerticalStackPanel>
          <FormField
            editorFor='defaultWhitelistDurationDays'
            indicateRequired={ true }
            label='Default whitelisting days'
          >
            <SelectInput
              value={ settings?.defaultWhitelistDurationDays }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...settings, defaultWhitelistDurationDays: value })
              } }
              options={ settings.whitelistDurations }
            />
          </FormField>
        </Group>
      </Expander>
      <Separator
        color='#4b5564'
      />
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Automated internal messages'
          />
        ) }
      >
        <VerticalStackPanel>
          <FormField
            editorFor='messageHitDetected'
            indicateRequired={ true }
            label='Hit detected'
          >
            <TextareaInput
              value={ settings?.messageHitDetected }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...settings, messageHitDetected: value })
              } }
              rows={ 3 }
            />
          </FormField>
          <FormField
            editorFor='messageHitCleared'
            indicateRequired={ true }
            label='Hit cleared'
          >
            <TextareaInput
              value={ settings?.messageHitCleared }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...settings, messageHitCleared: value })
              } }
              rows={ 3 }
            />
          </FormField>
        </VerticalStackPanel>
      </Expander>
      <Separator
        color='#4b5564'
      />
      <Expander
        headerFunc={ () => (
          <Text
            scale='title'
            weight='bold'
            children='Auto assignment'
          />
        ) }
      >
        <VerticalStackPanel>
          <TextDisplay
            value='The time configuration is based on UTC+0 time zone (Pacific time is UTC-7)'
          />
          <MultiStacker
            items={ settings?.autoAssignments }
            onChange={ (value) => {
              // $FlowExpectedError
              onChange({ ...settings, autoAssignments: value })
            } }
            itemDisplayFunc={ (data, onChange, inListIndex) => (
              <Group
                alignItems='center'
              >
                <FormField
                  editorFor='autoAssignments.from'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='From'
                >
                  <TimeInput
                    value={ data?.from }
                    onChange={ (value) => {
                      // $FlowExpectedError
                      onChange({ ...data, from: value })
                    } }
                  />
                </FormField>
                <FormField
                  editorFor='autoAssignments.to'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='To'
                >
                  <TimeInput
                    value={ data?.to }
                    onChange={ (value) => {
                      // $FlowExpectedError
                      onChange({ ...data, to: value })
                    } }
                  />
                </FormField>
                <FormField
                  editorFor='autoAssignments.user'
                  inListIndex={ inListIndex }
                  indicateRequired={ true }
                  label='User'
                >
                  <AssociationEditor
                    id={ data?.user?.id }
                    onChange={ (value) => {
                      // $FlowExpectedError
                      onChange({ ...data, user: value == null ? null : { id: value } })
                    } }
                    isNullable={ false }
                    query={ usersQuery }
                    toString={ x => x.toStringRepresentation }
                  />
                </FormField>
              </Group>
            ) }
            layout='verticalCompact'
            addNewItem={ createDefaultSettingsAutoAssignment }
          />
        </VerticalStackPanel>
      </Expander>
      <StyleControl
        marginTop={ 30 }
      >
        <FormSubmitButton
          mutation={ mutation }
          variables={
            {
              settings: {
                id: settings.id,
                highUrgencyShipments: settings.highUrgencyShipments,
                highUrgencyQuotes: settings.highUrgencyQuotes,
                highUrgencyBookings: settings.highUrgencyBookings,
                highUrgencyDaysBeforeShipment: settings.highUrgencyDaysBeforeShipment,
                highUrgencyEscalated: settings.highUrgencyEscalated,
                mediumUrgencyShipments: settings.mediumUrgencyShipments,
                mediumUrgencyQuotes: settings.mediumUrgencyQuotes,
                mediumUrgencyBookings: settings.mediumUrgencyBookings,
                mediumUrgencyDaysBeforeShipment: settings.mediumUrgencyDaysBeforeShipment,
                whitelistDurations: settings.whitelistDurations,
                defaultWhitelistDurationDays: settings.defaultWhitelistDurationDays,
                messageHitDetected: settings.messageHitDetected,
                messageHitCleared: settings.messageHitCleared,
                autoAssignments: settings.autoAssignments,
              }
            }
          }
          label='Save Settings'
          successMessage='Settings updated successfully.'
          errorMessage='There was a problem. Settings not saved.'
        />
      </StyleControl>
    </FormProvider>
  );
}

type StatefulProps = {|
  +settings: Settings,
|};
export function SettingsEditorStateful(props: StatefulProps): React.Node {
  const settings = relayToInternal(props.settings);
  const [editedSettings, setEditedSettings] = React.useState(settings);
  return <SettingsEditor
    settings={ editedSettings }
    onChange={ setEditedSettings }
  />
}

function relayToInternal(relay: any): Settings {
  return {
    ...relay,
  };
}

const mutation = graphql`
  mutation SettingsEditorMutation(
    $settings: SettingsEditorSettingsInput!
  ) {
    settingsEditorUpdateSettings(
      data: $settings
    ) {
      id
      autoAssignments {
        id
        from
        to
        user {
          id
          toStringRepresentation
        }
      }
      defaultWhitelistDurationDays
      highUrgencyBookings
      highUrgencyDaysBeforeShipment
      highUrgencyEscalated
      highUrgencyQuotes
      highUrgencyShipments
      mediumUrgencyBookings
      mediumUrgencyDaysBeforeShipment
      mediumUrgencyQuotes
      mediumUrgencyShipments
      messageHitCleared
      messageHitDetected
      whitelistDurations {
        id
        label
        value
      }
    }
  }
`;

// $FlowExpectedError
export default createFragmentContainer(SettingsEditorStateful, {
  settings: graphql`
    fragment SettingsEditor_settings on Settings {
      id
      autoAssignments {
        id
        from
        to
        user {
          id
          toStringRepresentation
        }
      }
      defaultWhitelistDurationDays
      highUrgencyBookings
      highUrgencyDaysBeforeShipment
      highUrgencyEscalated
      highUrgencyQuotes
      highUrgencyShipments
      mediumUrgencyBookings
      mediumUrgencyDaysBeforeShipment
      mediumUrgencyQuotes
      mediumUrgencyShipments
      messageHitCleared
      messageHitDetected
      whitelistDurations {
        id
        label
        value
      }
    }
  `,
});

const usersQuery = graphql`
  query SettingsEditor_usersQuery {
    entities: users {
      id
      toStringRepresentation
    }
  }
`;

