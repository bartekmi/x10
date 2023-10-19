// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type AddressType = {
  +id: string,
  +address: string,
};


// Create Default Function
export function createDefaultAddressType(): AddressType {
  return {
    id: uuid(),
    address: '',
  };
}


// Validations
export function addressTypeCalculateErrors(addressType: AddressType, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (addressType == null ) return errors;


  return errors;
}

