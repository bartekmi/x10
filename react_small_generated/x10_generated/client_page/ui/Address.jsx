// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import HelpTooltip from 'latitude/HelpTooltip';

import Button from 'react_lib/latitude_wrappers/Button';
import TextInput from 'react_lib/latitude_wrappers/TextInput';

import { type Address } from 'client_page/entities/Address';

import { type Address_address } from './__generated__/Address_address.graphql';



type Props = {|
  +address: Address_address,
|};
function Address(props: Props): React.Node {
  const { address } = props;

  return (
    <Group
      flexDirection='column'
    >
      <TextInput
        value={ address.theAddress }
        onChange={ (value) => {
          onChange({ ...address, theAddress: value })
        } }
      />
      <TextInput
        value={ address.theAddress2 }
        onChange={ (value) => {
          onChange({ ...address, theAddress2: value })
        } }
      />
      <TextInput
        value={ address.city }
        onChange={ (value) => {
          onChange({ ...address, city: value })
        } }
      />
      <TextInput
        value={ address.postalCode }
        onChange={ (value) => {
          onChange({ ...address, postalCode: value })
        } }
      />
      <TextInput
        value={ address.country.name }
        onChange={ (value) => {
          let newObj = JSON.parse(JSON.stringify(address));
          newObj.country.name = value;
          onChange(newObj);
        } }
      />
      <Button
        label='Verify'
      />
      <Group>
        <Button
          label='Make Location'
        />
        <HelpTooltip
          text='Create a location with this address...'
        />
      </Group>
    </Group>
  );
}

// $FlowExpectedError
export default createFragmentContainer(Address, {
  address: graphql`
    fragment Address_address on Address {
      id
      city
      country {
        id
        name
      }
      postalCode
      theAddress
      theAddress2
    }
  `,
});

