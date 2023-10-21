import { ChakraProvider, theme, Flex } from "@chakra-ui/react"

import SmallHeader from "./x10_generated/small/ui/Header";
import { AppContextProvider as SmallAppContextProvider } from "./SmallAppContext";

export default function App() {
  const appContext = {
    today: new Date().toISOString(),
  };

  return (
    <ChakraProvider theme={theme}>
      <SmallAppContextProvider value={appContext}>
        <Flex padding={8}>
          <SmallHeader />
        </Flex>
      </SmallAppContextProvider>
    </ChakraProvider>
  )
}
