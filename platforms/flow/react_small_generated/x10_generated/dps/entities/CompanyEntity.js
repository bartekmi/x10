// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { addressTypeCalculateErrors, createDefaultAddressType, type AddressType } from 'dps/entities/AddressType';
import { companyCalculateErrors, createDefaultCompany, type Company } from 'dps/entities/Company';


// Type Definition
export type CompanyEntity = {
  +id: string,
  +name: string,
  +primaryContact: string,
  +primaryContactEmail: string,
  +mainNumber: string,
  +segment: string,
  +website: string,
  +physicalAddress: AddressType,
  +company: Company,
};


// Create Default Function
export function createDefaultCompanyEntity(): CompanyEntity {
  return {
    id: uuid(),
    name: '',
    primaryContact: '',
    primaryContactEmail: '',
    mainNumber: '',
    segment: '',
    website: '',
    physicalAddress: createDefaultAddressType(),
    company: createDefaultCompany(),
  };
}


// Validations
export function companyEntityCalculateErrors(companyEntity: CompanyEntity, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (companyEntity == null ) return errors;


  errors.push(...addressTypeCalculateErrors(companyEntity.physicalAddress, 'physicalAddress'));
  errors.push(...companyCalculateErrors(companyEntity.company, 'company'));

  return errors;
}

