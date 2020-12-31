// @flow

import { createBrowserHistory } from 'history/cjs/history';

type HistoryType = {
  +push: (url: string) => void;
};

const history = (createBrowserHistory(): HistoryType);

export default history;