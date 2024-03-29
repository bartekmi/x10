// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type User } from 'dps/entities/User';


// Type Definition
export type HitEdit = {
  +id: string,
  +editedFields: string,
  +timestamp: ?string,
  +user: ?User,
};


// Create Default Function
export function createDefaultHitEdit(): HitEdit {
  return {
    id: uuid(),
    editedFields: '',
    // $FlowExpectedError Required field, but no default value
    timestamp: null,
    user: null,
  };
}


// Validations
export function hitEditCalculateErrors(hitEdit: HitEdit, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (hitEdit == null ) return errors;

  if (isBlank(hitEdit.editedFields))
    addError(errors, prefix, 'Edited Fields is required', ['editedFields'], inListIndex);
  if (isBlank(hitEdit.timestamp))
    addError(errors, prefix, 'Timestamp is required', ['timestamp'], inListIndex);
  if (isBlank(hitEdit.user))
    addError(errors, prefix, 'User is required', ['user'], inListIndex);

  return errors;
}

