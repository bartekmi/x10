// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import toNum from 'react_lib/utils/toNum';


// Type Definition
export type MatchInfo = {
  +id: string,
  +reasonListed: string,
  +name: string,
  +address: string,
  +ids: string,
  +matchType: string,
  +nameMatchScore: ?number,
  +addressMatchScore: ?number,
  +comments: string,
  +recordSource: string,
};


// Derived Attribute Functions
export function matchInfoScore(matchInfo: {
  +nameMatchScore: ?number,
  +addressMatchScore: ?number,
}): ?number {
  const result = toNum(matchInfo?.nameMatchScore) > toNum(matchInfo?.addressMatchScore) ? matchInfo?.nameMatchScore : matchInfo?.addressMatchScore;
  return isNaN(result) ? null : result;
}



// Create Default Function
export function createDefaultMatchInfo(): MatchInfo {
  return {
    id: uuid(),
    reasonListed: '',
    name: '',
    address: '',
    ids: '',
    matchType: '',
    nameMatchScore: null,
    addressMatchScore: null,
    comments: '',
    recordSource: '',
  };
}


// Validations
export function matchInfoCalculateErrors(matchInfo: MatchInfo, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (matchInfo == null ) return errors;


  return errors;
}
