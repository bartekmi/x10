// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type MatchInfoSource = {
  +id: string,
  +url: string,
};


// Create Default Function
export function createDefaultMatchInfoSource(): MatchInfoSource {
  return {
    id: uuid(),
    url: '',
  };
}


// Validations
export function matchInfoSourceCalculateErrors(matchInfoSource: MatchInfoSource, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (matchInfoSource == null ) return errors;


  return errors;
}

