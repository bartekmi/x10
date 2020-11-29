/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ConcreteRequest } from 'relay-runtime';
export type BuildingEditPageQueryVariables = {|
  id: number
|};
export type BuildingEditPageQueryResponse = {|
  +building: {|
    +name: string,
    +description: string,
  |}
|};
export type BuildingEditPageQuery = {|
  variables: BuildingEditPageQueryVariables,
  response: BuildingEditPageQueryResponse,
|};
*/


/*
query BuildingEditPageQuery(
  $id: Int!
) {
  building(id: $id) {
    name
    description
  }
}
*/

const node/*: ConcreteRequest*/ = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "id"
  }
],
v1 = [
  {
    "alias": null,
    "args": [
      {
        "kind": "Variable",
        "name": "id",
        "variableName": "id"
      }
    ],
    "concreteType": "Building",
    "kind": "LinkedField",
    "name": "building",
    "plural": false,
    "selections": [
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
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "BuildingEditPageQuery",
    "selections": (v1/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "BuildingEditPageQuery",
    "selections": (v1/*: any*/)
  },
  "params": {
    "cacheID": "7b762376ceced91802d1713d6748a525",
    "id": null,
    "metadata": {},
    "name": "BuildingEditPageQuery",
    "operationKind": "query",
    "text": "query BuildingEditPageQuery(\n  $id: Int!\n) {\n  building(id: $id) {\n    name\n    description\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = '41dce0811c9aaa130989506b85c4b987';

module.exports = node;
