// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import LatitudeTimeInput, {getTimeIntervals} from "latitude/date/TimeInput";
import {type WallTime, ZERO_OCLOCK, EOD_OCLOCK} from "latitude/date/wallTime";

type Props = {|
  +value: ?string,
  +onChange: (time: WallTime | null) => void,
  +readOnly?: boolean,
|};
export default function TimeInput(props: Props): React.Node {
  const {value, onChange, readOnly} = props;

  return (
    <div className={css(styles.styling)}>
      <LatitudeTimeInput
        options={getTimeIntervals(ZERO_OCLOCK, EOD_OCLOCK, 30)}
        value={toWallTime(value || null)} // Convert null or undefined to null
        onChange={onChange}
        disabled={readOnly}
        
      />
    </div>
  );
}

function toWallTime(time: string | null): WallTime | null {
  // $FlowIgnoreError
  return time;
}

const styles = StyleSheet.create({
  styling: {
    maxWidth: 150,
  },
});