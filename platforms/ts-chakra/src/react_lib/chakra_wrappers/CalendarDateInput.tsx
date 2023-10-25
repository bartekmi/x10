import * as React from "react";
import {StyleSheet, css} from "aphrodite";
import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";

// CSS Modules, react-datepicker-cssmodules.css
// import 'react-datepicker/dist/react-datepicker-cssmodules.css';

type CalendarDate = string;

type Props = {
  readonly value: CalendarDate | null | undefined,
  readonly onChange: (date?: CalendarDate) => void,
  readonly readOnly?: boolean,
};

export default function CalendarDateInput(props: Props): React.JSX.Element {
  const {value, onChange, readOnly} = props;
  const dateObject = value == null ? null : new Date(value);

  return (
    <div className={css(styles.styling)}>
      <DatePicker 
        selected={dateObject} 
        onChange={(date) => onChange(date?.toISOString())} 
      />
    </div>
  );
}

const styles = StyleSheet.create({
  styling: {
    maxWidth: 150,
  },
});