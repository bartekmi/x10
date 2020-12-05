// @flow

import { createBrowserHistory } from 'history';

type HistoryType = {
  +push: (url: string) => void;
};

const history = createBrowserHistory();

export default history;