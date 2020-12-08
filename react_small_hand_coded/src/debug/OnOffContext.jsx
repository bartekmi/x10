// @flow

import * as React from "react";

type OnOffContextType = {
  valueBefore: string,
  valueAfter: string,
};

const OnOffContext: React.Context<OnOffContextType> = React.createContext(
  // This default context kicks in if you useContext(), but there is none above.
  {
    valueBefore: "Default Before",
    valueAfter: "Default After",
  }
);

export default OnOffContext;