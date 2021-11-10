// TEAM: compliance
// @flow

import * as React from 'react';
import { Route } from 'react-router-dom';

import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';

import HitsInterface from 'dps/ui/HitsInterface';
import SettingsEditorInterface from 'dps/ui/SettingsEditorInterface';
import SlideoutPanelInterface from 'dps/ui/SlideoutPanelInterface';
import WorkspaceTabsInterface from 'dps/ui/WorkspaceTabsInterface';



type Props = {|
|};
export default function Header(props: Props): React.Node {
  const {  } = props;

  return (
    <VerticalStackPanel>
      <Menu>
        <MenuItem
          label='Hits'
          href='/hits'
        />
        <MenuItem
          label='Settings'
          href='/settings'
        />
      </Menu>
      <SpaContent
        rootComponent={ HitsInterface }
      >
        <Route exact path='/hits/panel/:id' component={ SlideoutPanelInterface } />
        <Route exact path='/hits' component={ HitsInterface } />
        <Route exact path='/settings/edit/:id' component={ SettingsEditorInterface } />
        <Route exact path='/settings/new' component={ SettingsEditorInterface } />
        <Route exact path='/hits/:id' component={ WorkspaceTabsInterface } />
      </SpaContent>
    </VerticalStackPanel>
  );
}

