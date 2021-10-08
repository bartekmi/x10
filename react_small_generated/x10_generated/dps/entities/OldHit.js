// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type Attachment } from 'dps/entities/Attachment';


// Type Definition
export type OldHit = {
  +id: string,
  +status: ?HitStatusEnum,
  +reasonForClearance: ?ReasonForCleranceEnum,
  +whiteListUntil: ?string,
  +notes: string,
  +createdAt: ?string,
  +resolutionTimestamp: ?string,
  +resolvedBy: ?{ id: string },
  +attachments: $ReadOnlyArray<Attachment>,
};


// Create Default Function
export function createDefaultOldHit(): OldHit {
  return {
    id: uuid(),
    status: 'UNRESOLVED',
    // $FlowExpectedError Required field, but no default value
    reasonForClearance: null,
    whiteListUntil: null,
    notes: '',
    createdAt: null,
    resolutionTimestamp: null,
    resolvedBy: null,
    attachments: [],
  };
}


// Validations
export function oldHitCalculateErrors(oldHit: OldHit, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (oldHit == null ) return errors;

  if (isBlank(oldHit.reasonForClearance))
    addError(errors, prefix, 'Reason For Clearance is required', ['reasonForClearance']);
  if (isBlank(oldHit.notes))
    addError(errors, prefix, 'Notes is required', ['notes']);

  return errors;
}
