// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import LatitudeCalendarDateInput from "latitude/date/CalendarDateInput";
import {type CalendarDate} from "latitude/date/CalendarDateType";

type Props = {|
  +value: ?string,
  +onChange: (date: CalendarDate | null) => void,
  +readOnly?: boolean,
|};

export default function CalendarDateInput(props: Props): React.Node {
  const {value, onChange, readOnly} = props;

  return (
    <div className={css(styles.styling)}>
      <LatitudeCalendarDateInput
        value={toCalendarDate(value || null)} // Convert null or undefined to null
        onChange={onChange}
        disabled={readOnly}
      />
    </div>
  );
}

function toCalendarDate(dateAndMaybeTime: string | null): CalendarDate | null {
  if (dateAndMaybeTime == null) {
    return null;
  }

  const index = dateAndMaybeTime.indexOf("T");
  if (index == -1) {
    throw "Invalid date/time format: " + dateAndMaybeTime;
  }
 
  const date = dateAndMaybeTime.substr(0, index);
  return `${date}T00:00:00Z`;
}

const styles = StyleSheet.create({
  styling: {
    maxWidth: 150,
  },
});