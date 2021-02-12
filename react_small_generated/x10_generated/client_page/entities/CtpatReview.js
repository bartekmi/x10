// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type CtpatReview = {
  +id: string,
  +complianceScreenRequired: boolean,
  +status: ?CtpatReviewStatusEnum,
  +expiresAt: ?string,
  +complianceContactEmail: string,
};


// Enums
export const CtpatReviewStatusEnumPairs = [
  {
    value: 'COMPLIANT',
    label: 'Compliant',
  },
  {
    value: 'GRACE_PERIOD',
    label: 'Grace Period',
  },
  {
    value: 'NON_COMPLIANT',
    label: 'Non Compliant',
  },
  {
    value: 'PROVISIONAL',
    label: 'Provisional',
  },
];

export type CtpatReviewStatusEnum = 'COMPLIANT' | 'GRACE_PERIOD' | 'NON_COMPLIANT' | 'PROVISIONAL';



// Create Default Function
export function createDefaultCtpatReview(): CtpatReview {
  return {
    id: uuid(),
    complianceScreenRequired: false,
    // $FlowExpectedError Required field, but no default value
    status: null,
    expiresAt: null,
    complianceContactEmail: '',
  };
}


// Validations
export function ctpatReviewCalculateErrors(ctpatReview: CtpatReview, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];

  if (isBlank(ctpatReview.status))
    addError(errors, prefix, 'Status is required', ['status']);
  if (isBlank(ctpatReview.complianceContactEmail))
    addError(errors, prefix, 'Compliance Contact Email is required', ['complianceContactEmail']);

  return errors;
}
