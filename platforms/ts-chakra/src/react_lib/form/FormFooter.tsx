import * as React from "react";
import {StyleSheet, css} from "aphrodite";

type Props = {
  readonly children: React.JSX.Element,
};
export default function FormFooter(props: Props): React.JSX.Element {
  const { children } = props

  return (
    <>
      <div className={css(styles.styling)}>
        {children}
      </div>
    </>
  );
}

const styles = StyleSheet.create({
  styling: {
    padding: 20,
  },
});
