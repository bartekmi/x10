import * as React from "react";

import { Spinner } from '@chakra-ui/react'
import { useQuery } from '@apollo/client';

// TODO: Make ours searchable, too.
// import SearchableSelectInput from "latitude/select/SearchableSelectInput";
import SelectInput from "../chakra_wrappers/SelectInput";

type Option = {
  value?: string,
  label: string,
};

type Props = {
  readonly id?: string,  // This should be the unique id of the currently selected object
  readonly query: any,
  readonly toStringRepresentation?: (item: any) => string,
  readonly onChange: (value?: string) => void,
  readonly isNullable: boolean,
  readonly order?: "sameAsDefined" | "alphabetic",
};
export default function AssociationEditor(props: Props): React.JSX.Element {
  const {id, query, toStringRepresentation, onChange, isNullable, order='alphabetic'} = props;

  const { loading, error, data } = useQuery(
    query, { variables: {} }
  );

  if (loading)
    return <Spinner/>
  if (error)
    return <div>`Error! ${error.message}`</div>;      

  let options: Option[] = data.entities.map((x: any) => ({  
        value: x.id, 
        label: toStringRepresentation == null ? x.id : toStringRepresentation(x)
      }));

  // function compare( a: Option, b: Option ) {
  //   if ( a.label < b.label ){
  //     return -1;
  //   }
  //   if ( a.label > b.label ){
  //     return 1;
  //   }
  //   return 0;
  // }

  // if (order === "alphabetic") {
  //   options = options.sort(compare);
  // }

  return (
    <SelectInput
      value={id || null}
      options={options}
      onChange={ value => onChange(value) }
      isNullable={ isNullable }
    />
  );
}