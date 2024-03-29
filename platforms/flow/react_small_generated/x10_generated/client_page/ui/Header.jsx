// This file was auto-generated by x10. Do not modify by hand.
// @flow

import * as React from 'react';
import { Route } from 'react-router-dom';

import VerticalStackPanel from 'react_lib/layout/VerticalStackPanel';
import Menu from 'react_lib/menu/Menu';
import MenuItem from 'react_lib/menu/MenuItem';
import SpaContent from 'react_lib/SpaContent';

import ClientsInterface from 'client_page/ui/ClientsInterface';
import ClientViewInterface from 'client_page/ui/ClientViewInterface';
import CompanyEntityFormInterface from 'client_page/ui/CompanyEntityFormInterface';



type Props = {|
|};
export default function Header(props: Props): React.Node {
  const {  } = props;

  return (
    <VerticalStackPanel>
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
        <Route exact path='/companyEntities/edit/:id' component={ CompanyEntityFormInterface } />
        <Route exact path='/companyEntities/new' component={ CompanyEntityFormInterface } />
        <Route exact path='/clients' component={ ClientsInterface } />
      </SpaContent>
    </VerticalStackPanel>
  );
}

