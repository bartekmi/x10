// This file was auto-generated by x10. Do not modify by hand.
// @flow


import * as React from 'react';
import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import { getYear } from 'react_lib/type_helpers/dateFunctions';
import isBlank from 'react_lib/utils/isBlank';
import toNum from 'react_lib/utils/toNum';

import { addressCalculateErrors, createDefaultAddress, type Address } from 'small/entities/Address';
import { type Unit } from 'small/entities/Unit';
import { AppContext } from 'SmallAppContext';


// Type Definition
export type Building = {
  +id: string,
  +moniker: string,
  +name: string,
  +description: string,
  +dateOfOccupancy: ?string,
  +mailboxType: ?MailboxTypeEnum,
  +petPolicy: ?PetPolicyEnum,
  +mailingAddressSameAsPhysical: boolean,
  +units: $ReadOnlyArray<Unit>,
  +physicalAddress: Address,
  +mailingAddress: Address,
};


// Enums
export const MailboxTypeEnumPairs = [
  {
    value: 'in_building',
    label: 'Mailroom in Building',
  },
  {
    value: 'community',
    label: 'Postal System Community Mailbox',
  },
  {
    value: 'individual',
    label: 'Mail Delivered to Unit',
  },
];

export type MailboxTypeEnum = 'in_building' | 'community' | 'individual';

export const PetPolicyEnumPairs = [
  {
    value: 'no_pets',
    label: 'No Pets',
  },
  {
    value: 'all_pets_ok',
    label: 'All Pets Ok',
  },
  {
    value: 'cats_only',
    label: 'Cats Only',
  },
  {
    value: 'dogs_only',
    label: 'Dogs Only',
  },
];

export type PetPolicyEnum = 'no_pets' | 'all_pets_ok' | 'cats_only' | 'dogs_only';



// Derived Attribute Functions
export function buildingAgeInYears(building: ?{
  +year: ?number,
  +dateOfOccupancy: ?string,
}): ?number {
  if (building == null) return null;
  const appContext = React.useContext(AppContext);
  const result = getYear(appContext?.today) - getYear(building?.dateOfOccupancy);
  return isNaN(result) ? null : result;
}

export function buildingApplicableWhenForMailingAddress(building: ?{
  +mailingAddressSameAsPhysical: boolean,
}): boolean {
  if (building == null) return false;
  const result = !building?.mailingAddressSameAsPhysical;
  return result;
}



// Create Default Function
export function createDefaultBuilding(): Building {
  return {
    id: uuid(),
    moniker: '1',
    name: '',
    description: '',
    // $FlowExpectedError Required field, but no default value
    dateOfOccupancy: null,
    mailboxType: 'in_building',
    petPolicy: null,
    mailingAddressSameAsPhysical: true,
    units: [],
    physicalAddress: createDefaultAddress(),
    mailingAddress: createDefaultAddress(),
  };
}


// Validations
export function buildingCalculateErrors(building: Building, prefix?: string): $ReadOnlyArray<FormError> {
  const appContext = React.useContext(AppContext);
  const errors = [];
  if (building == null ) return errors;

  if (isBlank(building.name))
    addError(errors, prefix, 'Name is required', ['name']);
  if (isBlank(building.dateOfOccupancy))
    addError(errors, prefix, 'Date Of Occupancy is required', ['dateOfOccupancy']);
  if (isBlank(building.mailboxType))
    addError(errors, prefix, 'Mailbox Type is required', ['mailboxType']);

  errors.push(...addressCalculateErrors(building.physicalAddress, 'physicalAddress'));
  if (buildingApplicableWhenForMailingAddress(building))
    errors.push(...addressCalculateErrors(building.mailingAddress, 'mailingAddress'));

  if (toNum(building?.dateOfOccupancy) > toNum(appContext?.today))
    addError(errors, prefix, 'Occupancy date cannot be in the future', ['dateOfOccupancy']);

  return errors;
}

