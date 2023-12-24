import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import { Button, Flex, IconButton } from '@chakra-ui/react'

import colors from "../colors"
import { DeleteIcon } from "@chakra-ui/icons";

type TItem = {
  readonly id: string,
};

type Props<T extends TItem> = {
  readonly items: T[] | null | undefined,
  readonly itemDisplayFunc: (data: T, onChange: (data: T) => void, inListIndex?: number) => React.JSX.Element,
  readonly onChange?: (newItems: Array<T>) => void,   // Not present for read-only display
  readonly addNewItem?: (() => T) | null,   // Not present for read-only display
  readonly addItemLabel ?: string,
  readonly layout?: "vertical" | "verticalCompact" | "wrap",
};
export default function MultiStacker<T extends TItem>(props: Props<T>): React.JSX.Element | null {
  const {layout = "vertical", items} = props;
  if (layout == "wrap") {
    return WrapPanel(props);
  }

  return VerticalList(props);
}

function WrapPanel<T extends TItem>({
  items,
  itemDisplayFunc
}: Props<T>) {
  if (items == null)
    return null;

  return (
    <Flex gap={12}>
      {items.map((item, index) => itemDisplayFunc(item, () => {}, index))}
    </Flex>
  );
}

function VerticalList<T extends TItem>({
  items,
  itemDisplayFunc,
  onChange,
  addNewItem,
  addItemLabel = "Add",
  layout,
}: Props<T>): React.JSX.Element | null{
  const isCompact = layout == "verticalCompact";
  if (items == null)
    return null;

  const showAddNew = onChange && addNewItem;

  return (
    <Flex gap={isCompact ? 0 : 4} flexDirection="column">
      {items.map((item, index) => (
        <>
          <div key={item.id} style={{ display: "flex", alignItems: "flex-end"}}>
            {itemDisplayFunc(
              item,
              (newItem) => {
                if (onChange) {
                  const newArray = items.map(x => x.id === newItem.id ? newItem : x);
                  onChange(newArray);
                }
              },
              index
            )}
            {onChange ? (
              <div style={{marginLeft: "20px"}}>
                <IconButton
                  icon={<DeleteIcon/>}
                  type="button"
                  onClick={() =>
                    onChange(items.filter(x => x.id !== item.id))
                  }
                  aria-label="Delete"
                />
              </div>
            ) : null}
          </div>
          {isCompact || (index == items.length - 1 && !showAddNew) ? 
            null : 
            <div className={css(styles.divider)}/>}
        </>
      ))}
      {showAddNew && AddNewButton(onChange, addNewItem, items, addItemLabel) }
    </Flex>
  );
}

function AddNewButton<T>(onChange: (newItems: Array<T>) => void, addNewItem: () => T, items: T[], addItemLabel: string) {
  return (
    <Button
      onClick={() =>
        onChange([
          ...items,
          addNewItem(), // Add a new one at end
        ])
      }
    >
      {addItemLabel}
    </Button>
  );
}

const styles = StyleSheet.create({
  divider: {
    height: "2px",
    borderBottom: `1px solid ${colors.grey50}`,
    margin: "10px 0",
    padding: "0",
  },
});