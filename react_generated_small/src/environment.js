// @flow
// Source: https://relay.dev/docs/en/relay-environment

import {
  Environment,
  Network,
  RecordSource,
  Store,
  type IEnvironment,
} from 'relay-runtime';

const source = new RecordSource();
const store = new Store(source);
const network = Network.create(fetchQuery);
const handlerProvider = null; // Will use default

const environment: IEnvironment = new Environment({
  handlerProvider, 
  network,
  store,
});

// Define a function that fetches the results of an operation (query/mutation/etc)
// and returns its results as a Promise:
function fetchQuery(
  operation,
  variables,
  cacheConfig,
  uploadables,
) {
  return fetch('http://localhost:5000/graphql', {
    method: 'POST',
    mode: 'cors',
    headers: {
      // Add authentication and other headers here
      'content-type': 'application/json'
    },
    body: JSON.stringify({
      query: operation.text, // GraphQL text from input
      variables,
    }),
  }).then(response => {
    return response.json();
  });
}

export default environment;