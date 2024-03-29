// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type DpsAttachment = {
  +id: string,
  +filename: string,
  +url: string,
};


// Create Default Function
export function createDefaultDpsAttachment(): DpsAttachment {
  return {
    id: uuid(),
    filename: '',
    url: '',
  };
}


// Validations
export function dpsAttachmentCalculateErrors(dpsAttachment: DpsAttachment, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (dpsAttachment == null ) return errors;


  return errors;
}

