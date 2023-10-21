import { gql } from '@apollo/client';
import { Flex, Heading } from '@chakra-ui/react';
import * as React from 'react';

import CalendarDateInput from 'react_lib/chakra_wrappers/CalendarDateInput';
import TextDisplay from 'react_lib/display/TextDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import Separator from 'react_lib/Separator';

import { moveCalculateErrors, type Move } from 'x10_generated/small/entities/Move';

import { MoveForm_MoveFragment } from '__generated__/graphql';



type Props = {
  readonly move: MoveForm_MoveFragment,
  readonly onChange: (move: MoveForm_MoveFragment) => void,
};
function MoveForm(props: Props): React.JSX.Element {
  const { move, onChange } = props;

  return (
    <FormProvider
      context={ { errors: moveCalculateErrors(move as Move) } }
    >
      <Heading
        as='h1'
        size='4xl'
        noOfLines={ 1 }
        children='New Move'
      />
      <Separator/>
      <FormField
        editorFor='date'
        indicateRequired={ true }
        label='Date'
      >
        <CalendarDateInput
          value={ move?.date }
          onChange={ (value) => {
            onChange({ ...move, date: value })
          } }
        />
      </FormField>
      <FormField
        editorFor='from'
        indicateRequired={ true }
        label='From'
      >
        <AssociationEditor
          id={ move?.from?.id }
          onChange={ (value) => {
            onChange({ ...move, from: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ buildingsQuery }
        />
      </FormField>
      <FormField
        editorFor='to'
        indicateRequired={ true }
        label='To'
      >
        <AssociationEditor
          id={ move?.to?.id }
          onChange={ (value) => {
            onChange({ ...move, to: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ buildingsQuery }
        />
      </FormField>
      <FormField
        editorFor='tenant'
        indicateRequired={ true }
        label='Tenant'
      >
        <AssociationEditor
          id={ move?.tenant?.id }
          onChange={ (value) => {
            onChange({ ...move, tenant: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ tenantsQuery }
        />
      </FormField>
      <Flex
        justifyContent='space-between'
      >
        <TextDisplay
          value='* Required'
        />
        <FormSubmitButton
          mutation={ mutation }
          variables={
            {
              move: {
                id: move.id,
                date: move.date,
                from: move.from,
                to: move.to,
                tenant: move.tenant,
              }
            }
          }
          label='Save'
        />
      </Flex>
    </FormProvider>
  );
}

type StatefulProps = {
  readonly move: MoveForm_MoveFragment,
};
export function MoveFormStateful(props: StatefulProps): React.JSX.Element {
  const [editedMove, setEditedMove] = React.useState(props.move);
  return <MoveForm
    move={ editedMove }
    onChange={ setEditedMove }
  />
}

const mutation = gql`
  mutation MoveFormMutation(
    $move: MoveFormMoveInput!
  ) {
    moveFormUpdateMove(
      data: $move
    ) {
      id
      date
      from {
        id
      }
      tenant {
        id
      }
      to {
        id
      }
    }
  }
`;

  gql`
    fragment MoveForm_Move on Move {
      id
      date
      from {
        id
      }
      tenant {
        id
      }
      to {
        id
      }
    }
  `

const buildingsQuery = graphql`
  query MoveForm_buildingsQuery {
    entities: buildings {
      id
    }
  }
`;

const tenantsQuery = graphql`
  query MoveForm_tenantsQuery {
    entities: tenants {
      id
    }
  }
`;
