// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Router } from "react-router-dom";

import Link from "latitude/Link";
import Group from "latitude/Group";
import Text from "latitude/Text";

import './App.css';
import BuildingsPage from "./pages/BuildingsPage";
import TenantsPage from "./pages/TenantsPage";
import BuildingEditPage from "./pages/BuildingEditPage";
import history from './history';

export default function App(): React.Node {
  return (
    <Router history={history}>

      <Group>
        <Link href="/buildings">Buildings</Link>
        <Link href="/buildings/new">New Building</Link>
        <Link href="/tenants">Tenants</Link>
      </Group>

      <Route exact path="/" component={BuildingsPage} />
      <Route exact path="/buildings" component={BuildingsPage} />
      <Route path="/buildings/edit/:id" component={BuildingEditPage} />
      <Route exact path="/buildings/new" component={BuildingEditPage} />
      <Route exact path="/tenants" component={TenantsPage} />
    </Router>


  );
}

