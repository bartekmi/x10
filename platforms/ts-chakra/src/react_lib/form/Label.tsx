// Credit: inspired by https://github.com/flexport/latitude/blob/master/Label.jsx
import * as React from "react";
import {css, StyleSheet} from "aphrodite";

import HelpTooltip from "./HelpTooltip";
import colors from "../colors";

type Props = {
  readonly children: React.JSX.Element,
  readonly indicateRequired?: boolean,
  readonly weight?: "bold" | "regular",
  readonly value: string | React.JSX.Element,
  readonly helpTooltip?: string | React.JSX.Element,
};

export default function Label({
  children,
  indicateRequired = false,
  weight = "regular",
  value,
  helpTooltip,
}: Props) {
  const labelRef = React.useRef<HTMLInputElement>(null);

  const handleClick = () => {
    // Focus the first child input of the label component
    const inputRef = labelRef.current && labelRef.current.querySelector("input");

    if (inputRef) {
      inputRef.focus();
    }
  };

  const renderHelpTooltip = () => {
    if (helpTooltip == null) {
      return null;
    } else if (typeof helpTooltip === "string") {
      return (
        <HelpTooltip
          text={helpTooltip}
        />
      );
    }

    return helpTooltip;
  };

  return (
    <div ref={labelRef} className={css(styles.wrapper)}>
      <label
        className={css(styles.label)}
        onClick={handleClick}
        style={{fontWeight: weight}}
      >
        {value}
        {indicateRequired ? (
          <span className={css(styles.optionalReqdMargin)}>*</span>
        ) : null}
        <span style={{ display: "inline-block", width: "10px" }}/>
        {renderHelpTooltip()}
      </label>
      {children}
    </div>
  );
}

const styles = StyleSheet.create({
  darkType: {
    color: colors.grey60,
  },
  lightType: {
    color: colors.grey40,
  },
  label: {
    marginTop: 0,
    whiteSpace: "nowrap",
    fontSize: "13px",
    lineHeight: "18px",
    height: "20px",
    marginBottom: "1px",
  },
  paddingBottom: {
    paddingBottom: "4px",
  },
  optionalReqdMargin: {
    marginLeft: "4px",
  },
  wrapper: {
    flex: 1,
    flexBasis: "auto",
    minHeight: 0,
    minWidth: 0,
    flexDirection: "column",
    display: "flex",
  },
});
