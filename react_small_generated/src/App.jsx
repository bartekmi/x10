// @flow

// Should generate this but, for now, it is hand-coded

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Router } from "react-router-dom";
import {StyleSheet, css} from "aphrodite";

import Link from "latitude/Link";
import Group from "latitude/Group";
import Text from "latitude/Text";
import colors from "latitude/colors";
import {whitespaceSizeConstants} from "latitude/styles/whitespace";

import { AppContextProvider } from "AppContext";

import Header from "../__generated__/ui/Header";

export default function App(): React.Node {

  const appContext = {
    today: new Date().toISOString(),
    currentUser: {
      name: "Bartek Muszynski",
      username: "bartekmi",
    },
  };

  return (
    <AppContextProvider value={appContext}>
      <Header/>
    </AppContextProvider>
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
