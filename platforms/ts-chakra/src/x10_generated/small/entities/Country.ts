import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { AppContextType } from 'SmallAppContext';


// Type Definition
export type Country = {
  readonly id: string,
  readonly code?: string,
  readonly name?: string,
};


// Derived Attribute Functions
export function countryToStringRepresentation(appContext: AppContextType, country?: {
    code?: string,
    name?: string,
} | null | undefined): string | undefined {
  if (country == null) return '';
  const result = x10toString(country?.code) + ' - ' + x10toString(country?.name);
  return result;
}



// Create Default Function
export function createDefaultCountry(): Country {
  return {
    id: uuid(),
    code: '',
    name: '',
  };
}


// Validations
export function countryCalculateErrors(appContext: AppContextType, country?: Country, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (country == null ) return errors;

  if (isBlank(country.code))
    addError(errors, 'Code is required', ['code'], prefix, inListIndex);
  if (isBlank(country.name))
    addError(errors, 'Name is required', ['name'], prefix, inListIndex);

  return errors;
}

