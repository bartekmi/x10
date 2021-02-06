// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import colors from "latitude/colors";
import Group from "latitude/Group";
import IconButton from "latitude/button/IconButton";

type Props = {|
    +headerFunc: () => React.Node,
    +children: React.Node, // The body when expanded
|};
export default function Expander(props: Props): React.Node {
  const {headerFunc, children} = props;
  const [expanded, setExpanded] = React.useState(false);

  return (
    <Group flexDirection="column">
      <Group>
        <IconButton 
          kind="bare" 
          iconName={expanded ? "downOpen" : "rightOpen"} 
          type="button" 
          onClick={() => setExpanded(!expanded)} />
        {headerFunc()}
      </Group>
      {expanded ? children : null}
    </Group>
  );
}

const styles = StyleSheet.create({
  main: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey30}`,
    margin: "1em 0",
    padding: "0",
  },
});
