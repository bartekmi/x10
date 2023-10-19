import * as React from "react";
// import type { __Context__ } from "./x10_hand_coded/small/entities/__Context__";

export type AppContextType = {
  today?: string
}

const initial: AppContextType = {}

export const AppContext = React.createContext(initial);
export const AppContextProvider = AppContext.Provider
