// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type SettingsAutoAssignment } from 'dps/entities/SettingsAutoAssignment';
import { type WhitelistDuration } from 'dps/entities/WhitelistDuration';


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
  +whitelistDurations: $ReadOnlyArray<WhitelistDuration>,
  +defaultWhitelistDuration: ?WhitelistDuration,
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
    whitelistDurations: [],
    defaultWhitelistDuration: null,
    autoAssignments: [],
  };
}


// Validations
export function settingsCalculateErrors(settings: Settings, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (settings == null ) return errors;

  if (isBlank(settings.messageHitDetected))
    addError(errors, prefix, 'Message Hit Detected is required', ['messageHitDetected']);
  if (isBlank(settings.messageHitCleared))
    addError(errors, prefix, 'Message Hit Cleared is required', ['messageHitCleared']);
  if (isBlank(settings.defaultWhitelistDuration))
    addError(errors, prefix, 'Default Whitelist Duration is required', ['defaultWhitelistDuration']);

  return errors;
}

