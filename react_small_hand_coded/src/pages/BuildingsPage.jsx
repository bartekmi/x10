// @flow

import * as React from "react";
import { Link } from 'react-router-dom';
import { graphql, QueryRenderer } from "react-relay";

import environment from "../environment";

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
      <h1>Buildings</h1>

      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {buildings.map(building =>
            <tr>
              <td>
                <Link to={`/buildings/view/${building.dbid}`}>{building.name}</Link>
              </td>
              <td>{building.description}</td>
            </tr>
          )}
        </tbody>
      </table>
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