// @flow

import * as React from "react";
import { Link } from 'react-router-dom';
import { graphql, QueryRenderer } from "react-relay";

import Button from "latitude/button/Button";
import Table from "latitude/table/Table";
import TextCell from "latitude/table/TextCell";
import LinkCell from "latitude/table/LinkCell";
import Text from "latitude/Text";

import environment from "../environment";
import history from '../history';

type Tenant = {
  +dbid: number,
  +name: string,
  +phone: string,
  +email: string,
};

type Props = {|
  +tenants: $ReadOnlyArray<Tenant>,
|};
function TenantsPage(props: Props) {
  const [sortBy, setSortBy] = React.useState({
    columnId: "name",
    direction: "asc",
  });

  const { tenants } = props;
  const columnDefinitions = [
    {
      id: "name",
      header: "Name",
      render: (Tenant: Tenant) => <LinkCell
        value={Tenant.name}
        href={`/Tenants/edit/${Tenant.dbid}`}
      />,
      width: 150,
      comparator: (a, b) => a.name.localeCompare(b.name),
    },
    {
      id: "phone",
      header: "Phone",
      render: (Tenant: Tenant) => <TextCell value={Tenant.phone} />,
      width: 110,
    },
    {
      id: "email",
      header: "Email",
      render: (Tenant: Tenant) => <TextCell value={Tenant.email} />,
      width: 200,
    },
  ];

  return (
    <div className="container">
      <Text scale="headline">Tenants</Text>

      <div
        style={{
          height: "500px",
          width: "90%",
        }}
      >
        <Table
          data={tenants}
          columnDefinitions={columnDefinitions}
          getUniqueRowId={data => data.dbid.toString()}
          sortBy={sortBy}
          onSortByChange={setSortBy}
        />
      </div>

      <Button
        intent="basic" kind="solid"
        onClick={() => history.push("/Tenants/new")}
      >
        New Tenant
      </Button>
    </div>
  );
}

export default function TenantsPageWrapper(): React.Node {
  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{}}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          return (
            <TenantsPage
              tenants={props.tenants.nodes}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

const query = graphql`
  query TenantsPageQuery {
    tenants {
      nodes {
        dbid
        name
        phone
        email
      }
    }
  }
`;