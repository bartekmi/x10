// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { type CompanyEntity } from 'dps/entities/CompanyEntity';
import { type Port } from 'dps/entities/Port';


// Type Definition
export type Shipment = {
  +id: string,
  +dbid: ?number,
  +flexId: string,
  +name: string,
  +priority: ?ShipmentPriorityEnum,
  +transportationMode: ?TransportationModeEnum,
  +status: string,
  +cargoReadyDate: ?string,
  +actualDepartureDate: ?string,
  +arrivalDate: ?string,
  +isLcl: boolean,
  +isLtl: boolean,
  +customs: string,
  +dueDate: ?string,
  +dueDateTask: string,
  +consignee: ?CompanyEntity,
  +shipper: ?CompanyEntity,
  +departurePort: ?Port,
  +arrivalPort: ?Port,
};


// Enums
export const TransportationModeEnumPairs = [
  {
    value: 'air',
    label: 'Air',
  },
  {
    value: 'ocean',
    label: 'Ocean',
  },
  {
    value: 'truck',
    label: 'Truck',
  },
  {
    value: 'rail',
    label: 'Rail',
  },
  {
    value: 'unknown_transportation',
    label: 'Unknown Transportation',
  },
  {
    value: 'truck_intl',
    label: 'Truck Intl',
  },
  {
    value: 'warehouse_storage',
    label: 'Warehouse Storage',
  },
];

export type TransportationModeEnum = 'air' | 'ocean' | 'truck' | 'rail' | 'unknown_transportation' | 'truck_intl' | 'warehouse_storage';

export const ShipmentPriorityEnumPairs = [
  {
    value: 'standard',
    label: 'Standard',
  },
  {
    value: 'high',
    label: 'High',
  },
];

export type ShipmentPriorityEnum = 'standard' | 'high';



// Derived Attribute Functions
export function shipmentUrl(shipment: ?{
  +dbid: ?number,
}): string {
  if (shipment == null) return '';
  const result = '/shipments/' + x10toString(shipment?.dbid);
  return result;
}



// Create Default Function
export function createDefaultShipment(): Shipment {
  return {
    id: uuid(),
    dbid: null,
    flexId: '',
    name: '',
    priority: null,
    // $FlowExpectedError Required field, but no default value
    transportationMode: null,
    status: '',
    cargoReadyDate: null,
    actualDepartureDate: null,
    arrivalDate: null,
    isLcl: false,
    isLtl: false,
    customs: '',
    dueDate: null,
    dueDateTask: '',
    consignee: null,
    shipper: null,
    departurePort: null,
    arrivalPort: null,
  };
}


// Validations
export function shipmentCalculateErrors(shipment: Shipment, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (shipment == null ) return errors;

  if (isBlank(shipment.transportationMode))
    addError(errors, prefix, 'Transportation Mode is required', ['transportationMode']);

  return errors;
}

