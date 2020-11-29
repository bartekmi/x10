/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ConcreteRequest } from 'relay-runtime';
export type BuildingsPageQueryVariables = {||};
export type BuildingsPageQueryResponse = {|
  +buildings: ?{|
    +nodes: ?$ReadOnlyArray<?{|
      +dbid: number,
      +name: string,
      +description: string,
    |}>
  |}
|};
export type BuildingsPageQuery = {|
  variables: BuildingsPageQueryVariables,
  response: BuildingsPageQueryResponse,
|};
*/


/*
query BuildingsPageQuery {
  buildings {
    nodes {
      dbid
      name
      description
    }
  }
}
*/

const node/*: ConcreteRequest*/ = (function(){
var v0 = [
  {
    "alias": null,
    "args": null,
    "concreteType": "BuildingConnection",
    "kind": "LinkedField",
    "name": "buildings",
    "plural": false,
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Building",
        "kind": "LinkedField",
        "name": "nodes",
        "plural": true,
        "selections": [
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "dbid",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "name",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "description",
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ],
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "BuildingsPageQuery",
    "selections": (v0/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "BuildingsPageQuery",
    "selections": (v0/*: any*/)
  },
  "params": {
    "cacheID": "faeeb81f126da682b99dc16ba55469f4",
    "id": null,
    "metadata": {},
    "name": "BuildingsPageQuery",
    "operationKind": "query",
    "text": "query BuildingsPageQuery {\n  buildings {\n    nodes {\n      dbid\n      name\n      description\n    }\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = '2ff6edf1e83d5619bc37ebeca39bd8e4';

module.exports = node;
