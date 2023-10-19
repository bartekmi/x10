// TEAM: compliance
// @flow

import * as React from 'react';
import { graphql, QueryRenderer } from 'react-relay';

import EntityQueryRenderer from 'react_lib/relay/EntityQueryRenderer';

import { createDefaultSettings } from 'dps/entities/Settings';
import SettingsEditor, { SettingsEditorStateful } from 'dps/ui/SettingsEditor';
import environment from 'environment';



type Props = { 
  +id?: string,      // When invoked from another Component
  +match?: {         // When invoked via Route
    +params: { 
      +id: string
    }
  }
};
export default function SettingsEditorInterface(props: Props): React.Node {
  return (
    <EntityQueryRenderer
      id={ props.id }
      match={ props.match }
      createComponentFunc={ (settings) => <SettingsEditor settings={ settings }/> }
      createComponentFuncNew={ () => <SettingsEditorStateful settings={ createDefaultSettings() }/> }
      query={ query }
    />
  );
}

const query = graphql`
  query SettingsEditorInterfaceQuery($id: String!) {
    entity: settings(id: $id) {
      ...SettingsEditor_settings
    }
  }
`;

