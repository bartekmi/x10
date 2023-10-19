// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';
import toEnum from 'react_lib/utils/toEnum';
import x10toString from 'react_lib/utils/x10toString';

import { type CompanyEntity } from 'dps/entities/CompanyEntity';
import { type Port } from 'dps/entities/Port';
import { type TransportationModeEnum } from 'dps/sharedEnums';


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

export function shipmentModeSubtext(shipment: ?{
  +transportationMode: ?TransportationModeEnum,
  +isLcl: boolean,
  +isLtl: boolean,
}): string {
  if (shipment == null) return '';
  const result = toEnum(shipment?.transportationMode) == "ocean" ? (shipment?.isLcl ? 'LCL' : 'FCL') : toEnum(shipment?.transportationMode) == "truck" ? (shipment?.isLtl ? 'LTL' : 'FTL') : '';
  return result;
}



// Create Default Function
export function createDefaultShipment(): Shipment {
  return {
    id: uuid(),
    dbid: null,
    flexId: '',
    name: '',
    // $FlowExpectedError Required field, but no default value
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
export function shipmentCalculateErrors(shipment: Shipment, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (shipment == null ) return errors;

  if (isBlank(shipment.priority))
    addError(errors, prefix, 'Priority is required', ['priority'], inListIndex);
  if (isBlank(shipment.transportationMode))
    addError(errors, prefix, 'Transportation Mode is required', ['transportationMode'], inListIndex);

  return errors;
}
