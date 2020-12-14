// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Router } from "react-router-dom";
import {StyleSheet, css} from "aphrodite";

import Link from "latitude/Link";
import Group from "latitude/Group";
import Text from "latitude/Text";
import colors from "latitude/colors";
import {whitespaceSizeConstants} from "latitude/styles/whitespace";

import './App.css';
import BuildingsPage from "./pages/BuildingsPage";
import TenantsPage from "./pages/TenantsPage";
import BuildingEditPage from "./pages/BuildingEditPage";
import TenantEditPage from "./pages/TenantEditPage";
import history from './history';

export default function App(): React.Node {
  return (
    <div className={css(styles.app)}>
      <Router history={history}>

        <Group>
          <div className={css(styles.menuItem)}>
            <Link href="/buildings">Buildings</Link>
          </div>
          <div className={css(styles.menuItem)}>
            <Link href="/buildings/new">New Building</Link>
          </div>
          <div className={css(styles.menuItem)}>
            <Link href="/tenants">Tenants</Link>
          </div>
          <div className={css(styles.menuItem)}>
            <Link href="/tenants/new">New Tenant</Link>
          </div>
        </Group>

        <Route exact path="/" component={BuildingsPage} />

        <Route exact path="/buildings" component={BuildingsPage} />
        <Route path="/buildings/edit/:id" component={BuildingEditPage} />
        <Route exact path="/buildings/new" component={BuildingEditPage} />

        <Route exact path="/tenants" component={TenantsPage} />
        <Route path="/tenants/edit/:id" component={TenantEditPage} />
        <Route exact path="/tenants/new" component={TenantEditPage} />
        
      </Router>
    </div>
  );
}


const styles = StyleSheet.create({
  app: {
    padding: whitespaceSizeConstants.m,
  },
  menuItem: {
    background: colors.grey20,
    padding: whitespaceSizeConstants.m,
    margins: "0px",
  },
});
