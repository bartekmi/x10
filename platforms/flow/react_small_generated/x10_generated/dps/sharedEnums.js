// TEAM: compliance
// @flow

export const HitStatusEnumPairs = [
  {
    value: 'unresolved',
    label: 'Unresolved',
  },
  {
    value: 'cleared',
    label: 'No',
  },
  {
    value: 'denied',
    label: 'Yes',
  },
];

export type HitStatusEnum = 'unresolved' | 'cleared' | 'denied';

export const ReasonForClearanceEnumPairs = [
  {
    value: 'person_not_entity',
    label: 'Person is not an Entity/Business',
  },
  {
    value: 'partial_name',
    label: 'Partial Name Match - No Country/Address Match',
  },
  {
    value: 'partial_address',
    label: 'Partial Address Match - No Name Match',
  },
  {
    value: 'other',
    label: 'Other',
  },
];

export type ReasonForClearanceEnum = 'person_not_entity' | 'partial_name' | 'partial_address' | 'other';

export const TransportationModeEnumPairs = [
  {
    value: 'air',
    label: 'Air',
    icon: 'plane'
  },
  {
    value: 'ocean',
    label: 'Ocean',
    icon: 'ship'
  },
  {
    value: 'truck',
    label: 'Truck',
    icon: 'truck'
  },
  {
    value: 'rail',
    label: 'Rail',
    icon: 'rail'
  },
  {
    value: 'unknown_transportation',
    label: 'Unknown Transportation',
    icon: 'question'
  },
  {
    value: 'truck_intl',
    label: 'Truck Intl',
    icon: 'truck'
  },
  {
    value: 'warehouse_storage',
    label: 'Warehouse Storage',
    icon: 'warehouse'
  },
];

export type TransportationModeEnum = 'air' | 'ocean' | 'truck' | 'rail' | 'unknown_transportation' | 'truck_intl' | 'warehouse_storage';

