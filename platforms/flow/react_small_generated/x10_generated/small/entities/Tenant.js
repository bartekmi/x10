// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { addressCalculateErrors, createDefaultAddress, type Address } from 'small/entities/Address';


// Type Definition
export type Tenant = {
  +id: string,
  +name: string,
  +phone: string,
  +email: string,
  +permanentMailingAddress: Address,
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
export function tenantCalculateErrors(tenant: Tenant, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (tenant == null ) return errors;

  if (isBlank(tenant.name))
    addError(errors, prefix, 'Name is required', ['name']);
  if (isBlank(tenant.email))
    addError(errors, prefix, 'Email is required', ['email']);

  errors.push(...addressCalculateErrors(tenant.permanentMailingAddress, 'permanentMailingAddress'));

  return errors;
}
