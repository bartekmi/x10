// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Document = {
  +id: string,
  +name: string,
  +fileName: string,
  +documentType: ?DocumentTypeEnum,
  +uploadedTimestamp: ?string,
  +uploadedBy: ?string,
};


// Enums
export const DocumentTypeEnumPairs = [
  {
    value: 'BUSINESS_LICENSE',
    label: 'Business License',
  },
  {
    value: 'BUSINESS_REGISTRATION',
    label: 'Business Registration',
  },
  {
    value: 'POWER_OF_ATTORNEY',
    label: 'Power Of Attorney',
  },
  {
    value: 'ETC',
    label: 'Etc',
  },
];

export type DocumentTypeEnum = 'BUSINESS_LICENSE' | 'BUSINESS_REGISTRATION' | 'POWER_OF_ATTORNEY' | 'ETC';



// Create Default Function
export function createDefaultDocument(): Document {
  return {
    id: uuid(),
    name: '',
    fileName: '',
    // $FlowExpectedError Required field, but no default value
    documentType: null,
    uploadedTimestamp: null,
    uploadedBy: null,
  };
}


// Validations
export function documentCalculateErrors(document: Document, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];

  if (isBlank(document.name))
    addError(errors, prefix, 'Name is required', ['name']);
  if (isBlank(document.fileName))
    addError(errors, prefix, 'File Name is required', ['fileName']);
  if (isBlank(document.documentType))
    addError(errors, prefix, 'Document Type is required', ['documentType']);

  return errors;
}

