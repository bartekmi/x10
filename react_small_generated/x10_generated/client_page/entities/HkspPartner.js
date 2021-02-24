// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type HkspPartner = {
  +id: string,
  +name: string,
};


// Create Default Function
export function createDefaultHkspPartner(): HkspPartner {
  return {
    id: uuid(),
    name: '',
  };
}


// Validations
export function hkspPartnerCalculateErrors(hkspPartner: HkspPartner, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (hkspPartner == null ) return errors;

  if (isBlank(hkspPartner.name))
    addError(errors, prefix, 'Name is required', ['name']);

  return errors;
}

