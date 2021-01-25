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
import MoveFormInterface from 'ui/MoveFormInterface';
import MovesInterface from 'ui/MovesInterface';
import MoveViewInterface from 'ui/MoveViewInterface';
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
          label='New Building'
          href='/buildings/new'
        />
        <MenuItem
          label='All Tenants'
          href='/tenants'
        />
        <MenuItem
          label='New Tenant'
          href='/tenants/new'
        />
        <MenuItem
          label='All Moves'
          href='/moves'
        />
        <MenuItem
          label='New Move'
          href='/moves/new'
        />
      </Menu>
      <SpaContent
        rootComponent={ BuildingsInterface }
      >
        <Route exact path='/moves/edit/:id' component={ MoveFormInterface } />
        <Route exact path='/moves/new' component={ MoveFormInterface } />
        <Route exact path='/tenants' component={ TenantsInterface } />
        <Route exact path='/moves' component={ MovesInterface } />
        <Route exact path='/buildings' component={ BuildingsInterface } />
        <Route exact path='/moves/view/edit/:id' component={ MoveViewInterface } />
        <Route exact path='/moves/view/new' component={ MoveViewInterface } />
        <Route exact path='/buildings/edit/:id' component={ BuildingFormInterface } />
        <Route exact path='/buildings/new' component={ BuildingFormInterface } />
        <Route exact path='/tenants/edit/:id' component={ TenantFormInterface } />
        <Route exact path='/tenants/new' component={ TenantFormInterface } />
      </SpaContent>
    </Group>
  );
}

