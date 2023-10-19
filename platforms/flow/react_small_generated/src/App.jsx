// @flow

// Should generate this but, for now, it is hand-coded

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";
import { Route, Router } from "react-router-dom";
import { StyleSheet, css } from "aphrodite";

import ConnectedToaster from "latitude/toast/DeprecatedConnectedToaster";
import Group from "latitude/Group";
import Button from "latitude/button/Button";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";

import { AppContextProvider as SmallAppContextProvider } from "SmallAppContext";

import SmallHeader from "../x10_generated/small/ui/Header";
import DpsHeader from "../x10_generated/dps/ui/Header";
import ClientPageHeader from "../x10_generated/client_page/ui/Header";

export default function App(): React.Node {
  // const [app, setApp] = React.useState(null);
  // const app = "client_page";
  // const app = "small";
  const app = "small";

  const appContext = {
    today: new Date().toISOString(),
  };

  // There is an issue where, apparently, we cannot use state ABOVE the router.
  // As soon as the user navigates - e.g. to edit a building, etc - we lose the state and this
  // "uber-app selector" goes back to "choose the app" mode.

  // if (app == null) {
  //   return (
  //     <Group>
  //       <Button onClick={() => setApp("small")}>Small App</Button>
  //       <Button onClick={() => setApp("client_page")}>Client Page App</Button>
  //     </Group>
  //   );
  // }

  if (app == "small") {
    return (
      <>
        <ConnectedToaster />
        <SmallAppContextProvider value={appContext}>
          <div className={css(styles.app)}>
            <SmallHeader />
          </div>
        </SmallAppContextProvider>
      </>
    );
  }

  else if (app == "dps") {
    return (
      <>
        <ConnectedToaster />
        <div className={css(styles.app)}>
          <DpsHeader />
        </div>
      </>
    );
  }

  else if (app == "client_page") {
    return (
      <>
        <ConnectedToaster />
        <div className={css(styles.app)}>
          <ClientPageHeader />
        </div>
      </>
    );
  }

  throw "Unknown app!!!";
}


const styles = StyleSheet.create({
  app: {
    padding: whitespaceSizeConstants.m,
  },
});
