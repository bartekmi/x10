import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { NumberOfBathroomsEnum } from '__generated__/graphql';


// Type Definition
export type Unit = {
  readonly id: string,
  readonly number: string,
  readonly squareFeet: number | null | undefined,
  readonly numberOfBedrooms: number | null | undefined,
  readonly numberOfBathrooms: NumberOfBathroomsEnum | null | undefined,
  readonly hasBalcony: boolean,
};


// Enums
export const NumberOfBathroomsEnumPairs: {
  value: NumberOfBathroomsEnum,
  label: string
}[] = [
  {
    value: NumberOfBathroomsEnum.Half,
    label: 'Half',
  },
  {
    value: NumberOfBathroomsEnum.One,
    label: '1',
  },
  {
    value: NumberOfBathroomsEnum.OneAndHalf,
    label: '1.5',
  },
  {
    value: NumberOfBathroomsEnum.Two,
    label: '2',
  },
  {
    value: NumberOfBathroomsEnum.Three,
    label: '3',
  },
  {
    value: NumberOfBathroomsEnum.FourPlus,
    label: '4+',
  },
];



// Create Default Function
export function createDefaultUnit(): Unit {
  return {
    id: uuid(),
    number: '',
    squareFeet: undefined,
    numberOfBedrooms: 2,
    numberOfBathrooms: NumberOfBathroomsEnum.One,
    hasBalcony: false,
  };
}


// Validations
export function unitCalculateErrors(unit: Unit, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (unit == null ) return errors;

  if (isBlank(unit.number))
    addError(errors, 'Number is required', ['number'], prefix, inListIndex);
  if (isBlank(unit.numberOfBedrooms))
    addError(errors, 'Number Of Bedrooms is required', ['numberOfBedrooms'], prefix, inListIndex);
  if (isBlank(unit.numberOfBathrooms))
    addError(errors, 'Number Of Bathrooms is required', ['numberOfBathrooms'], prefix, inListIndex);

  return errors;
}
