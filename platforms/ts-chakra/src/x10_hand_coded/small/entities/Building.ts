import { v4 as uuid } from 'uuid';

import { createError, FormError } from '../../../react_lib/form/FormProvider';
import { getYear } from '../../../react_lib/type_helpers/dateFunctions';
import isBlank from '../../../react_lib/utils/isBlank';
// import toNum from '../../../react_lib/utils/toNum';

import { /*addressCalculateErrors,*/ createDefaultAddress, type Address } from './Address';
//import { type Unit } from './Unit';
import { AppContextType } from '../../../SmallAppContext';


// Type Definition
export type Building = {
  readonly id: string,
  readonly moniker: string,
  readonly name: string,
  readonly description: string,
  readonly dateOfOccupancy?: string,
  readonly mailboxType: MailboxTypeEnum | null | undefined,
  readonly petPolicy: PetPolicyEnum | null | undefined,
  readonly mailingAddressSameAsPhysical: boolean,
  // readonly units: Unit[],
  readonly physicalAddress: Address,
  // readonly mailingAddress: Address,
};


// Enums
export const MailboxTypeEnumPairs: {
  value: MailboxTypeEnum,
  label: string
}[] = [
  {
    value: 'in_building',
    label: 'Mailroom in Building',
  },
  {
    value: 'community',
    label: 'Postal System Community Mailbox',
  },
  {
    value: 'individual',
    label: 'Mail Delivered to Unit',
  },
];

export type MailboxTypeEnum = 'in_building' | 'community' | 'individual';

export const PetPolicyEnumPairs: {
  value: PetPolicyEnum,
  label: string
}[] = [
  {
    value: 'no_pets',
    label: 'No Pets',
  },
  {
    value: 'all_pets_ok',
    label: 'All Pets Ok',
  },
  {
    value: 'cats_only',
    label: 'Cats Only',
  },
  {
    value: 'dogs_only',
    label: 'Dogs Only',
  },
];

export type PetPolicyEnum = 'no_pets' | 'all_pets_ok' | 'cats_only' | 'dogs_only';



// Derived Attribute Functions
export function buildingAgeInYears(appContext: AppContextType, building?: {
  readonly year?: number,
  readonly dateOfOccupancy?: string,
}): number | null {
  if (building == null) return null;
  //const appContext = React.useContext(AppContext);
  const result = getYear(appContext?.today) - getYear(building?.dateOfOccupancy);
  return isNaN(result) ? null : result;
}

export function buildingApplicableWhenForMailingAddress(building?: {
  readonly mailingAddressSameAsPhysical: boolean,
}): boolean {
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
    mailboxType: 'in_building',
    petPolicy: undefined,
    mailingAddressSameAsPhysical: true,
    // units: [],
    physicalAddress: createDefaultAddress(),
    // mailingAddress: createDefaultAddress(),
  };
}

export function buildingErrorName(name: string, prefix?: string): FormError | null {
  return isBlank(name) ?
    createError('Name is required', ['name'], prefix) :
    null;
}


// Validations
// export function buildingCalculateErrors(building: Building, prefix?: string): FormError[] {
//   const appContext = React.useContext(AppContext);
//   const errors: FormError[] = [];
//   if (building == null ) return errors;

//   if (isBlank(building.name))
//     addError(errors, 'Name is required', ['name'], prefix);
//   if (isBlank(building.dateOfOccupancy))
//     addError(errors, 'Date Of Occupancy is required', ['dateOfOccupancy'], prefix);
//   if (isBlank(building.mailboxType))
//     addError(errors, 'Mailbox Type is required', ['mailboxType'], prefix);

//   // errors.push(...addressCalculateErrors(building.physicalAddress, 'physicalAddress'));
//   // if (buildingApplicableWhenForMailingAddress(building))
//   //   errors.push(...addressCalculateErrors(building.mailingAddress, 'mailingAddress'));

//   if (toNum(building?.dateOfOccupancy) > toNum(appContext?.today))
//     addError(errors, 'Occupancy date cannot be in the future', ['dateOfOccupancy'], prefix);

//   return errors;
// }

