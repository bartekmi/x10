// @flow

import * as React from "react";

type Tenant = {|
  +firstName: String,
  +lastName: String,
|};

type Props = {|
  +tenants: $ReadOnlyArray<Tenant>,
|};
export default function TenantsPage(props: Props): React.Node {
  const { tenants } = props;

  return (
    <div className="container">
      <h1>Tenants</h1>

      <table>
        <thead>
          <tr>
            <th>First Name</th>
            <th>Last Name</th>
          </tr>
        </thead>
        <tbody>
          {tenants.map(tenant =>
            <tr>
              <td>{tenant.firstName}</td>
              <td>{tenant.lastName}</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}