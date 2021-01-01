// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';

// Type Definition
export type Address = {|
  +id: string,
  +dbid: number,
  +unitNumber: string,
  +address: string,
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
    address: '',
    city: '',
    stateOrProvince: '',
    zip: '',
  };
}


// Derived Attribute Functions
export function addressFirstAddressLine (address: Address): ?string {
  return address.address + '   Unit ' + address.unitNumber;
}
export function addressSecondAddressLine (address: Address): ?string {
  return address.city + ', ' + address.stateOrProvince;
}
export function addressThirdAddressLine (address: Address): ?string {
  return address.zip;
}


