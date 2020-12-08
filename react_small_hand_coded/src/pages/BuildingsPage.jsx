// @flow

import * as React from "react";
import { Link } from 'react-router-dom';
import { graphql, QueryRenderer } from "react-relay";

import Button from "latitude/button/Button";
import Table from "latitude/table/Table";
import TextCell from "latitude/table/TextCell";
import LinkCell from "latitude/table/LinkCell";
import Text from "latitude/Text";

import enumToLabel from "../lib_components/utils/enumToLabel";
import environment from "../environment";
import history from "../history";
import PetPolicyEnum from "../constants/PetPolicyEnum";

import {type PetPolicyEnum as PetPolicEnumGql} from "./__generated__/BuildingsPageQuery.graphql"

type Building = {
  +dbid: number,
  +name: string,
  +description: string,
  +petPolicy: PetPolicEnumGql,
  +physicalAddress: {
    +city: string,
  }
};

type Props = {|
  +buildings: $ReadOnlyArray < Building >,
|};
function BuildingsPage(props: Props) {
  const [sortBy, setSortBy] = React.useState({
    columnId: "name",
    direction: "asc",
  });

  const { buildings } = props;
  const columnDefinitions = [
    {
      id: "name",
      header: "Name",
      render: (building: Building) => <LinkCell
        value={building.name}
        href={`/buildings/edit/${building.dbid}`}
      />,
      width: 150,
      comparator: (a, b) => a.name.localeCompare(b.name),
    },
    {
      id: "petPolicy",
      header: "Pet Policy",
      render: (building: Building) => <TextCell 
        value={enumToLabel(PetPolicyEnum, building.petPolicy)} 
      />,
      width: 150,
    },
    {
      id: "physicalAddress_city",
      header: "City",
      render: (building: Building) => <TextCell value={building.physicalAddress.city} />,
      width: 100,
    },
    {
      id: "description",
      header: "Description",
      render: (building: Building) => <TextCell value={building.description} />,
      width: 300,
    },
  ];

  return (
    <div className="container">
      <Text scale="headline">Buildings</Text>

      <div
        style={{
          height: "500px",
          width: "100%",
        }}
      >
        <Table
          data={buildings}
          columnDefinitions={columnDefinitions}
          getUniqueRowId={data => data.dbid.toString()}
          sortBy={sortBy}
          onSortByChange={setSortBy}
        />
      </div>

      <Button
        intent="basic" kind="solid"
        onClick={() => history.push("/buildings/new")}
      >
        New Building
      </Button>
    </div>
  );
}

export default function BuildingsPageWrapper(): React.Node {
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
            <BuildingsPage
              buildings={props.buildings.nodes}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

const query = graphql`
  query BuildingsPageQuery {
    buildings {
      nodes {
        dbid
        name
        description
        petPolicy
        physicalAddress {
          city
        }
      }
    }
  }
`;