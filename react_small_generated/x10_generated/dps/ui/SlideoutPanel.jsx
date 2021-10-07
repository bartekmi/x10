// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { createFragmentContainer, graphql } from 'react-relay';

import Text from 'latitude/Text';

import { type Hit } from 'dps/entities/Hit';

import { type SlideoutPanel_hit } from './__generated__/SlideoutPanel_hit.graphql';



type Props = {|
  +hit: SlideoutPanel_hit,
|};
function SlideoutPanel(props: Props): React.Node {
  const { hit } = props;

  return (
    <Text
      scale='display'
      children='TBD Slide-out Panel'
    />
  );
}

// $FlowExpectedError
export default createFragmentContainer(SlideoutPanel, {
  hit: graphql`
    fragment SlideoutPanel_hit on Hit {
      id
    }
  `,
});

