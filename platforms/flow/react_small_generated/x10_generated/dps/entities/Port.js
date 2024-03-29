// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';


// Type Definition
export type Port = {
  +id: string,
  +city: string,
  +country_name: string,
  +name: string,
};


// Derived Attribute Functions
export function portCityAndCountry(port: ?{
  +city: string,
  +country_name: string,
}): string {
  if (port == null) return '';
  const result = x10toString(port?.city) + ', ' + x10toString(port?.country_name);
  return result;
}



// Create Default Function
export function createDefaultPort(): Port {
  return {
    id: uuid(),
    city: '',
    country_name: '',
    name: '',
  };
}


// Validations
export function portCalculateErrors(port: Port, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (port == null ) return errors;


  return errors;
}

