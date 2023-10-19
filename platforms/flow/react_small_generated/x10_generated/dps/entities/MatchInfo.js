// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import toNum from 'react_lib/utils/toNum';

import { matchInfoSourceCalculateErrors, type MatchInfoSource } from 'dps/entities/MatchInfoSource';


// Type Definition
export type MatchInfo = {
  +id: string,
  +number: ?number,
  +reasonListed: string,
  +name: string,
  +address: string,
  +matchType: ?MatchTypeEnum,
  +score: ?number,
  +nameMatchScore: ?number,
  +addressMatchScore: ?number,
  +comments: string,
  +dob: string,
  +gender: string,
  +idNumbers: string,
  +sources: $ReadOnlyArray<MatchInfoSource>,
};


// Enums
export const MatchTypeEnumPairs = [
  {
    value: 'business',
    label: 'Business',
  },
  {
    value: 'individual',
    label: 'Individual',
  },
  {
    value: 'vessel',
    label: 'Vessel',
  },
];

export type MatchTypeEnum = 'business' | 'individual' | 'vessel';



// Derived Attribute Functions
export function matchInfoIsNameMatch(matchInfo: ?{
  +nameMatchScore: ?number,
}): boolean {
  if (matchInfo == null) return false;
  const result = toNum(matchInfo?.nameMatchScore) > toNum(85);
  return result;
}

export function matchInfoIsAddressMatch(matchInfo: ?{
  +addressMatchScore: ?number,
}): boolean {
  if (matchInfo == null) return false;
  const result = toNum(matchInfo?.addressMatchScore) > toNum(85);
  return result;
}



// Create Default Function
export function createDefaultMatchInfo(): MatchInfo {
  return {
    id: uuid(),
    // $FlowExpectedError Required field, but no default value
    number: null,
    reasonListed: '',
    name: '',
    address: '',
    // $FlowExpectedError Required field, but no default value
    matchType: null,
    // $FlowExpectedError Required field, but no default value
    score: null,
    // $FlowExpectedError Required field, but no default value
    nameMatchScore: null,
    // $FlowExpectedError Required field, but no default value
    addressMatchScore: null,
    comments: '',
    dob: '',
    gender: '',
    idNumbers: '',
    sources: [],
  };
}


// Validations
export function matchInfoCalculateErrors(matchInfo: MatchInfo, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (matchInfo == null ) return errors;

  if (isBlank(matchInfo.number))
    addError(errors, prefix, 'Number is required', ['number'], inListIndex);
  if (isBlank(matchInfo.reasonListed))
    addError(errors, prefix, 'Reason Listed is required', ['reasonListed'], inListIndex);
  if (isBlank(matchInfo.name))
    addError(errors, prefix, 'Name is required', ['name'], inListIndex);
  if (isBlank(matchInfo.address))
    addError(errors, prefix, 'Address is required', ['address'], inListIndex);
  if (isBlank(matchInfo.matchType))
    addError(errors, prefix, 'Match Type is required', ['matchType'], inListIndex);
  if (isBlank(matchInfo.score))
    addError(errors, prefix, 'Score is required', ['score'], inListIndex);
  if (isBlank(matchInfo.nameMatchScore))
    addError(errors, prefix, 'Name Match Score is required', ['nameMatchScore'], inListIndex);
  if (isBlank(matchInfo.addressMatchScore))
    addError(errors, prefix, 'Address Match Score is required', ['addressMatchScore'], inListIndex);

  matchInfo.sources?.forEach((x, ii) => errors.push(...matchInfoSourceCalculateErrors(x, 'sources', ii)));

  return errors;
}

