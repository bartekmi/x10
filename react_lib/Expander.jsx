// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import colors from "latitude/colors";
import Group from "latitude/Group";
import IconButton from "latitude/button/IconButton";
import { whitespaceSizeConstants } from "latitude/styles/whitespace";

type Props = {|
    +headerFunc: () => React.Node,
    +children: React.Node, // The body when expanded
    +defaultState?: "open" | "closed"
|};
export default function Expander(props: Props): React.Node {
  const {headerFunc, children, defaultState = "open"} = props;
  const [expanded, setExpanded] = React.useState(defaultState == "open");

  return (
    <Group flexDirection="column">
      <Group alignItems="center" justifyContent="space-between">
        {headerFunc()}
        <IconButton 
          kind="bare" 
          iconName={expanded ? "downOpen" : "rightOpen"} 
          type="button" 
          onClick={() => setExpanded(!expanded)} />
      </Group>
      {expanded ? children : null}
    </Group>
  );
}

const styles = StyleSheet.create({
  border: {
    border: `1px solid ${colors.grey20}`,
    padding: whitespaceSizeConstants.xs,
    margins: "0px",
  },
});