// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import Text from "latitude/Text";
import Group from "latitude/Group"

import environment from "../environment";
import type { BuildingEditPageQueryResponse } from "./__generated__/BuildingEditPageQuery.graphql";
import DummyStatefulComponent from "../debug/DummyStatefulComponent";
import DummyContextComponent from "../debug/DummyContextComponent";
import OnOffContext from "../debug/OnOffContext";

type Building = $PropertyType<BuildingEditPageQueryResponse, "building">;

type Props = {|
  +building: Building,
|};
function BuildingEditPage(props: Props): React.Node {
  const [isOn, setOn] = React.useState(false);

  const { building } = props;

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
    //<p>{building.name}</p>
    // <Group flexDirection="column">
    //   <Group>
    //     <Text>Name:</Text>
    //     <Text>{building.name}</Text>
    //   </Group>
    //   <Group>
    //     <Text>Description:</Text>
    //     <Text>{building.description}</Text>
    //   </Group>
    // </Group>
    <>
      <DummyStatefulComponent
        onText="I am ON :)"
        offText="I am OFF :("
        onChange={((value) => setOn(value))}
      />
      <OnOffContext.Provider value={isOn ? contextValues.on : contextValues.off}>
        <DummyContextComponent text="This is the main text!"/>
      </OnOffContext.Provider>
      <DummyContextComponent text="Surrounded by default values ~~~"/>
    </>
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
    }
  }
`;