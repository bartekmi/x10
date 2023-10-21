import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { type Country } from 'x10_generated/small/entities/Country';


// Type Definition
export type Address = {
  readonly id: string,
  readonly unitNumber: string,
  readonly theAddress: string,
  readonly city: string,
  readonly stateOrProvince: string,
  readonly zip: string,
  readonly country: Country,
};


// Derived Attribute Functions
export function addressFirstAddressLine(address?: {
    theAddress?: string,
    unitNumber?: string,
}): string {
  if (address == null) return '';
  const result = x10toString(address?.theAddress) + '   Unit ' + x10toString(address?.unitNumber);
  return result;
}

export function addressSecondAddressLine(address?: {
    city?: string,
    stateOrProvince?: string,
}): string {
  if (address == null) return '';
  const result = x10toString(address?.city) + ', ' + x10toString(address?.stateOrProvince);
  return result;
}

export function addressThirdAddressLine(address?: {
    zip?: string,
}): string {
  if (address == null) return '';
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
export function addressCalculateErrors(address: Address, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (address == null ) return errors;

  if (isBlank(address.theAddress))
    addError(errors, 'The Address is required', ['theAddress'], prefix, inListIndex);
  if (isBlank(address.city))
    addError(errors, 'City is required', ['city'], prefix, inListIndex);
  if (isBlank(address.stateOrProvince))
    addError(errors, 'State Or Province is required', ['stateOrProvince'], prefix, inListIndex);
  if (isBlank(address.zip))
    addError(errors, 'Zip is required', ['zip'], prefix, inListIndex);
  if (isBlank(address.country))
    addError(errors, 'Country is required', ['country'], prefix, inListIndex);

  return errors;
}

