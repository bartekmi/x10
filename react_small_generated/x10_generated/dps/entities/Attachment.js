// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Attachment = {
  +id: string,
  +filename: string,
  +url: string,
};


// Create Default Function
export function createDefaultAttachment(): Attachment {
  return {
    id: uuid(),
    filename: '',
    url: '',
  };
}


// Validations
export function attachmentCalculateErrors(attachment: Attachment, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (attachment == null ) return errors;


  return errors;
}

