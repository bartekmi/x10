import * as React from 'react';
import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import { getYear } from 'react_lib/type_helpers/dateFunctions';
import isBlank from 'react_lib/utils/isBlank';
import toNum from 'react_lib/utils/toNum';

import { AppContext } from 'SmallAppContext';
import { addressCalculateErrors, createDefaultAddress, type Address } from 'x10_generated/small/entities/Address';
import { unitCalculateErrors, type Unit } from 'x10_generated/small/entities/Unit';

import { MailboxTypeEnum, PetPolicyEnum } from '__generated__/graphql';


// Type Definition
export type Building = {
  readonly id: string,
  readonly moniker: string,
  readonly name?: string,
  readonly description?: string,
  readonly dateOfOccupancy?: string | null | undefined,
  readonly mailboxType?: MailboxTypeEnum | null | undefined,
  readonly petPolicy?: PetPolicyEnum | null | undefined,
  readonly mailingAddressSameAsPhysical?: boolean,
  readonly units?: Unit[],
  readonly physicalAddress?: Address,
  readonly mailingAddress?: Address,
};


// Enums
export const MailboxTypeEnumPairs: {
  value: MailboxTypeEnum,
  label: string
}[] = [
  {
    value: MailboxTypeEnum.InBuilding,
    label: 'Mailroom in Building',
  },
  {
    value: MailboxTypeEnum.Community,
    label: 'Postal System Community Mailbox',
  },
  {
    value: MailboxTypeEnum.Individual,
    label: 'Mail Delivered to Unit',
  },
];

export const PetPolicyEnumPairs: {
  value: PetPolicyEnum,
  label: string
}[] = [
  {
    value: PetPolicyEnum.NoPets,
    label: 'No Pets',
  },
  {
    value: PetPolicyEnum.AllPetsOk,
    label: 'All Pets Ok',
  },
  {
    value: PetPolicyEnum.CatsOnly,
    label: 'Cats Only',
  },
  {
    value: PetPolicyEnum.DogsOnly,
    label: 'Dogs Only',
  },
];



// Derived Attribute Functions
export function buildingAgeInYears(building?: {
    dateOfOccupancy?: string | null | undefined,
} | null | undefined): number | null | undefined | undefined {
  if (building == null) return null;
  const appContext = React.useContext(AppContext);
  const result = getYear(appContext?.today) - getYear(building?.dateOfOccupancy);
  return isNaN(result) ? null : result;
}

export function buildingApplicableWhenForMailingAddress(building?: {
    mailingAddressSameAsPhysical?: boolean,
} | null | undefined): boolean | undefined {
  if (building == null) return false;
  const result = !building?.mailingAddressSameAsPhysical;
  return result;
}



// Create Default Function
export function createDefaultBuilding(): Building {
  return {
    id: uuid(),
    moniker: '1',
    name: '',
    description: '',
    dateOfOccupancy: undefined,
    mailboxType: MailboxTypeEnum.InBuilding,
    petPolicy: undefined,
    mailingAddressSameAsPhysical: true,
    units: [],
    physicalAddress: createDefaultAddress(),
    mailingAddress: createDefaultAddress(),
  };
}


// Validations
export function buildingCalculateErrors(building?: Building, prefix?: string, inListIndex?: number): FormError[] {
  const appContext = React.useContext(AppContext);
  const errors: FormError[] = [];
  if (building == null ) return errors;

  if (isBlank(building.name))
    addError(errors, 'Name is required', ['name'], prefix, inListIndex);
  if (isBlank(building.dateOfOccupancy))
    addError(errors, 'Date Of Occupancy is required', ['dateOfOccupancy'], prefix, inListIndex);
  if (isBlank(building.mailboxType))
    addError(errors, 'Mailbox Type is required', ['mailboxType'], prefix, inListIndex);

  building.units?.forEach((x, ii) => errors.push(...unitCalculateErrors(x, 'units', ii)));
  errors.push(...addressCalculateErrors(building.physicalAddress, 'physicalAddress'));
  if (buildingApplicableWhenForMailingAddress(building))
    errors.push(...addressCalculateErrors(building.mailingAddress, 'mailingAddress'));

  if (toNum(building?.dateOfOccupancy) > toNum(appContext?.today))
    addError(errors, 'Occupancy date cannot be in the future', ['dateOfOccupancy'], prefix, inListIndex);

  return errors;
}

