// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';
import invariant from "invariant";

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";
import FloatInput from "latitude/FloatInput";
import Checkbox from "latitude/Checkbox";
import SelectInput from "latitude/select/SelectInput";

import isBlank from "react_lib/utils/isBlank";
import isPositive from "react_lib/utils/isPositive";
import { DBID_LOCALLY_CREATED } from "react_lib/constants";
import FormField from "react_lib/form/FormField";

import { type NumberOfBathroomsEnum, NumberOfBathroomsEnumPairs } from "../constants/NumberOfBathroomsEnum";

export type Unit = {|
  +id: string,
  +dbid: number,
  +number: string,
  +squareFeet: ?number,
  +numberOfBedrooms: number,
  +numberOfBathrooms: NumberOfBathroomsEnum,
  +hasBalcony: boolean,
|};

type Props = {|
  +unit: Unit,
  +onChange: (unit: Unit) => void,
|};
export default function UnitEdit(props: Props): React.Node {
  const { unit, onChange } = props;
  const { number, squareFeet, numberOfBedrooms, numberOfBathrooms, hasBalcony } = unit;

  return (
    <Group>
      <FormField 
        label="Number:" 
        indicateRequired={true}
        errorMessageFullContext="Unit Number is mandatory"
        errorMessage={isBlank(number) ? "Mandatory" : null}
      >
        <TextInput
          value={number}
          onChange={(value) => {
            onChange({ ...unit, number: value })
          }}
        />
      </FormField>
      
      <FormField label="Square Feet:">
        <FloatInput
          value={squareFeet}
          onChange={(value) => {
            onChange({ ...unit, squareFeet: value })
          }}
        />
      </FormField>
      
      <FormField 
        label="Bedrooms:" 
        indicateRequired={true}
        errorMessageFullContext="Number of bedrooms is mandatory"
        errorMessage={!isPositive(numberOfBedrooms) ? "Mandatory" : null}
      >
        <FloatInput
          value={numberOfBedrooms}
          onChange={(value) => {
            // The || 0 is a nasty hack to keep Flow happy (for now)
            // Most likely, the effect will be that when user deletes content,
            // they may get a spurious zero, which would be super-annoying!
            onChange({ ...unit, numberOfBedrooms: value || 0 })
          }}
        />
      </FormField>
      
      <FormField 
        label="Bathrooms:" 
        indicateRequired={true}
      >
        <SelectInput
          value={numberOfBathrooms}
          options={NumberOfBathroomsEnumPairs}
          onChange={(value) => {
            invariant(value, "Select Input configured to not return null values - should never happen");
            onChange({ ...unit, numberOfBathrooms: value })
          }}
        />
      </FormField>
      
      <FormField label="Has Balcony?">
        <Checkbox
          checked={hasBalcony}
          onChange={(value) => {
            onChange({ ...unit, hasBalcony: value })
          }}
        />
      </FormField>
    </Group>
  );
}

export function createDefaultUnit(): Unit {
  return {
    id: uuid(),
    dbid: DBID_LOCALLY_CREATED,
    hasBalcony: false,
    number: "", 
    numberOfBathrooms: "ONE", 
    // $FlowExpectedError Making this null forces user to enter input
    numberOfBedrooms: null,
    squareFeet: null, 
  };
}
