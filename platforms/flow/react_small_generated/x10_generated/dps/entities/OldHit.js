// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { dpsAttachmentCalculateErrors, type DpsAttachment } from 'dps/entities/DpsAttachment';
import { hitEditCalculateErrors, type HitEdit } from 'dps/entities/HitEdit';
import { type User } from 'dps/entities/User';
import { type HitStatusEnum, type ReasonForClearanceEnum } from 'dps/sharedEnums';


// Type Definition
export type OldHit = {
  +id: string,
  +status: ?HitStatusEnum,
  +reasonForClearance: ?ReasonForClearanceEnum,
  +whiteListUntil: ?string,
  +notes: string,
  +createdAt: ?string,
  +resolutionTimestamp: ?string,
  +resolvedBy: ?User,
  +changeLog: $ReadOnlyArray<HitEdit>,
  +attachments: $ReadOnlyArray<DpsAttachment>,
};


// Create Default Function
export function createDefaultOldHit(): OldHit {
  return {
    id: uuid(),
    status: 'cleared',
    // $FlowExpectedError Required field, but no default value
    reasonForClearance: null,
    whiteListUntil: null,
    notes: '',
    createdAt: null,
    resolutionTimestamp: null,
    resolvedBy: null,
    changeLog: [],
    attachments: [],
  };
}


// Validations
export function oldHitCalculateErrors(oldHit: OldHit, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (oldHit == null ) return errors;

  if (isBlank(oldHit.status))
    addError(errors, prefix, 'Status is required', ['status'], inListIndex);
  if (isBlank(oldHit.reasonForClearance))
    addError(errors, prefix, 'Reason For Clearance is required', ['reasonForClearance'], inListIndex);

  oldHit.changeLog?.forEach((x, ii) => errors.push(...hitEditCalculateErrors(x, 'changeLog', ii)));
  oldHit.attachments?.forEach((x, ii) => errors.push(...dpsAttachmentCalculateErrors(x, 'attachments', ii)));

  return errors;
}

