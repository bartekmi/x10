// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';


// Type Definition
export type User = {
  +id: string,
  +firstName: string,
  +lastName: string,
  +email: string,
  +phone: string,
  +location: string,
};


// Derived Attribute Functions
export function userName(user: ?{
  +firstName: string,
  +lastName: string,
}): string {
  if (user == null) return '';
  const result = x10toString(user?.firstName) + ' ' + x10toString(user?.lastName);
  return result;
}



// Create Default Function
export function createDefaultUser(): User {
  return {
    id: uuid(),
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    location: '',
  };
}


// Validations
export function userCalculateErrors(user: User, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (user == null ) return errors;

  if (isBlank(user.firstName))
    addError(errors, prefix, 'First Name is required', ['firstName']);
  if (isBlank(user.lastName))
    addError(errors, prefix, 'Last Name is required', ['lastName']);
  if (isBlank(user.email))
    addError(errors, prefix, 'Email is required', ['email']);

  return errors;
}

