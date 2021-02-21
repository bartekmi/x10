// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Text from 'latitude/Text';

import TextDisplay from 'react_lib/display/TextDisplay';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import CalendarDateInput from 'react_lib/latitude_wrappers/CalendarDateInput';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';

import { moveCalculateErrors, type Move } from 'small/entities/Move';



type Props = {|
  +move: Move,
  +onChange: (move: Move) => void,
|};
function MoveForm(props: Props): React.Node {
  const { move, onChange } = props;

  return (
    <FormProvider
      value={ { errors: moveCalculateErrors(move) } }
    >
      <Text
        scale='display'
        children='New Move'
      />
      <Separator/>
      <FormField
        editorFor='date'
        label='Date'
      >
        <CalendarDateInput
          value={ move.date }
          onChange={ (value) => {
            onChange({ ...move, date: value })
          } }
        />
      </FormField>
      <FormField
        editorFor='from'
        label='From'
      >
        <AssociationEditor
          id={ move.from?.id }
          onChange={ (value) => {
            onChange({ ...move, from: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ buildingsQuery }
          toString={ x => x.toStringRepresentation }
        />
      </FormField>
      <FormField
        editorFor='to'
        label='To'
      >
        <AssociationEditor
          id={ move.to?.id }
          onChange={ (value) => {
            onChange({ ...move, to: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ buildingsQuery }
          toString={ x => x.toStringRepresentation }
        />
      </FormField>
      <FormField
        editorFor='tenant'
        label='Tenant'
      >
        <AssociationEditor
          id={ move.tenant?.id }
          onChange={ (value) => {
            onChange({ ...move, tenant: value == null ? null : { id: value } })
          } }
          isNullable={ false }
          query={ tenantsQuery }
          toString={ x => x.toStringRepresentation }
        />
      </FormField>
      <Group
        justifyContent='space-between'
      >
        <TextDisplay
          value='* Required'
        />
        <FormSubmitButton
          onClick={ () => save(move) }
          action={
            {
              successUrl: '/moves',
            }
          }
        />
      </Group>
    </FormProvider>
  );
}

type StatefulProps = {|
  +move: Move,
|};
export function MoveFormStateful(props: StatefulProps): React.Node {
  const move = relayToInternal(props.move);
  const [editedMove, setEditedMove] = React.useState(move);
  return <MoveForm
    move={ editedMove }
    onChange={ setEditedMove }
  />
}

function relayToInternal(relay: any): Move {
  return {
    ...relay,
  };
}

function save(move: Move) {
  basicCommitMutation(mutation, { move });
}

const mutation = graphql`
  mutation MoveFormMutation(
    $move: MoveInput!
  ) {
    createOrUpdateMove(
      move: $move
    )
  }
`;

// $FlowExpectedError
export default createFragmentContainer(MoveFormStateful, {
  move: graphql`
    fragment MoveForm_move on Move {
      id
      date
      from {
        id
        toStringRepresentation
      }
      tenant {
        id
        toStringRepresentation
      }
      to {
        id
        toStringRepresentation
      }
    }
  `,
});

const buildingsQuery = graphql`
  query MoveForm_buildingsQuery {
    entities: buildings {
      id
      toStringRepresentation
    }
  }
`;

const tenantsQuery = graphql`
  query MoveForm_tenantsQuery {
    entities: tenants {
      id
      toStringRepresentation
    }
  }
`;

