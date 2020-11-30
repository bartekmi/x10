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
    +dbid: number,
    +name: string,
    +description: string,
    +dateOfOccupancy: ?string,
    +mailingAddressSameAsPhysical: boolean,
    +physicalAddress: ?{|
      +city: ?string,
      +dbid: number,
      +stateOrProvince: ?string,
      +theAddress: ?string,
      +unitNumber: ?string,
      +zip: ?string,
    |},
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
    dbid
    name
    description
    dateOfOccupancy
    mailingAddressSameAsPhysical
    physicalAddress {
      city
      dbid
      stateOrProvince
      theAddress
      unitNumber
      zip
    }
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
v1 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "dbid",
  "storageKey": null
},
v2 = [
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
      (v1/*: any*/),
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
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "dateOfOccupancy",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "mailingAddressSameAsPhysical",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "concreteType": "Address",
        "kind": "LinkedField",
        "name": "physicalAddress",
        "plural": false,
        "selections": [
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "city",
            "storageKey": null
          },
          (v1/*: any*/),
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "stateOrProvince",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "theAddress",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "unitNumber",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "zip",
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
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "BuildingEditPageQuery",
    "selections": (v2/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "BuildingEditPageQuery",
    "selections": (v2/*: any*/)
  },
  "params": {
    "cacheID": "435f1d207f2beb9a44498fc75abfcac9",
    "id": null,
    "metadata": {},
    "name": "BuildingEditPageQuery",
    "operationKind": "query",
    "text": "query BuildingEditPageQuery(\n  $id: Int!\n) {\n  building(id: $id) {\n    dbid\n    name\n    description\n    dateOfOccupancy\n    mailingAddressSameAsPhysical\n    physicalAddress {\n      city\n      dbid\n      stateOrProvince\n      theAddress\n      unitNumber\n      zip\n    }\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = 'ed21e7fac6272024e24f1d1943fccb1e';

module.exports = node;
