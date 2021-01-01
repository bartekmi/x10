// @flow

import * as React from "react";

type User = {|
  +name: string,
  +username: string,
|};

type AppContextType = {|
  +now: Date,
  +currentUser: ?User,
|};

export const AppContext: React.Context<AppContextType> = React.createContext({
  now: new Date(),
  currentUser: null,
});
export const AppContextProvider = AppContext.Provider
