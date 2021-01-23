// This file was auto-generated by x10. Do not modify by hand.
// @flow

import Group from 'latitude/Group';
import * as React from 'react';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';
import { Route } from 'react-router-dom';
import BuildingFormInterface from 'ui/BuildingFormInterface';
import BuildingsInterface from 'ui/BuildingsInterface';
import MovesInterface from 'ui/MovesInterface';
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
          href='/buildings/new'
        />
        <MenuItem
          label='All Tenants'
          href='/tenants'
        />
        <MenuItem
          label='Create New Tenant'
          href='/tenants/new'
        />
        <MenuItem
          label='All Moves'
          href='/moves'
        />
      </Menu>
      <SpaContent
        rootComponent={ BuildingsInterface }
      >
        <Route exact path='/tenants' component={ TenantsInterface } />
        <Route exact path='/moves' component={ MovesInterface } />
        <Route exact path='/buildings' component={ BuildingsInterface } />
        <Route exact path='/buildings/edit/:id' component={ BuildingFormInterface } />
        <Route exact path='/buildings/new' component={ BuildingFormInterface } />
        <Route exact path='/tenants/edit/:id' component={ TenantFormInterface } />
        <Route exact path='/tenants/new' component={ TenantFormInterface } />
      </SpaContent>
    </Group>
  );
}

