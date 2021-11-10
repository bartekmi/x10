// TEAM: compliance
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';

import Button from 'react_lib/latitude_wrappers/Button';
import StyleControl from 'react_lib/StyleControl';

import { type DpsAttachment } from 'dps/entities/DpsAttachment';

import { type AttachmentComponent_dpsAttachment } from './__generated__/AttachmentComponent_dpsAttachment.graphql';



type Props = {|
  +dpsAttachment: AttachmentComponent_dpsAttachment,
|};
function AttachmentComponent(props: Props): React.Node {
  const { dpsAttachment } = props;

  return (
    <StyleControl
      padding={ 10 }
      borderColor='silver'
    >
      <Group
        alignItems='center'
      >
        <Icon
          iconName='attach'
        />
        <Button
          label={ dpsAttachment?.filename }
          url={ dpsAttachment?.url }
        />
      </Group>
    </StyleControl>
  );
}

// $FlowExpectedError
export default createFragmentContainer(AttachmentComponent, {
  dpsAttachment: graphql`
    fragment AttachmentComponent_dpsAttachment on DpsAttachment {
      id
      filename
      url
    }
  `,
});

