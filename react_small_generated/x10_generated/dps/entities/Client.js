// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Client = {
  +id: string,
  +dbid: ?number,
};


// Create Default Function
export function createDefaultClient(): Client {
  return {
    id: uuid(),
    dbid: null,
  };
}


// Validations
export function clientCalculateErrors(client: Client, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (client == null ) return errors;


  return errors;
}
