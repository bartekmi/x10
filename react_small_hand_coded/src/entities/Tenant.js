// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';
import { createDefaultAddress } from 'entities/Address'
import { type Address } from 'entities/Address'

// Type Definition
export type Tenant = {|
  +id: string,
  +dbid: number,
  +name: string,
  +phone: string,
  +email: string,
  +permanentMailingAddress: Address,
|};


// Create Default Function
export function createDefaultTenant(): Tenant {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    name: '',
    phone: '',
    email: '',
    permanentMailingAddress: createDefaultAddress(),
  };
}


