// @flow

import * as React from "react";

import Group from "latitude/Group"
import TextInput from "latitude/TextInput";
import Label from "latitude/Label";

import {DBID_LOCALLY_CREATED} from "../lib_components/constants";

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

  return (
    <Group flexDirection="column">
      <Label value="Unit Number:" >
        <TextInput
          value={address.unitNumber || ""}
          onChange={(value) => {
            onChange({ ...address, unitNumber: value })
          }}
        />
      </Label>
      <Label value="Address:" >
        <TextInput
          value={address.theAddress || ""}
          onChange={(value) => {
            onChange({ ...address, theAddress: value })
          }}
        />
      </Label>
      <Label value="City:" >
        <TextInput
          value={address.city || ""}
          onChange={(value) => {
            onChange({ ...address, city: value })
          }}
        />
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
