// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Address = {
  +id: string,
  +unitNumber: string,
  +theAddress: string,
  +city: string,
  +stateOrProvince: string,
  +zip: string,
  +country: ?{
    id: string,
  }
};


// Derived Attribute Functions
export function addressFirstAddressLine(address: {
  +theAddress: string,
  +unitNumber: string,
}): string {
  const result = address?.theAddress + '   Unit ' + address?.unitNumber;
  return result;
}

export function addressSecondAddressLine(address: {
  +city: string,
  +stateOrProvince: string,
}): string {
  const result = address?.city + ', ' + address?.stateOrProvince;
  return result;
}

export function addressThirdAddressLine(address: {
  +zip: string,
}): string {
  const result = address?.zip;
  return result;
}



// Create Default Function
export function createDefaultAddress(): Address {
  return {
    id: uuid(),
    unitNumber: '',
    theAddress: '',
    city: '',
    stateOrProvince: '',
    zip: '',
    country: null,
  };
}


// Validations
export function addressCalculateErrors(address: Address, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];

  if (isBlank(address.theAddress))
    addError(errors, prefix, 'The Address is required', ['theAddress']);
  if (isBlank(address.city))
    addError(errors, prefix, 'City is required', ['city']);
  if (isBlank(address.stateOrProvince))
    addError(errors, prefix, 'State Or Province is required', ['stateOrProvince']);
  if (isBlank(address.zip))
    addError(errors, prefix, 'Zip is required', ['zip']);
  if (isBlank(address.country))
    addError(errors, prefix, 'Country is required', ['country']);

  return errors;
}

