// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type SuggestedResource = {
  +id: string,
  +type: ?SuggestedResourceTypeEnum,
  +title: string,
  +text: string,
  +helpful: ?HelpfulStateEnum,
};


// Enums
export const SuggestedResourceTypeEnumPairs = [
  {
    value: 'google',
    label: 'Google',
  },
  {
    value: 'linked_in',
    label: 'Linked In',
  },
];

export type SuggestedResourceTypeEnum = 'google' | 'linked_in';

export const HelpfulStateEnumPairs = [
  {
    value: 'unspecified',
    label: 'Unspecified',
  },
  {
    value: 'helpful',
    label: 'Helpful',
  },
  {
    value: 'unhelpful',
    label: 'Unhelpful',
  },
];

export type HelpfulStateEnum = 'unspecified' | 'helpful' | 'unhelpful';



// Create Default Function
export function createDefaultSuggestedResource(): SuggestedResource {
  return {
    id: uuid(),
    type: null,
    title: '',
    text: '',
    helpful: 'unspecified',
  };
}


// Validations
export function suggestedResourceCalculateErrors(suggestedResource: SuggestedResource, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (suggestedResource == null ) return errors;


  return errors;
}

