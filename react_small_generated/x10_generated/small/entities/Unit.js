// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Unit = {
  +id: string,
  +number: string,
  +squareFeet: ?number,
  +numberOfBedrooms: ?number,
  +numberOfBathrooms: ?NumberOfBathroomsEnum,
  +hasBalcony: boolean,
};


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



// Create Default Function
export function createDefaultUnit(): Unit {
  return {
    id: uuid(),
    number: '',
    squareFeet: null,
    numberOfBedrooms: 2,
    numberOfBathrooms: 'ONE',
    hasBalcony: false,
  };
}


// Validations
export function unitCalculateErrors(unit: Unit, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (unit == null ) return errors;

  if (isBlank(unit.number))
    addError(errors, prefix, 'Number is required', ['number']);
  if (isBlank(unit.numberOfBedrooms))
    addError(errors, prefix, 'Number Of Bedrooms is required', ['numberOfBedrooms']);
  if (isBlank(unit.numberOfBathrooms))
    addError(errors, prefix, 'Number Of Bathrooms is required', ['numberOfBathrooms']);

  return errors;
}

