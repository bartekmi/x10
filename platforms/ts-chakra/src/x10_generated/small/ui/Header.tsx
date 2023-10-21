import * as React from 'react';
import { Route } from 'react-router-dom';

import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';

import BuildingFormInterface from 'x10_generated/small/ui/BuildingFormInterface';
import BuildingsInterface from 'x10_generated/small/ui/BuildingsInterface';
import MoveFormInterface from 'x10_generated/small/ui/MoveFormInterface';
import MovesInterface from 'x10_generated/small/ui/MovesInterface';
import MoveViewInterface from 'x10_generated/small/ui/MoveViewInterface';
import TenantFormInterface from 'x10_generated/small/ui/TenantFormInterface';
import TenantsInterface from 'x10_generated/small/ui/TenantsInterface';



type Props = {
};
export default function Header(props: Props): React.JSX.Element {
  const {  } = props;

  return (
    <VerticalStackPanel>
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
        <Route exact path='/moves/view/:id' component={ MoveViewInterface } />
        <Route exact path='/buildings/edit/:id' component={ BuildingFormInterface } />
        <Route exact path='/buildings/new' component={ BuildingFormInterface } />
        <Route exact path='/tenants/edit/:id' component={ TenantFormInterface } />
        <Route exact path='/tenants/new' component={ TenantFormInterface } />
      </SpaContent>
    </VerticalStackPanel>
  );
}

