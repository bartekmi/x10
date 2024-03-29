// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import toEnum from 'react_lib/utils/toEnum';

import { bookingCalculateErrors, type Booking } from 'dps/entities/Booking';
import { type CompanyEntity } from 'dps/entities/CompanyEntity';
import { dpsAttachmentCalculateErrors, type DpsAttachment } from 'dps/entities/DpsAttachment';
import { dpsMessageCalculateErrors, type DpsMessage } from 'dps/entities/DpsMessage';
import { matchInfoCalculateErrors, type MatchInfo } from 'dps/entities/MatchInfo';
import { oldHitCalculateErrors, type OldHit } from 'dps/entities/OldHit';
import { quoteCalculateErrors, type Quote } from 'dps/entities/Quote';
import { shipmentCalculateErrors, type Shipment } from 'dps/entities/Shipment';
import { type User } from 'dps/entities/User';
import { type WhitelistDuration } from 'dps/entities/WhitelistDuration';
import { type HitStatusEnum, type ReasonForClearanceEnum } from 'dps/sharedEnums';


// Type Definition
export type Hit = {
  +id: string,
  +urgency: ?UrgencyEnum,
  +status: ?HitStatusEnum,
  +reasonForClearance: ?ReasonForClearanceEnum,
  +notes: string,
  +companyEntity: ?CompanyEntity,
  +user: ?User,
  +attachments: $ReadOnlyArray<DpsAttachment>,
  +matches: $ReadOnlyArray<MatchInfo>,
  +shipments: $ReadOnlyArray<Shipment>,
  +quotes: $ReadOnlyArray<Quote>,
  +bookings: $ReadOnlyArray<Booking>,
  +messages: $ReadOnlyArray<DpsMessage>,
  +oldHits: $ReadOnlyArray<OldHit>,
  +whitelistDays: ?WhitelistDuration,
};


// Enums
export const UrgencyEnumPairs = [
  {
    value: 'low',
    label: 'Low',
  },
  {
    value: 'medium',
    label: 'Medium',
  },
  {
    value: 'high',
    label: 'High',
  },
];

export type UrgencyEnum = 'low' | 'medium' | 'high';



// Create Default Function
export function createDefaultHit(): Hit {
  return {
    id: uuid(),
    urgency: null,
    status: 'unresolved',
    // $FlowExpectedError Required field, but no default value
    reasonForClearance: null,
    notes: '',
    companyEntity: null,
    user: null,
    attachments: [],
    matches: [],
    shipments: [],
    quotes: [],
    bookings: [],
    messages: [],
    oldHits: [],
    whitelistDays: null,
  };
}


// Validations
export function hitCalculateErrors(hit: Hit, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (hit == null ) return errors;

  if (isBlank(hit.reasonForClearance))
    addError(errors, prefix, 'Reason For Clearance is required', ['reasonForClearance'], inListIndex);
  if (isBlank(hit.notes))
    addError(errors, prefix, 'Notes is required', ['notes'], inListIndex);
  if (isBlank(hit.whitelistDays))
    addError(errors, prefix, 'Whitelist Days is required', ['whitelistDays'], inListIndex);

  hit.attachments?.forEach((x, ii) => errors.push(...dpsAttachmentCalculateErrors(x, 'attachments', ii)));
  hit.matches?.forEach((x, ii) => errors.push(...matchInfoCalculateErrors(x, 'matches', ii)));
  hit.shipments?.forEach((x, ii) => errors.push(...shipmentCalculateErrors(x, 'shipments', ii)));
  hit.quotes?.forEach((x, ii) => errors.push(...quoteCalculateErrors(x, 'quotes', ii)));
  hit.bookings?.forEach((x, ii) => errors.push(...bookingCalculateErrors(x, 'bookings', ii)));
  hit.messages?.forEach((x, ii) => errors.push(...dpsMessageCalculateErrors(x, 'messages', ii)));
  hit.oldHits?.forEach((x, ii) => errors.push(...oldHitCalculateErrors(x, 'oldHits', ii)));

  if (toEnum(hit?.status) == "unresolved")
    addError(errors, prefix, 'Please select one of the choices above', ['status'], inListIndex);

  return errors;
}

