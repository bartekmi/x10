// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import toNum from 'react_lib/utils/toNum';

import { settingsAutoAssignmentCalculateErrors, type SettingsAutoAssignment } from 'dps/entities/SettingsAutoAssignment';
import { whitelistDurationCalculateErrors, type WhitelistDuration } from 'dps/entities/WhitelistDuration';
import hasDuplicates from 'dps/hasDuplicates';


// Type Definition
export type Settings = {
  +id: string,
  +highUrgencyShipments: ?number,
  +highUrgencyQuotes: ?number,
  +highUrgencyBookings: ?number,
  +highUrgencyDaysBeforeShipment: ?number,
  +highUrgencyEscalated: boolean,
  +mediumUrgencyShipments: ?number,
  +mediumUrgencyQuotes: ?number,
  +mediumUrgencyBookings: ?number,
  +mediumUrgencyDaysBeforeShipment: ?number,
  +messageHitDetected: string,
  +messageHitCleared: string,
  +defaultWhitelistDurationDays: ?number,
  +whitelistDurations: $ReadOnlyArray<WhitelistDuration>,
  +autoAssignments: $ReadOnlyArray<SettingsAutoAssignment>,
};


// Create Default Function
export function createDefaultSettings(): Settings {
  return {
    id: uuid(),
    highUrgencyShipments: null,
    highUrgencyQuotes: null,
    highUrgencyBookings: null,
    highUrgencyDaysBeforeShipment: null,
    highUrgencyEscalated: false,
    mediumUrgencyShipments: null,
    mediumUrgencyQuotes: null,
    mediumUrgencyBookings: null,
    mediumUrgencyDaysBeforeShipment: null,
    messageHitDetected: 'This shipment, quote or booking is blocked due to possible denied party matches. Non-messaging functions are disabled pending Compliance review.',
    messageHitCleared: 'The denied party hit has been cleared by Compliance team. This shipment, quote or booking is unblocked.',
    // $FlowExpectedError Required field, but no default value
    defaultWhitelistDurationDays: null,
    whitelistDurations: [],
    autoAssignments: [],
  };
}


// Validations
export function settingsCalculateErrors(settings: Settings, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (settings == null ) return errors;

  if (isBlank(settings.messageHitDetected))
    addError(errors, prefix, 'Message Hit Detected is required', ['messageHitDetected'], inListIndex);
  if (isBlank(settings.messageHitCleared))
    addError(errors, prefix, 'Message Hit Cleared is required', ['messageHitCleared'], inListIndex);
  if (isBlank(settings.defaultWhitelistDurationDays))
    addError(errors, prefix, 'Default Whitelist Duration Days is required', ['defaultWhitelistDurationDays'], inListIndex);

  settings.whitelistDurations?.forEach((x, ii) => errors.push(...whitelistDurationCalculateErrors(x, 'whitelistDurations', ii)));
  settings.autoAssignments?.forEach((x, ii) => errors.push(...settingsAutoAssignmentCalculateErrors(x, 'autoAssignments', ii)));

  if (toNum(settings?.highUrgencyShipments) <= toNum(settings?.mediumUrgencyShipments))
    addError(errors, prefix, 'High urgency shipments must be greater than medium', ['highUrgencyShipments', 'mediumUrgencyShipments'], inListIndex);
  if (toNum(settings?.highUrgencyQuotes) <= toNum(settings?.mediumUrgencyQuotes))
    addError(errors, prefix, 'High urgency quotes must be greater than medium', ['highUrgencyQuotes', 'mediumUrgencyQuotes'], inListIndex);
  if (toNum(settings?.highUrgencyBookings) <= toNum(settings?.mediumUrgencyBookings))
    addError(errors, prefix, 'High urgency bookings must be greater than medium', ['highUrgencyBookings', 'mediumUrgencyBookings'], inListIndex);
  if (toNum(settings?.highUrgencyDaysBeforeShipment) >= toNum(settings?.mediumUrgencyDaysBeforeShipment))
    addError(errors, prefix, 'High urgency days before shipment must be less than medium', ['highUrgencyDaysBeforeShipment', 'mediumUrgencyDaysBeforeShipment'], inListIndex);
  if (hasDuplicates(settings?.whitelistDurations, 'value'))
    addError(errors, prefix, 'Whitelist days must be unique', ['whitelistDurations'], inListIndex);
  if (hasDuplicates(settings?.whitelistDurations, 'label'))
    addError(errors, prefix, 'Whitelist day labels must be unique', ['whitelistDurations'], inListIndex);

  return errors;
}

