// @flow

import * as React from "react";
import type { __Context__ } from "entities/__Context__";

export const AppContext: React.Context<__Context__> = React.createContext({
  today: null,
});
export const AppContextProvider = AppContext.Provider
