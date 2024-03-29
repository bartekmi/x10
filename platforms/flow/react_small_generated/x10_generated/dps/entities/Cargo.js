// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';


// Type Definition
export type Cargo = {
  +id: string,
  +metric_weight: ?number,
  +metric_volume: ?number,
};


// Create Default Function
export function createDefaultCargo(): Cargo {
  return {
    id: uuid(),
    metric_weight: null,
    metric_volume: null,
  };
}


// Validations
export function cargoCalculateErrors(cargo: Cargo, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (cargo == null ) return errors;


  return errors;
}

