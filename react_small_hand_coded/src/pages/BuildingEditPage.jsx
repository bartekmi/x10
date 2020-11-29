// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import Group from "latitude/Group"
import Text from "latitude/Text";
import TextInput from "latitude/TextInput";
import TextareaInput from "latitude/TextareaInput";
import Label from "latitude/Label";
import Button from "latitude/button/Button";

import X10_CalendarDateInput from "../lib_components/X10_CalendarDateInput";
import environment from "../environment";

import type { BuildingEditPageQueryResponse } from "./__generated__/BuildingEditPageQuery.graphql";

type Building = $PropertyType<BuildingEditPageQueryResponse, "building">;

type Props = {|
  +building: Building,
|};
function BuildingEditPage(props: Props): React.Node {
  const { building } = props;
  const [editedBuilding, setEditedBuilding] = React.useState(building);

  const save = () => {
    console.log("Clicked Save!")
  };

  const contextValues = {
    on: {
      valueBefore: "ON - BEFORE",
      valueAfter: "ON - AFTER",
    },
    off: {
      valueBefore: "OFF - BEFORE",
      valueAfter: "OFF - AFTER",
    },
  };

  return (
    <Group flexDirection="column">
      <Text scale="headline">{`Editing Building ${building.name || ""}`}</Text>
      <Label value="Name:" >
        <TextInput
          value={editedBuilding.name}
          onChange={(value) => {
            setEditedBuilding({...editedBuilding, name: value})
          }}
        />
      </Label>
      <Label value="Description:" >
        <TextareaInput
          value={editedBuilding.description}
          onChange={(value) => {
            setEditedBuilding({...editedBuilding, description: value})
          }}
        />
      </Label>
      <Label value="Date of Occupancy:" >
        <X10_CalendarDateInput
          value={editedBuilding.dateOfOccupancy}
          onChange={(value) => {
            setEditedBuilding({...editedBuilding, dateOfOccupancy: value})
          }}
        />
      </Label>
      <Button onClick={save}>Save</Button>
    </Group>
  );
}

type WrapperProps = {
  +match: {
    +params: {
      +id: string
    }
  }
};
export default function BuildingEditPageWrapper(props: WrapperProps): React.Node {
  const stringId = props.match.params.id;
  const id: number = parseInt(stringId);
  if (isNaN(id)) {
    throw new Error("Not a number: " + stringId);
  }

  return (
    <QueryRenderer
      environment={environment}
      query={query}
      variables={{
        id
      }}
      render={({ error, props }) => {
        if (error) {
          return <div>{error.message}</div>;
        } else if (props) {
          return (
            <BuildingEditPage
              building={props.building}
            />
          );
        }
        return <div>Loading</div>;
      }}
    />
  );
}

const query = graphql`
  query BuildingEditPageQuery($id: Int!) {
    building(id: $id) {
      name
      description
      dateOfOccupancy
    }
  }
`;