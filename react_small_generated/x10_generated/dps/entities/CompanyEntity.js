// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

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


// Derived Attribute Functions
export function companyEntityUrl(companyEntity: ?{
  +clientId: ?number,
}): string {
  if (companyEntity == null) return '';
  const result = '/clients/' + x10toString(companyEntity?.clientId);
  return result;
}



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

