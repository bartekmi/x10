import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { addressCalculateErrors, createDefaultAddress, type Address } from 'x10_generated/small/entities/Address';


// Type Definition
export type Tenant = {
  readonly id: string,
  readonly name: string,
  readonly phone: string,
  readonly email: string,
  readonly permanentMailingAddress: Address,
};


// Create Default Function
export function createDefaultTenant(): Tenant {
  return {
    id: uuid(),
    name: '',
    phone: '',
    email: '',
    permanentMailingAddress: createDefaultAddress(),
  };
}


// Validations
export function tenantCalculateErrors(tenant: Tenant, prefix?: string, inListIndex?: number): FormError[] {
  const errors: FormError[] = [];
  if (tenant == null ) return errors;

  if (isBlank(tenant.name))
    addError(errors, 'Name is required', ['name'], prefix, inListIndex);
  if (isBlank(tenant.email))
    addError(errors, 'Email is required', ['email'], prefix, inListIndex);

  errors.push(...addressCalculateErrors(tenant.permanentMailingAddress, 'permanentMailingAddress'));

  return errors;
}
