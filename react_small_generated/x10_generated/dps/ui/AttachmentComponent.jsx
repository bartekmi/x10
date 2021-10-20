// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Group from 'latitude/Group';
import Icon from 'latitude/Icon';

import Button from 'react_lib/latitude_wrappers/Button';
import StyleControl from 'react_lib/StyleControl';

import { type Attachment } from 'dps/entities/Attachment';

import { type AttachmentComponent_attachment } from './__generated__/AttachmentComponent_attachment.graphql';



type Props = {|
  +attachment: AttachmentComponent_attachment,
|};
function AttachmentComponent(props: Props): React.Node {
  const { attachment } = props;

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
          label={ attachment?.filename }
          url={ attachment?.url }
        />
      </Group>
    </StyleControl>
  );
}

// $FlowExpectedError
export default createFragmentContainer(AttachmentComponent, {
  attachment: graphql`
    fragment AttachmentComponent_attachment on Attachment {
      id
      filename
      url
    }
  `,
});

