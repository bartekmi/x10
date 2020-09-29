// @flow

import * as React from "react";
import { graphql, QueryRenderer } from "react-relay";

import environment from "../environment";
import type {BuildingEditPageQueryResponse} from "./__generated__/BuildingEditPageQuery.graphql";

type Building = $PropertyType<BuildingEditPageQueryResponse, "building">;

type Props = {|
  +building: Building,
|};
function BuildingEditPage(props: Props): React.Node {
  const {building} = props;

  return (
    <>
    <div>
      <span>Name:</span>
      <span>{building.name}</span>
    </div>
    <div>
      <span>Description:</span>
      <span>{building.description}</span>
    </div>
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