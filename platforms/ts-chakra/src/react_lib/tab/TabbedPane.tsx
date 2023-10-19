import React from "react";
import { Tabs, TabList, TabPanels, Tab as ChakraTab, TabPanel } from '@chakra-ui/react'

type Tab = {
  readonly id: string,      // Used for switching tabs
  readonly label: string,   // Label to show for the tab
    // readonly displayFunc: (data: T, onChange: (data: T) => void) => React.JSX.Element, // Content/body of tab
    readonly displayFunc: () => React.JSX.Element,
};

type Props = {
  readonly initialTab?: string,
  readonly tabs: Tab[],
  // readonly data: T,
  // readonly onChange?: (newData: T) => void,   // Not present for read-only display
};
export default function TabbedPane(props: Props) {
  const { initialTab, tabs /*, data, onChange */ } = props;

  let index = 0;
  if (initialTab) {
    for (let ii = 0; ii < tabs.length; ii++) {
      if (tabs[ii].id == initialTab) {
        index = ii;
        break;
      }
    }
  }

  return (
    <Tabs defaultIndex={index}>
      <TabList>
        {tabs.map(tab => <ChakraTab>{tab.label}</ChakraTab>)}
      </TabList>

      <TabPanels>
        {tabs.map(tab => <TabPanel>{tab.displayFunc()}</TabPanel>)}
      </TabPanels>
    </Tabs>
  );
}
