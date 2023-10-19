import * as React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import VerticalStackPanel from '../../../react_lib/layout/VerticalStackPanel';
import Menu from '../../../react_lib/menu/Menu';
import MenuItem from '../../../react_lib/menu/MenuItem';

import BuildingFormInterface from './BuildingFormInterface';



type Props = {
};
export default function Header(props: Props): React.JSX.Element {
  return (
    <VerticalStackPanel>
      <Menu>
        <MenuItem
          label='New Building'
          href='/buildings/new'
        />
      </Menu>
      <Router>
        <Routes>
          <Route path='/' element={ <BuildingFormInterface/> } />
          <Route path='/buildings/new' element={ <BuildingFormInterface/> } />
        </Routes>
      </Router>
    </VerticalStackPanel>
  );
}

