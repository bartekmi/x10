// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { type Client } from 'dps/entities/Client';
import { type Port } from 'dps/entities/Port';
import { type TransportationModeEnum } from 'dps/sharedEnums';


// Type Definition
export type Quote = {
  +id: string,
  +dbid: ?number,
  +name: string,
  +transportationMode: ?TransportationModeEnum,
  +status: string,
  +client: ?Client,
  +departurePort: ?Port,
  +arrivalPort: ?Port,
};


// Derived Attribute Functions
export function quoteFlexId(quote: ?{
  +dbid: ?number,
}): string {
  if (quote == null) return '';
  const result = 'FLEX-' + x10toString(quote?.dbid);
  return result;
}

export function quoteUrl(quote: ?{
  +dbid: ?number,
}): string {
  if (quote == null) return '';
  const result = '/quotes/' + x10toString(quote?.dbid);
  return result;
}



// Create Default Function
export function createDefaultQuote(): Quote {
  return {
    id: uuid(),
    dbid: null,
    name: '',
    // $FlowExpectedError Required field, but no default value
    transportationMode: null,
    status: '',
    client: null,
    departurePort: null,
    arrivalPort: null,
  };
}


// Validations
export function quoteCalculateErrors(quote: Quote, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (quote == null ) return errors;

  if (isBlank(quote.transportationMode))
    addError(errors, prefix, 'Transportation Mode is required', ['transportationMode'], inListIndex);
  if (isBlank(quote.status))
    addError(errors, prefix, 'Status is required', ['status'], inListIndex);

  return errors;
}

