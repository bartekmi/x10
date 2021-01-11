// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';


// Type Definition
export type __Context__ = {|
  +id: string,
  +dbid: number,
  +today: ?string,
|};


// Create Default Function
export function createDefault__Context__(): __Context__ {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    // $FlowExpectedError Required field, but no default value
    today: null,
  };
}


