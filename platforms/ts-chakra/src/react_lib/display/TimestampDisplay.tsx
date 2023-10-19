import moment from "moment-timezone";
import * as React from "react";

import { Text } from '@chakra-ui/react'
import { Flex } from '@chakra-ui/react'

type Props = {
  readonly value?: string,
  readonly weight?: "bold",
};
export default function TimestampDisplay(props: Props): React.JSX.Element | null {
  const {value, weight} = props;

  if (value == null)
    return null;

  const theMoment = moment(value, [moment.ISO_8601, "YYYY-MM-DD"]);
  const formattedDate = theMoment.format("YYYY-MM-DD");
  const formattedTime = theMoment.format("h:mm A");

  const nonNullWeight = weight || "regular";
  const as = weight == "bold" ? "b" : undefined;
  return (
    <Flex gap={12}>
      <Text as={as}>{formattedDate}</Text>
      <Text as={as}>{formattedTime}</Text>
    </Flex>
  );
}
