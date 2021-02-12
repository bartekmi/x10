// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { Route } from 'react-router-dom';

import Group from 'latitude/Group';

import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';

import ClientsInterface from 'client_page/ui/ClientsInterface';
import ClientViewInterface from 'client_page/ui/ClientViewInterface';



type Props = {|
|};
export default function Header(props: Props): React.Node {
  const {  } = props;

  return (
    <Group
      flexDirection='column'
    >
      <Menu>
        <MenuItem
          label='All Clients'
          href='/clients'
        />
      </Menu>
      <SpaContent
        rootComponent={ ClientsInterface }
      >
        <Route exact path='/clients/view/:id' component={ ClientViewInterface } />
        <Route exact path='/clients' component={ ClientsInterface } />
      </SpaContent>
    </Group>
  );
}
