// This file was auto-generated on 01/10/2021 22:40:08. Do not modify by hand.
// @flow

import * as React from 'react';

import history from 'history';
import Group from 'latitude/Group';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import { Route, Router } from 'react-router-dom';
import BuildingForm from 'ui/BuildingForm';
import Buildings from 'ui/Buildings';
import TenantForm from 'ui/TenantForm';
import Tenants from 'ui/Tenants';


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
          label='All Buildings'
          href='/buildings'
        />
        <MenuItem
          label='Create New Building'
          href='/building/new'
        />
        <MenuItem
          label='All Tenants'
          href='/tenants'
        />
        <MenuItem
          label='Create New Tenant'
          href='/tenant/new'
        />
      </Menu>
      <Router
        history={ history }
      >
        <Route exact path='/tenants' component={ Tenants } />
        <Route exact path='/buildings' component={ Buildings } />
        <Route exact path='/building/{$buildingId}/edit/:id' component={ BuildingForm } />
        <Route exact path='/building/{$buildingId}/new' component={ BuildingForm } />
        <Route exact path='/tenant/{$tenantId}/edit/:id' component={ TenantForm } />
        <Route exact path='/tenant/{$tenantId}/new' component={ TenantForm } />
      </Router>
    </Group>
  );
}

