// @flow

import * as React from "react";

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";
import Label from "latitude/Label";
import InputError from "latitude/InputError";

import isEmpty from "../lib_components/utils/isEmpty";
import { DBID_LOCALLY_CREATED } from "../lib_components/constants";

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
      <Label value="Unit Number:" >
        <TextInput
          value={unitNumber || ""}
          onChange={(value) => {
            onChange({ ...address, unitNumber: value })
          }}
        />
      </Label>
      <Label value="Address:" >
        <InputError errorText="Address is mandatory" showError={isEmpty(theAddress)}>
          <TextInput
            value={theAddress || ""}
            onChange={(value) => {
              onChange({ ...address, theAddress: value })
            }}
          />
        </InputError>
      </Label>
      <Label value="City:" >
        <InputError errorText="City is mandatory" showError={isEmpty(city)}>
          <TextInput
            value={city || ""}
            onChange={(value) => {
              onChange({ ...address, city: value })
            }}
          />
        </InputError>
      </Label>
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
