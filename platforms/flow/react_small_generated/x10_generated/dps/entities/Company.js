// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { clientCalculateErrors, createDefaultClient, type Client } from 'dps/entities/Client';


// Type Definition
export type Company = {
  +id: string,
  +client: Client,
};


// Create Default Function
export function createDefaultCompany(): Company {
  return {
    id: uuid(),
    client: createDefaultClient(),
  };
}


// Validations
export function companyCalculateErrors(company: Company, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (company == null ) return errors;


  errors.push(...clientCalculateErrors(company.client, 'client'));

  return errors;
}

