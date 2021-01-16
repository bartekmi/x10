// @flow

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import CalendarDateInput from "latitude/date/CalendarDateInput";
import {type CalendarDate} from "latitude/date/CalendarDateType";

type Props = {|
  value: ?Date,
  onChange: (date: Date | null) => void,
|};

export default function X10_CalendarDateInput(props: Props): React.Node {
  const {value, onChange} = props;

  return (
    <div className={css(styles.styling)}>
      <CalendarDateInput
        value={dateToCalendarDate(value)} 
        onChange={value => onChange(calendarDateToDate(value))}
      />
    </div>
  );
}

function dateToCalendarDate(date: ?Date): CalendarDate | null {
  if (date == null) {
    return null;
  }

  date.setUTCHours(0, 0, 0, 0);
  return date.toISOString();
}

function calendarDateToDate(date: ?CalendarDate): Date | null {
  if (date == null) {
    return null;
  }

  return new Date(date);
}

const styles = StyleSheet.create({
  styling: {
    maxWidth: 150,
  },
});
