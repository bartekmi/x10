// @flow

import * as React from "react";
import type { __Context__ } from "entities/__Context__";

type User = {|
  +name: string,
  +username: string,
|};

export const AppContext: React.Context<__Context__> = React.createContext({
  today: new Date(),
});
export const AppContextProvider = AppContext.Provider
