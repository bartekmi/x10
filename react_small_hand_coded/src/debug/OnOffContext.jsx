// @flow

import * as React from "react";

type OnOffContextType = {
  valueBefore: string,
  valueAfter: string,
};

const OnOffContext: React.Context<OnOffContextType> = React.createContext(
  {
    valueBefore: "Default Before",
    valueAfter: "Default After",
  }
);

export default OnOffContext;