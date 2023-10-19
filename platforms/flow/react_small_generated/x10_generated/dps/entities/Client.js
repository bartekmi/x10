// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';


// Type Definition
export type Client = {
  +id: string,
  +dbid: ?number,
  +name: string,
};


// Derived Attribute Functions
export function clientUrl(client: ?{
  +dbid: ?number,
}): string {
  if (client == null) return '';
  const result = '/clients/' + x10toString(client?.dbid);
  return result;
}



// Create Default Function
export function createDefaultClient(): Client {
  return {
    id: uuid(),
    dbid: null,
    name: '',
  };
}


// Validations
export function clientCalculateErrors(client: Client, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (client == null ) return errors;

  if (isBlank(client.name))
    addError(errors, prefix, 'Name is required', ['name'], inListIndex);

  return errors;
}

