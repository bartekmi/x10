// @flow

import * as React from "react";

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";

import isEmpty from "../lib_components/utils/isEmpty";
import { DBID_LOCALLY_CREATED } from "../lib_components/constants";
import FormField from "../lib_components/form/FormField";

export type Address = {|
  +city: ?string,
  +dbid: number,
  +stateOrProvince: ?string,
  +theAddress: ?string,
  +unitNumber: ?string,
  +zip: ?string,
|};

type Props = {|
  +address: Address,
  +onChange: (address: Address) => void,
|};
export default function AddressEditPage(props: Props): React.Node {
  const { address, onChange } = props;
  const { unitNumber, theAddress, city } = address;

  return (
    <Group flexDirection="column">
      <FormField label="Unit Number:" >
        <TextInput
          value={unitNumber || ""}
          onChange={(value) => {
            onChange({ ...address, unitNumber: value })
          }}
        />
      </FormField>
      <FormField 
        label="Address:" 
        indicateRequired={true}
        errorMessage={isEmpty(theAddress) ? "Address is mandatory" : null}
      >
          <TextInput
            value={theAddress || ""}
            onChange={(value) => {
              onChange({ ...address, theAddress: value })
            }}
          />
      </FormField>

      <FormField 
        label="City:" 
        indicateRequired={true}
        errorMessage={isEmpty(city) ? "City is mandatory" : null}
        toolTip="If this is a rural address, enter the closest city or village"
      >
        <TextInput
          value={city || ""}
          onChange={(value) => {
            onChange({ ...address, city: value })
          }}
        />
      </FormField>
    </Group>
  );
}

export function createDefaultAddress(): Address {
  return {
    dbid: DBID_LOCALLY_CREATED,
    city: "",
    stateOrProvince: "",
    theAddress: "",
    unitNumber: "",
    zip: ""
  };
}
