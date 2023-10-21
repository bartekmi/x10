import * as React from "react";
import {StyleSheet, css} from "aphrodite";

type CalendarDate = string;

type Props = {
  readonly value: CalendarDate | null | undefined,
  readonly onChange: (date: CalendarDate | null | undefined) => void,
  readonly readOnly?: boolean,
};

export default function CalendarDateInput(props: Props): React.JSX.Element {
  const {value, onChange, readOnly} = props;

  return (
    <div className={css(styles.styling)}>
      TODO
      {/* <LatitudeCalendarDateInput
        value={toCalendarDate(value || null)} // Convert null or undefined to null
        onChange={onChange}
        disabled={readOnly}
      /> */}
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