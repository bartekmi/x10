import { gql } from '@apollo/client';
import { Heading } from '@chakra-ui/react';
import * as React from 'react';

import TextDisplay from 'react_lib/display/TextDisplay';
import DisplayField from 'react_lib/form/DisplayField';
import DisplayForm from 'react_lib/form/DisplayForm';
import FormSection from 'react_lib/form/FormSection';
import Group from 'react_lib/Group';
import Separator from 'react_lib/Separator';

import { AppContext } from 'SmallAppContext';
import { type Move } from 'x10_generated/small/entities/Move';
import BuildingView from 'x10_generated/small/ui/BuildingView';



type Props = {
  readonly move?: Move,
};
export default function MoveView(props: Props): React.JSX.Element {
  const { move } = props;
  const appContext = React.useContext(AppContext);

  return (
    <DisplayForm>
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children='Move Details'
      />
      <Separator/>
      <FormSection
        label='Moving from location...'
      >
        <BuildingView building={ move?.from }/>
      </FormSection>
      <FormSection
        label='Moving to location...'
      >
        <BuildingView building={ move?.to }/>
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
            <TextDisplay
              value={ move?.tenant?.name }
            />
          </DisplayField>
          <DisplayField
            label='Phone'
          >
            <TextDisplay
              value={ move?.tenant?.phone }
            />
          </DisplayField>
          <DisplayField
            label='Email'
          >
            <TextDisplay
              value={ move?.tenant?.email }
            />
          </DisplayField>
          <DisplayField
            label='The Address'
          >
            <TextDisplay
              value={ move?.tenant?.permanentMailingAddress?.theAddress }
            />
          </DisplayField>
        </Group>
      </FormSection>
    </DisplayForm>
  );
}

export const MOVEVIEW_MOVE_FRAGMENT = gql`
  fragment MoveView_Move on Move {
    id
    from {
      id
      ...BuildingView_Building
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
      ...BuildingView_Building
    }
  }
`

