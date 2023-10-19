// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { dpsAttachmentCalculateErrors, type DpsAttachment } from 'dps/entities/DpsAttachment';
import { type User } from 'dps/entities/User';


// Type Definition
export type DpsMessage = {
  +id: string,
  +timestamp: ?string,
  +text: string,
  +coreShipmentId: ?number,
  +user: ?User,
  +attachments: $ReadOnlyArray<DpsAttachment>,
};


// Derived Attribute Functions
export function dpsMessageShipmentUrl(dpsMessage: ?{
  +coreShipmentId: ?number,
}): string {
  if (dpsMessage == null) return '';
  const result = '/shipments/' + x10toString(dpsMessage?.coreShipmentId);
  return result;
}

export function dpsMessageFlexId(dpsMessage: ?{
  +coreShipmentId: ?number,
}): string {
  if (dpsMessage == null) return '';
  const result = 'Flex-' + x10toString(dpsMessage?.coreShipmentId);
  return result;
}



// Create Default Function
export function createDefaultDpsMessage(): DpsMessage {
  return {
    id: uuid(),
    timestamp: null,
    text: '',
    coreShipmentId: null,
    user: null,
    attachments: [],
  };
}


// Validations
export function dpsMessageCalculateErrors(dpsMessage: DpsMessage, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (dpsMessage == null ) return errors;

  if (isBlank(dpsMessage.user))
    addError(errors, prefix, 'User is required', ['user'], inListIndex);

  dpsMessage.attachments?.forEach((x, ii) => errors.push(...dpsAttachmentCalculateErrors(x, 'attachments', ii)));

  return errors;
}

