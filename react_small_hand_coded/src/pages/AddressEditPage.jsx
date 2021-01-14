// @flow

import * as React from "react";
import { v4 as uuid } from 'uuid';

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";

import isBlank from "react_lib/utils/isBlank";
import { DBID_LOCALLY_CREATED } from "react_lib/constants";
import FormField from "react_lib/form/FormField";

import { type Address, createDefaultAddress } from "entities/Address";

type Props = {|
  +address: Address,
  +onChange: (address: Address) => void,
|};
export default function AddressEditPage(props: Props): React.Node {
  const { address, onChange } = props;

  return (
    <Group flexDirection="column">
      <FormField label="Unit Number:" maxWidth={120}>
        <TextInput
          value={address.unitNumber}
          onChange={(value) => {
            onChange({ ...address, unitNumber: value })
          }}
        />
      </FormField>
      <FormField 
        label="Address:" 
        indicateRequired={true}
        errorMessage={isBlank(address.theAddress) ? "Address is mandatory" : null}
      >
          <TextInput
            value={address.theAddress}
            onChange={(value) => {
              onChange({ ...address, theAddress: value })
            }}
          />
      </FormField>

      <FormField 
        label="City:" 
        indicateRequired={true}
        errorMessage={isBlank(address.city) ? "City is mandatory" : null}
        toolTip="If this is a rural address, enter the closest city or village"
        maxWidth={350}
      >
        <TextInput
          value={address.city}
          onChange={(value) => {
            onChange({ ...address, city: value })
          }}
        />
      </FormField>
    </Group>
  );
}
