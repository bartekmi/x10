// This file was auto-generated on 01/10/2021 23:35:27. Do not modify by hand.
// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';


// Type Definition
export type Address = {|
  +id: string,
  +dbid: number,
  +unitNumber: string,
  +theAddress: string,
  +city: string,
  +stateOrProvince: string,
  +zip: string,
|};


// Create Default Function
export function createDefaultAddress(): Address {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    unitNumber: '',
    theAddress: '',
    city: '',
    stateOrProvince: '',
    zip: '',
  };
}


// Derived Attribute Functions
export function addressFirstAddressLine(address: Address): string {
  return address.theAddress + '   Unit ' + address.unitNumber;
}
export function addressSecondAddressLine(address: Address): string {
  return address.city + ', ' + address.stateOrProvince;
}
export function addressThirdAddressLine(address: Address): string {
  return address.zip;
}


