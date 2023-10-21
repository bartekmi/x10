import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { type Building } from 'x10_generated/small/entities/Building';
import { type Tenant } from 'x10_generated/small/entities/Tenant';


// Type Definition
export type Move = {
  readonly id: string,
  readonly date?: string | null | undefined,
  readonly from?: Building,
  readonly to?: Building,
  readonly tenant?: Tenant,
};


// Create Default Function
export function createDefaultMove(): Move {
  return {
    id: uuid(),
    date: undefined,
    from: undefined,
    to: undefined,
    tenant: undefined,
  };
}


// Validations
export function moveCalculateErrors(move?: Move, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (move == null ) return errors;

  if (isBlank(move.date))
    addError(errors, 'Date is required', ['date'], prefix, inListIndex);
  if (isBlank(move.from))
    addError(errors, 'From is required', ['from'], prefix, inListIndex);
  if (isBlank(move.to))
    addError(errors, 'To is required', ['to'], prefix, inListIndex);
  if (isBlank(move.tenant))
    addError(errors, 'Tenant is required', ['tenant'], prefix, inListIndex);

  return errors;
}

