// This file was auto-generated on 01/10/2021 23:35:27. Do not modify by hand.
// @flow

import { v4 as uuid } from 'uuid';

import { DBID_LOCALLY_CREATED } from 'react_lib/constants';


// Type Definition
export type Unit = {|
  +id: string,
  +dbid: number,
  +number: string,
  +squareFeet: ?number,
  +numberOfBedrooms: ?number,
  +numberOfBathrooms: ?NumberOfBathroomsEnum,
  +hasBalcony: boolean,
|};


// Create Default Function
export function createDefaultUnit(): Unit {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    number: '',
    squareFeet: null,
    numberOfBedrooms: 2,
    numberOfBathrooms: 'ONE',
    hasBalcony: false,
  };
}


// Enums
export const NumberOfBathroomsEnumPairs = [
  {
    value: 'HALF',
    label: 'Half',
  },
  {
    value: 'ONE',
    label: '1',
  },
  {
    value: 'ONE_AND_HALF',
    label: '1.5',
  },
  {
    value: 'TWO',
    label: '2',
  },
  {
    value: 'THREE',
    label: '3',
  },
  {
    value: 'FOUR_PLUS',
    label: '4+',
  },
];

export type NumberOfBathroomsEnum = 'HALF' | 'ONE' | 'ONE_AND_HALF' | 'TWO' | 'THREE' | 'FOUR_PLUS';



