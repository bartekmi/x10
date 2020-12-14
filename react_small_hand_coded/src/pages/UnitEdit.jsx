// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';
import invariant from "invariant";

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";
import FloatInput from "latitude/FloatInput";
import Checkbox from "latitude/Checkbox";
import SelectInput from "latitude/select/SelectInput";

import isEmpty from "../lib_components/utils/isEmpty";
import isPositive from "../lib_components/utils/isPositive";
import { DBID_LOCALLY_CREATED } from "../lib_components/constants";
import FormField from "../lib_components/form/FormField";

import NumberOfBathroomsEnum from "../constants/NumberOfBathroomsEnum";
import {type NumberOfBathroomsEnum as NumberOfBathroomsEnumGql} from "./__generated__/BuildingEditPageQuery.graphql"

export type Unit = {|
  +id: string,
  +dbid: number,
  +number: string,
  +squareFeet: ?number,
  // We would like number here to be optional so we can handle null user input in a 
  // legitimate way. But that breaks our nice paradign of using GraphQL data types
  // to directly populate the UI. In particular, some fields that are mandatory as
  // far as the data model is concerned (i.e. GraphQL) should be nullable before the
  // user has had a chance to enter them. This is a systemic problem.
  +numberOfBedrooms: number,
  +numberOfBathrooms: NumberOfBathroomsEnumGql,
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
        errorMessage={isEmpty(number) ? "Mandatory" : null}
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
          options={NumberOfBathroomsEnum}
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
    numberOfBedrooms: 0, 
    squareFeet: null, 
  };
}
