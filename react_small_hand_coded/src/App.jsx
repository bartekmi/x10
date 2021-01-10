// @flow

import * as React from "react";
import { Route, Router } from "react-router-dom";
import { StyleSheet, css } from "aphrodite";

import Group from "latitude/Group";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";

import 'App.css';
import { AppContextProvider } from "AppContext";

import Menu from "react_lib/menu/Menu";
import MenuItem from "react_lib/menu/MenuItem";

import BuildingsPageInterface from "./pages/BuildingsPageInterface";
import TenantsPage from "./pages/TenantsPage";
import BuildingEditPageInterface from "./pages/BuildingEditPageInterface";
import TenantEditPage from "./pages/TenantEditPage";
import history from './history';


export default function App(): React.Node {

  const appContext = {
    now: new Date(),
    currentUser: {
      name: "Bartek Muszynski",
      username: "bartekmi",
    },
  };

  return (
    <AppContextProvider value={appContext}>
      <div className={css(styles.app)}>
        <Group gap={12}>
          <Menu>
            <MenuItem href="/buildings" label="Buildings" />
            <MenuItem href="/buildings/new" label="New Building" />
            <MenuItem href="/tenants" label="Tenants" />
            <MenuItem href="/tenants/new" label="New Tenant" />
          </Menu>

          <Router history={history}>
            <Route exact path="/" component={BuildingsPageInterface} />

            <Route exact path="/buildings" component={BuildingsPageInterface} />
            <Route path="/buildings/edit/:id" component={BuildingEditPageInterface} />
            <Route exact path="/buildings/new" component={BuildingEditPageInterface} />

            <Route exact path="/tenants" component={TenantsPage} />
            <Route path="/tenants/edit/:id" component={TenantEditPage} />
            <Route exact path="/tenants/new" component={TenantEditPage} />

          </Router>
        </Group>
      </div>
    </AppContextProvider>
  );
}


const styles = StyleSheet.create({
  app: {
    padding: whitespaceSizeConstants.m,
  },
});
