// This file was auto-generated by x10. Do not modify by hand.
// @flow


export const HitStatusEnumPairs = [
  {
    value: 'UNRESOLVED',
    label: 'Unresolved',
  },
  {
    value: 'CLEARED',
    label: 'Cleared',
  },
  {
    value: 'DENIED',
    label: 'Denied',
  },
];

export type HitStatusEnum = 'UNRESOLVED' | 'CLEARED' | 'DENIED';

export const ReasonForCleranceEnumPairs = [
  {
    value: 'PERSON_NOT_ENTITY',
    label: 'Person is not an Entity/Business',
  },
  {
    value: 'PARTIAL_NAME',
    label: 'Partial Name Match - No Country/Address Match',
  },
  {
    value: 'PARTIAL_ADDRESS',
    label: 'Partial Address Match - No Name Match',
  },
  {
    value: 'OTHER',
    label: 'Other',
  },
];

export type ReasonForCleranceEnum = 'PERSON_NOT_ENTITY' | 'PARTIAL_NAME' | 'PARTIAL_ADDRESS' | 'OTHER';
