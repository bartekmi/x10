import React from "react";
import { useTable } from "react-table";

import TabHeader from "latitude/tabs/TabHeader";
import Group from "latitude/Group";

type Tab = {|
  +id: string,      // Used for switching tabs
  +label: string,   // Label to show for the tab
    // +displayFunc: (data: T, onChange: (data: T) => void) => React.Node, // Content/body of tab
    +displayFunc: () => React.Node,
|};

type Props<T> = {|
  +initialTab ?: string,
  +tabs: ReadOnlyArray < Tab < T >>,
  // +data: T,
  // +onChange?: (newData: T) => void,   // Not present for read-only display
|};
export default function TabbedPane(props: Props) {
  const { initialTab, tabs /*, data, onChange */ } = props;
  const [currentTabId, setCurrentTabId] = React.useState(initialTab || tabs[0].id);

  let tab = tabs.find(x => x.id == currentTabId);
  if (tab == null) {    // Could be true if bogus initialTab was passed
    tab = tabs[0];
  }

  return (
    <Group flexDirection="column">
      <TabHeader
        tabs={tabs.map(x => {
          return {
            id: x.id,
            name: x.label,
          }
        })}
        activeTab={currentTabId}
        onTabChange={setCurrentTabId}
      />
      {tab.displayFunc()}
    </Group>
  )
}