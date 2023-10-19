// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type User } from 'dps/entities/User';


// Type Definition
export type SettingsAutoAssignment = {
  +id: string,
  +from: ?string,
  +to: ?string,
  +user: ?User,
};


// Create Default Function
export function createDefaultSettingsAutoAssignment(): SettingsAutoAssignment {
  return {
    id: uuid(),
    // $FlowExpectedError Required field, but no default value
    from: null,
    // $FlowExpectedError Required field, but no default value
    to: null,
    user: null,
  };
}


// Validations
export function settingsAutoAssignmentCalculateErrors(settingsAutoAssignment: SettingsAutoAssignment, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (settingsAutoAssignment == null ) return errors;

  if (isBlank(settingsAutoAssignment.from))
    addError(errors, prefix, 'From is required', ['from'], inListIndex);
  if (isBlank(settingsAutoAssignment.to))
    addError(errors, prefix, 'To is required', ['to'], inListIndex);
  if (isBlank(settingsAutoAssignment.user))
    addError(errors, prefix, 'User is required', ['user'], inListIndex);

  return errors;
}

