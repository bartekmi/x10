// @flow

import * as React from "react";

type User = {|
  +name: string,
  +username: string,
|};

type AppContextType = {|
  +today: string,
  +currentUser: ?User,
|};

export const AppContext: React.Context<AppContextType> = React.createContext({
  today: new Date().toISOString(),
  currentUser: null,
});
export const AppContextProvider = AppContext.Provider
