// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type StateOrProvince } from 'client_page/entities/StateOrProvince';


// Type Definition
export type Country = {
  +id: string,
  +code: string,
  +name: string,
  +subRegions: $ReadOnlyArray<StateOrProvince>,
};


// Create Default Function
export function createDefaultCountry(): Country {
  return {
    id: uuid(),
    code: '',
    name: '',
    subRegions: [],
  };
}


// Validations
export function countryCalculateErrors(country: Country, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (country == null ) return errors;

  if (isBlank(country.code))
    addError(errors, prefix, 'Code is required', ['code']);
  if (isBlank(country.name))
    addError(errors, prefix, 'Name is required', ['name']);

  return errors;
}

