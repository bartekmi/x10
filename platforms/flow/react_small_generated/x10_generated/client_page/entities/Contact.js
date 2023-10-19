// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';


// Type Definition
export type Contact = {
  +id: string,
  +firstName: string,
  +lastName: string,
  +phone: string,
  +email: string,
};


// Derived Attribute Functions
export function contactName(contact: ?{
  +firstName: string,
  +lastName: string,
}): string {
  if (contact == null) return '';
  const result = x10toString(contact?.firstName) + ' ' + x10toString(contact?.lastName);
  return result;
}



// Create Default Function
export function createDefaultContact(): Contact {
  return {
    id: uuid(),
    firstName: '',
    lastName: '',
    phone: '',
    email: '',
  };
}


// Validations
export function contactCalculateErrors(contact: Contact, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (contact == null ) return errors;

  if (isBlank(contact.firstName))
    addError(errors, prefix, 'First Name is required', ['firstName']);
  if (isBlank(contact.lastName))
    addError(errors, prefix, 'Last Name is required', ['lastName']);
  if (isBlank(contact.email))
    addError(errors, prefix, 'Email is required', ['email']);

  return errors;
}

