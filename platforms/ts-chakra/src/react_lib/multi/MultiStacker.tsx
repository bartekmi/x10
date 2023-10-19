import * as React from "react";
import {StyleSheet, css} from "aphrodite";

import { Button, Flex, IconButton } from '@chakra-ui/react'

import colors from "../colors"
import { DeleteIcon } from "@chakra-ui/icons";

type TItem = {
  readonly id: string,
};

type Props<T extends TItem> = {
  readonly items: T[],
  readonly itemDisplayFunc: (data: T, onChange: (data: T) => void, inListIndex?: number) => React.JSX.Element,
  readonly onChange ?: (newItems: Array<T>) => void,   // Not present for read-only display
  readonly addNewItem: () => T,
  readonly addItemLabel ?: string,
  readonly layout?: "vertical" | "verticalCompact" | "wrap",
};
export default function MultiStacker<T extends TItem>(props: Props<T>): React.JSX.Element {
  const {layout = "vertical"} = props;
  if (layout == "wrap") {
    return WrapPanel(props);
  }

  return VerticalList(props);
}

function WrapPanel<T extends TItem>({
  items,
  itemDisplayFunc
}: Props<T>) {
  // TODO: Currently, not editable
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
}: Props<T>): React.JSX.Element {
  const isCompact = layout == "verticalCompact";

  return (
    <Flex gap={isCompact ? 0 : 20} flexDirection="column">
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
          {isCompact ? null : <div className={css(styles.divider)}/>}
        </>
      ))}
      {onChange ? (
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
      ) : null}
    </Flex>
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