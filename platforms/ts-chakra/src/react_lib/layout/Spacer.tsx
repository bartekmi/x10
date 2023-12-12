import React from 'react';

type Props = {
  readonly size: number
}
export default function(props: Props) {
  const { size } = props;

    const style = {
        display: 'inline-block', // Use 'block' for vertical space
        width: size, // For horizontal space
    };

    return <span style={style} />;
};
