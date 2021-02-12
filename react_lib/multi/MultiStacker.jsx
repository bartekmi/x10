/**
 * @flow 
 */

import * as React from "react";

import Group from "latitude/Group";
import Button from "latitude/button/Button";
import IconButton from "latitude/button/IconButton";

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
  const showDeleteIcon = items.length > 1

  return (
    <Group gap={20} flexDirection="column">
      {items.map(item => (
        <Group key={item.id} alignItems="center">
          {itemDisplayFunc(
            item,
            (newItem) => {
              if (onChange) {
                const newArray = items.map(x => x.id === newItem.id ? newItem : x);
                onChange(newArray);
              }
            },
          )}
          {showDeleteIcon && onChange ? (
            <IconButton
              iconName="trash"
              type="button"
              onClick={() =>
                onChange(items.filter(x => x.id !== item.id))
              }
            />
          ) : null}
        </Group>
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