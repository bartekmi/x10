// @flow

import * as React from "react";
import { Link } from 'react-router-dom';
import { graphql, QueryRenderer } from "react-relay";

import Button from "latitude/button/Button";
import Table from "latitude/table/Table";
import TextCell from "latitude/table/TextCell";
import LinkCell from "latitude/table/LinkCell";
import Text from "latitude/Text";
import Group from "latitude/Group";
import HelpTooltip from "latitude/HelpTooltip";
import TextLink from "latitude/TextLink";

import enumToLabel from "react_lib/utils/enumToLabel";
import CellRenderer from "react_lib/table/CellRenderer";

import history from "../history";
import { type PetPolicyEnum, PetPolicyEnumPairs } from "../constants/PetPolicyEnum";

type Building = {
  +dbid: number,
  +name: string,
  +description: string,
  +petPolicy: PetPolicyEnum,
  +physicalAddress: {
    +city: string,
  }
};

type Props = {|
  +buildings: $ReadOnlyArray < Building >,
|};
export default function BuildingsPage(props: Props): React.Node {
  const [sortBy, setSortBy] = React.useState({
    columnId: "name_and_description",
    direction: "asc",
  });

  const { buildings } = props;
  const columnDefinitions = [
    {
      id: "name_and_description",
      header: "Name/Desc",
      render: (building: Building) => 
        <CellRenderer>
          <span>
            <Text children={building.name}/>
            <HelpTooltip text={building.description}/>
          </span>
        </CellRenderer>,
      width: 150,
      comparator: (a, b) => a.name.localeCompare(b.name),
    },
    {
      id: "petPolicy",
      header: "Pet Policy",
      render: (building: Building) => <TextCell 
        value={enumToLabel(PetPolicyEnumPairs, building.petPolicy)} 
      />,
      width: 70,
    },
    {
      id: "physicalAddress_city",
      header: "City",
      render: (building: Building) => <TextCell value={building.physicalAddress.city} />,
      width: 70,
    },
    {
      id: "actions",
      header: "Actions!",
      render: (building: Building) => 
        <CellRenderer>
          <Group>
            <Button onClick={() => console.log(building)}>Edit</Button>
            <TextLink href={`/buildings/edit/${building.dbid}`}>Edit</TextLink>
          </Group>
        </CellRenderer>,
      width: 150,
      comparator: (a, b) => a.name.localeCompare(b.name),
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
