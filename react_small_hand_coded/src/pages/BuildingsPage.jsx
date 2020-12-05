// @flow

import * as React from "react";
import { Link } from 'react-router-dom';
import { graphql, QueryRenderer } from "react-relay";

import Button from "latitude/button/Button";
import Text from "latitude/Text";

import environment from "../environment";
import history from '../history';

type Building = {
  +dbid: number,
  +name: string,
    +description: string,
};

type Props = {|
  +buildings: $ReadOnlyArray < Building >,
|};
function BuildingsPage(props: Props) {
  const { buildings } = props;

  return (
    <div className="container">
      <Text scale="headline">Buildings</Text>

      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {buildings.map(building =>
            <tr key={building.dbid}>
              <td>
                <Link to={`/buildings/edit/${building.dbid}`}>{building.name}</Link>
              </td>
              <td>{building.description}</td>
            </tr>
          )}
        </tbody>
      </table>

      <Button
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
      }
    }
  }
`;