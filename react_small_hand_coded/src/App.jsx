// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Router } from "react-router-dom";

import './App.css';
import BuildingsPage from "./pages/BuildingsPage";
import TenantsPage from "./pages/TenantsPage";
import BuildingEditPage from "./pages/BuildingEditPage";
import history from './history';

export default function App(): React.Node {
  return (
    <Router history={history}>
      <Route exact path="/" component={BuildingsPage} />
      <Route exact path="/buildings" component={BuildingsPage} />
      <Route path="/buildings/edit/:id" component={BuildingEditPage} />
      <Route path="/buildings/new" component={BuildingEditPage} />
      <Route path="/tenants" component={TenantsPage} />
    </Router>
  );
}

