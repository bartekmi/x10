// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type WhitelistDuration = {
  +id: string,
  +value: ?number,
  +label: string,
};


// Create Default Function
export function createDefaultWhitelistDuration(): WhitelistDuration {
  return {
    id: uuid(),
    // $FlowExpectedError Required field, but no default value
    value: null,
    label: '',
  };
}


// Validations
export function whitelistDurationCalculateErrors(whitelistDuration: WhitelistDuration, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (whitelistDuration == null ) return errors;

  if (isBlank(whitelistDuration.value))
    addError(errors, prefix, 'Value is required', ['value'], inListIndex);
  if (isBlank(whitelistDuration.label))
    addError(errors, prefix, 'Label is required', ['label'], inListIndex);

  return errors;
}

