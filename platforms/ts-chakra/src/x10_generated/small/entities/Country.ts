import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Country = {
  readonly id: string,
  readonly code: string,
  readonly name: string,
};


// Create Default Function
export function createDefaultCountry(): Country {
  return {
    id: uuid(),
    code: '',
    name: '',
  };
}


// Validations
export function countryCalculateErrors(country: Country, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (country == null ) return errors;

  if (isBlank(country.code))
    addError(errors, 'Code is required', ['code'], prefix, inListIndex);
  if (isBlank(country.name))
    addError(errors, 'Name is required', ['name'], prefix, inListIndex);

  return errors;
}

