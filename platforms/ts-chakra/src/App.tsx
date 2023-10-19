import { StyleSheet, css } from "aphrodite";
import { ChakraProvider, theme, Flex } from "@chakra-ui/react"

import SmallHeader from "./x10_hand_coded/small/ui/Header";
import { AppContextProvider as SmallAppContextProvider } from "./SmallAppContext";

export default function App() {
  const appContext = {
    today: new Date().toISOString(),
  };

  return (
    // <div className={css(styles.app)}>
    <ChakraProvider theme={theme}>
      <SmallAppContextProvider value={appContext}>
        <Flex padding={8}>
          <SmallHeader />
        </Flex>
      </SmallAppContextProvider>
    </ChakraProvider>
  )
    // </div>);
}

const styles = StyleSheet.create({
  app: {
    padding: 12,
  },
});
