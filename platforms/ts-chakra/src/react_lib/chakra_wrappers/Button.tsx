import * as React from "react";

import { Button as ChakraButton, Link } from '@chakra-ui/react'

type Props = {
  readonly label: string,
  readonly onClick?: () => unknown,
  readonly url?: string,
  readonly disabled: boolean,
  // TODO: This does nothing
  readonly style?: "normal" | "link"
};
export default function Button(props: Props): React.JSX.Element {
  const {label, onClick, url, disabled=false, style} = props;

  // const kind = style === "link" ? "bare" : "solid";
  
  return (
    url == null ?
      <ChakraButton 
        onClick={onClick} /*kind={kind}*/ 
        isDisabled={disabled}
        size="s">{label}
      </ChakraButton> :
      <Link href={url}>{label}</Link>
  );
}
