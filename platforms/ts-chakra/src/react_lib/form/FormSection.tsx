import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import { Flex } from '@chakra-ui/react'

type Props = {
  readonly children: JSX.Element | JSX.Element[],
  readonly label?: string,
};
export default function FormSection(props: Props): React.JSX.Element {
  const { children, label } = props

  return (
    <>
      <div className={css(styles.heading)}>
        <h2>{label}</h2>
      </div>
      <Flex flexDirection="column" gap={20}>
        {children}
      </Flex>
    </>
  );
}

const styles = StyleSheet.create({
  heading: {
    paddingTop: 12,
    paddingBottom: 12,
  },
});
