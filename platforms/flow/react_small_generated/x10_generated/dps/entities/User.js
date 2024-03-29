// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type User = {
  +id: string,
  +name: string,
  +email: string,
  +phone: string,
  +location: string,
};


// Create Default Function
export function createDefaultUser(): User {
  return {
    id: uuid(),
    name: '',
    email: '',
    phone: '',
    location: '',
  };
}


// Validations
export function userCalculateErrors(user: User, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (user == null ) return errors;

  if (isBlank(user.name))
    addError(errors, prefix, 'Name is required', ['name'], inListIndex);
  if (isBlank(user.email))
    addError(errors, prefix, 'Email is required', ['email'], inListIndex);

  return errors;
}

