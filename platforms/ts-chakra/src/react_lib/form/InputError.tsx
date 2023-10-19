// Credit: https://github.com/flexport/latitude/blob/master/InputError.jsx
import * as React from "react";

import {StyleSheet, css} from "aphrodite";
import colors from "../colors";
import isBlank from "../utils/isBlank"

type Props = {
  readonly errorText: string | React.JSX.Element | null,
  readonly showError?: boolean,
  readonly children?: React.JSX.Element,
};

export default function InputError({
  errorText,
  children,
}: Props) {
  return (
    <div>
      {children}
      {!isBlank(errorText) ? (
        <div className={css(inputErrorStyles.style)}>
          {errorText}
        </div>
      ) : null}
    </div>
  );
}

export const inputErrorStyles = StyleSheet.create({
  style: {
    fontWeight: 500,
    color: colors.red40,
  },
});
