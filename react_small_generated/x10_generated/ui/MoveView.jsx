// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import FormSection from 'react_lib/form/FormSection';
import TextInput from 'react_lib/latitude_wrappers/TextInput';
import Separator from 'react_lib/Separator';

import { type Move } from 'entities/Move';
import BuildingView from 'ui/BuildingView';

import { type MoveView_move } from './__generated__/MoveView_move.graphql';



type Props = {|
  +move: MoveView_move,
|};
function MoveView(props: Props): React.Node {
  const { move } = props;

  return (
    <DisplayForm>
      <Text
        scale='display'
        children='Move Details'
      />
      <Separator/>
      <FormSection
        label='Moving from location...'
      >
        <BuildingView building={ move.from }/>
      </FormSection>
      <FormSection
        label='Moving to location...'
      >
        <BuildingView building={ move.to }/>
      </FormSection>
      <FormSection
        label='Tenant details...'
      >
        <Group
          gap={ 40 }
        >
          <DisplayField
            label='Name'
          >
            <TextInput
              value={ move.tenant?.name }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            label='Phone'
          >
            <TextInput
              value={ move.tenant?.phone }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            label='Email'
          >
            <TextInput
              value={ move.tenant?.email }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
          <DisplayField
            label='The Address'
          >
            <TextInput
              value={ move.tenant?.permanentMailingAddress?.theAddress }
              onChange={ () => { } }
              readOnly={ true }
            />
          </DisplayField>
        </Group>
      </FormSection>
    </DisplayForm>
  );
}

// $FlowExpectedError
export default createFragmentContainer(MoveView, {
  move: graphql`
    fragment MoveView_move on Move {
      id
      from {
        id
        ...BuildingView_building
      }
      tenant {
        id
        email
        name
        permanentMailingAddress {
          id
          theAddress
        }
        phone
      }
      to {
        id
        ...BuildingView_building
      }
    }
  `,
});

