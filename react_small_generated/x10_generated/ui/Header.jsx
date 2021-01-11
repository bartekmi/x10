// This file was auto-generated on 01/11/2021 09:22:34. Do not modify by hand.
// @flow

import * as React from 'react';

import Group from 'latitude/Group';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';
import { Route } from 'react-router-dom';
import BuildingFormInterface from 'ui/BuildingFormInterface';
import BuildingsInterface from 'ui/BuildingsInterface';
import TenantFormInterface from 'ui/TenantFormInterface';
import TenantsInterface from 'ui/TenantsInterface';


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
      <SpaContent
        rootComponent={BuildingsInterface}
      >
        <Route exact path='/tenants' component={ TenantsInterface } />
        <Route exact path='/buildings' component={ BuildingsInterface } />
        <Route exact path='/building/edit/:id' component={ BuildingFormInterface } />
        <Route exact path='/building/new' component={ BuildingFormInterface } />
        <Route exact path='/tenant/edit/:id' component={ TenantFormInterface } />
        <Route exact path='/tenant/new' component={ TenantFormInterface } />
      </SpaContent>
    </Group>
  );
}

