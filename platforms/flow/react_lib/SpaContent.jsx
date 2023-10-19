// @flow

import { createBrowserHistory } from 'history/cjs/history';
import * as React from "react";
import { Route, Router } from "react-router-dom";

type HistoryType = {
  +push: (url: string) => void;
};
const history = (createBrowserHistory(): HistoryType);

type Props = {|
  +rootComponent: (props: {...}) => React.Node,
  +children: React.Node
|};
export default function SpaContent(props: Props): React.Node {
  const {rootComponent, children} = props;

  return <Router history={history}>
    <Route exact path='/' component={rootComponent}/>
    {children}
  </Router>
}

