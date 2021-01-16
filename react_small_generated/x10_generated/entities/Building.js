// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';

import { AppContext } from 'AppContext';
import { createDefaultAddress, type Address } from 'entities/Address';
import { type Unit } from 'entities/Unit';
import * as React from 'react';
import { getYear } from 'react_lib/type_helpers/dateFunctions';

// Type Definition
export type Building = {|
  +id: string,
  +dbid: number,
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
|};


// Create Default Function
export function createDefaultBuilding(): Building {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    moniker: '1',
    name: '',
    description: '',
    // $FlowExpectedError Required field, but no default value
    dateOfOccupancy: null,
    mailboxType: 'IN_BUILDING',
    petPolicy: null,
    mailingAddressSameAsPhysical: true,
    units: [],
    physicalAddress: createDefaultAddress(),
    mailingAddress: createDefaultAddress(),
  };
}


// Enums
export const MailboxTypeEnumPairs = [
  {
    value: 'IN_BUILDING',
    label: 'Mailroom in Building',
  },
  {
    value: 'COMMUNITY',
    label: 'Postal System Community Mailbox',
  },
  {
    value: 'INDIVIDUAL',
    label: 'Mail Delivered to Unit',
  },
];

export type MailboxTypeEnum = 'IN_BUILDING' | 'COMMUNITY' | 'INDIVIDUAL';

export const PetPolicyEnumPairs = [
  {
    value: 'NO_PETS',
    label: 'No Pets',
  },
  {
    value: 'ALL_PETS_OK',
    label: 'All Pets Ok',
  },
  {
    value: 'CATS_ONLY',
    label: 'Cats Only',
  },
  {
    value: 'DOGS_ONLY',
    label: 'Dogs Only',
  },
];

export type PetPolicyEnum = 'NO_PETS' | 'ALL_PETS_OK' | 'CATS_ONLY' | 'DOGS_ONLY';



// Derived Attribute Functions
export function buildingAgeInYears(building: Building): ?number {
  const appContext = React.useContext(AppContext);
  const result = getYear(appContext?.today) - getYear(building.dateOfOccupancy);
  return isNaN(result) ? null : result;
}
export function buildingApplicableWhenForMailingAddress(building: Building): boolean {
  const result = !building.mailingAddressSameAsPhysical;
  return result;
}


