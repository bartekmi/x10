// TEAM: compliance
// @flow

import { v4 as uuid } from 'uuid';

import { addError, type FormError } from 'react_lib/form/FormProvider';
import isBlank from 'react_lib/utils/isBlank';

import { cargoCalculateErrors, createDefaultCargo, type Cargo } from 'dps/entities/Cargo';
import { type CompanyEntity } from 'dps/entities/CompanyEntity';
import { createDefaultShipment, shipmentCalculateErrors, type Shipment } from 'dps/entities/Shipment';
import { type TransportationModeEnum } from 'dps/sharedEnums';


// Type Definition
export type Booking = {
  +id: string,
  +name: string,
  +ocean_fcl: boolean,
  +ocean_lcl: boolean,
  +truck_ftl: boolean,
  +truck_ltl: boolean,
  +air: boolean,
  +booking_stage: ?BookingStgeEnum,
  +createdAt: ?string,
  +cargo_ready_date: ?string,
  +shipper_entity: ?CompanyEntity,
  +consignee_entity: ?CompanyEntity,
  +cargo: Cargo,
  +shipment: Shipment,
};


// Enums
export const BookingStgeEnumPairs = [
  {
    value: 'archived',
    label: 'Archived',
  },
  {
    value: 'booked',
    label: 'Booked',
  },
  {
    value: 'draft',
    label: 'Draft',
  },
  {
    value: 'shipment',
    label: 'Shipment',
  },
  {
    value: 'submitted',
    label: 'Submitted',
  },
];

export type BookingStgeEnum = 'archived' | 'booked' | 'draft' | 'shipment' | 'submitted';



// Derived Attribute Functions
export function bookingTransportationMode(booking: ?{
  +ocean_fcl: boolean,
  +ocean_lcl: boolean,
  +truck_ftl: boolean,
  +truck_ltl: boolean,
  +air: boolean,
}): ?TransportationModeEnum {
  if (booking == null) return null;
  const result = booking?.ocean_fcl || booking?.ocean_lcl ? "ocean" : booking?.truck_ftl || booking?.truck_ltl ? "truck" : booking?.air ? "air" : "unknown_transportation";
  return result;
}



// Create Default Function
export function createDefaultBooking(): Booking {
  return {
    id: uuid(),
    name: '',
    ocean_fcl: false,
    ocean_lcl: false,
    truck_ftl: false,
    truck_ltl: false,
    air: false,
    // $FlowExpectedError Required field, but no default value
    booking_stage: null,
    // $FlowExpectedError Required field, but no default value
    createdAt: null,
    cargo_ready_date: null,
    shipper_entity: null,
    consignee_entity: null,
    cargo: createDefaultCargo(),
    shipment: createDefaultShipment(),
  };
}


// Validations
export function bookingCalculateErrors(booking: Booking, prefix?: string, inListIndex?: number): $ReadOnlyArray<FormError> {
  const errors = [];
  if (booking == null ) return errors;

  if (isBlank(booking.booking_stage))
    addError(errors, prefix, 'Booking_stage is required', ['booking_stage'], inListIndex);
  if (isBlank(booking.createdAt))
    addError(errors, prefix, 'Created At is required', ['createdAt'], inListIndex);
  if (isBlank(booking.shipper_entity))
    addError(errors, prefix, 'Shipper_entity is required', ['shipper_entity'], inListIndex);
  if (isBlank(booking.consignee_entity))
    addError(errors, prefix, 'Consignee_entity is required', ['consignee_entity'], inListIndex);

  errors.push(...cargoCalculateErrors(booking.cargo, 'cargo'));
  errors.push(...shipmentCalculateErrors(booking.shipment, 'shipment'));

  return errors;
}

