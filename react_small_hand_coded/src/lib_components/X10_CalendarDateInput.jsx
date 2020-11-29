// @flow

import * as React from "react";
import CalendarDateInput from "latitude/date/CalendarDateInput";
import {
  addDaysFromCalendarDate,
  today,
  type CalendarDate,
  parseStringToCalendarDate,
  formatCalendarDate,
  shouldBeCalendarDate,
  calendarDateToMoment,
  momentToCalendarDate,
} from "latitude/date/CalendarDateType";

type Props = {|
  value: ?string,
  onChange: (date: CalendarDate | null) => void,
|};

export default function X10_CalendarDateInput(props: Props): React.Node {
  const {value, onChange} = props;

  return (
    <CalendarDateInput
      value={toCalendarDate(value || null)} // Convert null or undefined to null
      onChange={onChange}
    />
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