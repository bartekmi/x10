// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';
import SelectInput from 'latitude/select/SelectInput';
import TextareaInput from 'latitude/TextareaInput';

import TextDisplay from 'react_lib/display/TextDisplay';
import RadioGroup from 'react_lib/enum/RadioGroup';
import FormField from 'react_lib/form/FormField';
import FormProvider from 'react_lib/form/FormProvider';
import FormSubmitButton from 'react_lib/form/FormSubmitButton';
import AssociationEditor from 'react_lib/multi/AssociationEditor';
import basicCommitMutation from 'react_lib/relay/basicCommitMutation';
import StyleControl from 'react_lib/StyleControl';
import toEnum from 'react_lib/utils/toEnum';

import { hitCalculateErrors, type Hit } from 'dps/entities/Hit';
import { HitStatusEnumPairs, ReasonForCleranceEnumPairs } from 'dps/sharedEnums';



type Props = {|
  +hit: Hit,
  +onChange: (hit: Hit) => void,
|};
function ClearanceForm(props: Props): React.Node {
  const { hit, onChange } = props;

  return (
    <FormProvider
      value={ { errors: hitCalculateErrors(hit) } }
    >
      <Group
        alignItems='center'
        gap={ 100 }
      >
        <TextDisplay
          value='Is this a denid party?'
        />
        <FormField
          editorFor='status'
          label=''
        >
          <RadioGroup
            value={ hit?.status }
            onChange={ (value) => {
              // $FlowExpectedError
              onChange({ ...hit, status: value })
            } }
            excludeItems='unresolved'
            options={ HitStatusEnumPairs }
          />
        </FormField>
      </Group>
      <StyleControl
        visible={ toEnum(hit?.status) == "denied" }
        width={ 800 }
        padding={ 10 }
        borderColor='red'
        borderWidth={ 1.5 }
      >
        <Group
          alignItems='center'
        >
          <Icon
            iconName='attention'
            color='red30'
          />
          <TextDisplay
            value='Shipments related to this entity will remain blocked'
          />
        </Group>
      </StyleControl>
      <StyleControl
        visible={ toEnum(hit?.status) != "denied" }
      >
        <Group
          alignItems='center'
          gap={ 32 }
        >
          <FormField
            editorFor='reasonForClearance'
            indicateRequired={ true }
            label='Reason For Clearance'
          >
            <SelectInput
              value={ hit?.reasonForClearance }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...hit, reasonForClearance: value })
              } }
              options={ ReasonForCleranceEnumPairs }
            />
          </FormField>
          <FormField
            editorFor='whitelistDays'
            indicateRequired={ true }
            label='Whitelist time'
          >
            <AssociationEditor
              id={ hit?.whitelistDays?.id }
              onChange={ (value) => {
                // $FlowExpectedError
                onChange({ ...hit, whitelistDays: value == null ? null : { id: value } })
              } }
              isNullable={ false }
              query={ whitelistDurationsQuery }
              toString={ x => x.toStringRepresentation }
            />
          </FormField>
        </Group>
      </StyleControl>
      <StyleControl
        maxWidth={ 800 }
      >
        <FormField
          editorFor='notes'
          indicateRequired={ true }
          label='Notes'
        >
          <TextareaInput
            value={ hit?.notes }
            onChange={ (value) => {
              // $FlowExpectedError
              onChange({ ...hit, notes: value })
            } }
            rows={ 3 }
          />
        </FormField>
      </StyleControl>
      <StyleControl
        marginTop={ 30 }
      >
        <FormSubmitButton
          label={ toEnum(hit?.status) != "denied" ? 'Clear the hit' : 'Confirm' }
          mutation={mutation}
          variables={hit}
          successMessage="Hit updated successfully."
          errorMessage="There was a problem. Hit not saved."
        />
      </StyleControl>
    </FormProvider>
  );
}

type StatefulProps = {|
  +hit: Hit,
|};
export function ClearanceFormStateful(props: StatefulProps): React.Node {
  const hit = relayToInternal(props.hit);
  const [editedHit, setEditedHit] = React.useState(hit);
  return <ClearanceForm
    hit={ editedHit }
    onChange={ setEditedHit }
  />
}

function relayToInternal(relay: any): Hit {
  return {
    ...relay,
  };
}

const mutation = graphql`
  mutation ClearanceFormMutation(
    $hit: HitInput!
  ) {
    createOrUpdateHit(
      hit: $hit
    )
  }
`;

// $FlowExpectedError
export default createFragmentContainer(ClearanceFormStateful, {
  hit: graphql`
    fragment ClearanceForm_hit on Hit {
      id
      notes
      reasonForClearance
      status
      whitelistDays {
        id
        toStringRepresentation
      }
    }
  `,
});

const whitelistDurationsQuery = graphql`
  query ClearanceForm_whitelistDurationsQuery {
    entities: whitelistDurations {
      id
      toStringRepresentation
    }
  }
`;

