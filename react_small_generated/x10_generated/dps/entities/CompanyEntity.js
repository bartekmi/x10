// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { addressTypeCalculateErrors, createDefaultAddressType, type AddressType } from 'dps/entities/AddressType';


// Type Definition
export type CompanyEntity = {
  +id: string,
  +clientId: ?number,
  +name: string,
  +primaryContact: string,
  +primaryContactEmail: string,
  +mainNumber: string,
  +segment: string,
  +website: string,
  +physicalAddress: AddressType,
};


// Create Default Function
export function createDefaultCompanyEntity(): CompanyEntity {
  return {
    id: uuid(),
    clientId: null,
    name: '',
    primaryContact: '',
    primaryContactEmail: '',
    mainNumber: '',
    segment: '',
    website: '',
    physicalAddress: createDefaultAddressType(),
  };
}


// Validations
export function companyEntityCalculateErrors(companyEntity: CompanyEntity, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (companyEntity == null ) return errors;


  errors.push(...addressTypeCalculateErrors(companyEntity.physicalAddress, 'physicalAddress'));

  return errors;
}

