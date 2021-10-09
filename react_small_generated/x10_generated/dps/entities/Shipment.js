// This file was auto-generated by x10. Do not modify by hand.
// @flow


import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import x10toString from 'react_lib/utils/x10toString';

import { type CompanyEntity } from 'dps/entities/CompanyEntity';


// Type Definition
export type Shipment = {
  +id: string,
  +coreId: ?number,
  +name: string,
  +transportationMode: ?TransportationModeEnum,
  +status: string,
  +customs: string,
  +cargoReady: ?string,
  +departsDate: ?string,
  +departsLocation: string,
  +arrivesDate: ?string,
  +arrivesLocation: string,
  +dueDate: ?string,
  +dueDateTask: string,
  +isLcl: boolean,
  +isLtl: boolean,
  +consignee: CompanyEntity,
  +shipper: CompanyEntity,
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



// Derived Attribute Functions
export function shipmentFlexId(shipment: ?{
  +coreId: ?number,
}): ?string {
  if (shipment == null) return null;
  const result = 'Flex-' + x10toString(shipment?.coreId);
  return result;
}



// Create Default Function
export function createDefaultShipment(): Shipment {
  return {
    id: uuid(),
    coreId: null,
    name: '',
    transportationMode: null,
    status: '',
    customs: '',
    cargoReady: null,
    departsDate: null,
    departsLocation: '',
    arrivesDate: null,
    arrivesLocation: '',
    dueDate: null,
    dueDateTask: '',
    isLcl: false,
    isLtl: false,
    consignee: null,
    shipper: null,
  };
}


// Validations
export function shipmentCalculateErrors(shipment: Shipment, prefix?: string): $ReadOnlyArray<FormError> {
  const errors = [];
  if (shipment == null ) return errors;


  return errors;
}

