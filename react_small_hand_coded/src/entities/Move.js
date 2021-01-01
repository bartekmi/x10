// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';
import { createDefaultTenant } from 'entities/Tenant'
import { createDefaultUnit } from 'entities/Unit'
import { type Tenant } from 'entities/Tenant'
import { type Unit } from 'entities/Unit'

// Type Definition
export type Move = {|
  +id: string,
  +dbid: number,
  +date: string,
  +from: Unit,
  +to: Unit,
  +tenant: Tenant,
|};


// Create Default Function
export function createDefaultMove(): Move {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    // $FlowExpectedError Required field, but no default value
    date: null,
    from: createDefaultUnit(),
    to: createDefaultUnit(),
    tenant: createDefaultTenant(),
  };
}


