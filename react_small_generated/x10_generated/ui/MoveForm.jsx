// This file was auto-generated by x10. Do not modify by hand.
// @flow

import { moveCalculateErrors, type Move } from 'entities/Move';
import Group from 'latitude/Group';
import Text from 'latitude/Text';
import * as React from 'react';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import CalendarDateInput from 'react_lib/latitude_wrappers/CalendarDateInput';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import Separator from 'react_lib/Separator';
import { createFragmentContainer, graphql } from 'react-relay';


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
          id={ move.from }
          onChange={ (value) => {
            onChange({ ...move, from: value })
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
          id={ move.to }
          onChange={ (value) => {
            onChange({ ...move, to: value })
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
          id={ move.tenant }
          onChange={ (value) => {
            onChange({ ...move, tenant: value })
          } }
          isNullable={ false }
          query={ tenantsQuery }
          toString={ x => x.toStringRepresentation }
        />
      </FormField>
      <Group
        justifyContent='space-between'
      >
        <Text
          children='* Required'
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
    from: relay.from?.id,
    to: relay.to?.id,
    tenant: relay.tenant?.id,
  };
}

function save(move: Move) {
  const variables = {
    id: move.id,
    date: move.date,
    from: move.from,
    to: move.to,
    tenant: move.tenant,
  };

  basicCommitMutation(mutation, variables);
}

const mutation = graphql`
  mutation MoveFormMutation(
    $id: String!
    $date: DateTime!
    $from: String!
    $to: String!
    $tenant: String!
  ) {
    createOrUpdateMove(
      id: $id
      date: $date
      fromId: $from
      toId: $to
      tenantId: $tenant
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
      }
      tenant {
        id
      }
      to {
        id
      }
    }
  `,
});

const buildingsQuery = graphql`
  query MoveForm_buildingsQuery {
    entities: buildings {
      nodes {
        id
        toStringRepresentation
      }
    }
  }
`;

const tenantsQuery = graphql`
  query MoveForm_tenantsQuery {
    entities: tenants {
      nodes {
        id
        toStringRepresentation
      }
    }
  }
`;

