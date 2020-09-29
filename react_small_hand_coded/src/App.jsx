// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Switch, HashRouter } from "react-router-dom";


import './App.css';
import BuildingsPage from "./pages/BuildingsPage";
import TenantsPage from "./pages/TenantsPage";
import BuildingEditPage from "./pages/BuildingEditPage";

export default function App(): React.Node {
  return (
    <HashRouter>
      <Switch>
        <Route exact path="/" component={BuildingsPage} />
        <Route exact path="/buildings" component={BuildingsPage} />
        <Route path="/buildings/view/:id" component={BuildingEditPage} />
        <Route path="/tenants" component={TenantsPage} />
      </Switch>
    </HashRouter>
  );
}

