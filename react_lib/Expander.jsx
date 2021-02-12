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
|};
export default function Expander(props: Props): React.Node {
  const {headerFunc, children} = props;
  const [expanded, setExpanded] = React.useState(false);

  return (
    <div className={css(styles.border)}>
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
    </div>
  );
}

const styles = StyleSheet.create({
  border: {
    border: `1px solid ${colors.grey20}`,
    padding: whitespaceSizeConstants.xs,
    margins: "0px",
  },
});