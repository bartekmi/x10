// @flow

import * as React from "react";
import type { __Context__ } from "small/entities/__Context__";

export const SmallAppContext: React.Context<__Context__> = React.createContext({
  today: null,
});
export const SmallAppContextProvider = SmallAppContext.Provider
