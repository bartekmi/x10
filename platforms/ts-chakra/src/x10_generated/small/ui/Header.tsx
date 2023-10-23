import * as React from 'react';
import { Route } from 'react-router-dom';

import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';

import { AppContext } from 'SmallAppContext';
import BuildingFormInterface from 'x10_generated/small/ui/BuildingFormInterface';
import BuildingsInterface from 'x10_generated/small/ui/BuildingsInterface';
import BuildingViewInterface from 'x10_generated/small/ui/BuildingViewInterface';
import MoveFormInterface from 'x10_generated/small/ui/MoveFormInterface';
import MovesInterface from 'x10_generated/small/ui/MovesInterface';
import MoveViewInterface from 'x10_generated/small/ui/MoveViewInterface';
import TenantFormInterface from 'x10_generated/small/ui/TenantFormInterface';
import TenantsInterface from 'x10_generated/small/ui/TenantsInterface';
import TenantViewInterface from 'x10_generated/small/ui/TenantViewInterface';



type Props = {
};
export default function Header(props: Props): React.JSX.Element {
  const {  } = props;
  const appContext = React.useContext(AppContext);

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
        rootComponent={ <BuildingsInterface/> }
      >
        <Route path='/moves/edit/:id' element={ <MoveFormInterface/> } />
        <Route path='/moves/new' element={ <MoveFormInterface/> } />
        <Route path='/tenants' element={ <TenantsInterface/> } />
        <Route path='/moves' element={ <MovesInterface/> } />
        <Route path='/buildings' element={ <BuildingsInterface/> } />
        <Route path='/moves/view/:id' element={ <MoveViewInterface/> } />
        <Route path='/buildings/edit/:id' element={ <BuildingFormInterface/> } />
        <Route path='/buildings/new' element={ <BuildingFormInterface/> } />
        <Route path='/tenants/view/:id' element={ <TenantViewInterface/> } />
        <Route path='/tenants/edit/:id' element={ <TenantFormInterface/> } />
        <Route path='/tenants/new' element={ <TenantFormInterface/> } />
        <Route path='/buildings/view/:id' element={ <BuildingViewInterface/> } />
      </SpaContent>
    </VerticalStackPanel>
  );
}

