/**
 * @flow 
 */

import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import Group from "latitude/Group";
import Button from "latitude/button/Button";
import IconButton from "latitude/button/IconButton";
import colors from "latitude/colors";

type TItem = {
  +id: string,
};

type Props<T: TItem> = {|
  +items: $ReadOnlyArray < T >,
  +itemDisplayFunc: (data: T, onChange: (data: T) => void) => React.Node,
    +onChange ?: (newItems: Array<T>) => void,   // Not present for read-only display
    +addNewItem: () => T,
      +addItemLabel ?: string,
|};

export default function MultiStacker<T: TItem>({
  items,
  itemDisplayFunc,
  onChange,
  addNewItem,
  addItemLabel = "Add",
}: Props<T>): React.Node {
  return (
    <Group gap={20} flexDirection="column">
      {items.map(item => (
        <>
          <div key={item.id} style={{ display: "flex", alignItems: "center" }}>
            {itemDisplayFunc(
              item,
              (newItem) => {
                if (onChange) {
                  const newArray = items.map(x => x.id === newItem.id ? newItem : x);
                  onChange(newArray);
                }
              },
            )}
            {onChange ? (
              <IconButton
                iconName="trash"
                type="button"
                onClick={() =>
                  onChange(items.filter(x => x.id !== item.id))
                }
              />
            ) : null}
          </div>
          <div className={css(styles.divider)}/>
        </>
      ))}
      {onChange ? (
        <Button
          intent="basic"
          kind="bare"
          onClick={() =>
            onChange([
              ...items,
              addNewItem(), // Add a new one at end
            ])
          }
          label={addItemLabel}
        />
      ) : null}
    </Group>
  );
}

const styles = StyleSheet.create({
  divider: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey30}`,
    margin: "10px 0",
    padding: "0",
  },
});