import * as React from "react";

import { Spinner } from '@chakra-ui/react'
import { useQuery } from '@apollo/client';

// TODO: Make ours searchable, too.
// import SearchableSelectInput from "latitude/select/SearchableSelectInput";
import SelectInput from "../chakra_wrappers/SelectInput";

type Props = {
  readonly id?: string,  // This should be the unique id of the currently selected object
  readonly query: any,
  readonly toString: (item: any) => string,
  readonly onChange: (value?: string) => void,
  readonly isNullable: boolean,
  readonly order?: "sameAsDefined" | "alphabetic",
};
export default function AssociationEditor(props: Props): React.JSX.Element {
  const {id, query, toString, onChange, isNullable, order} = props;

  const { loading, error, data } = useQuery(
    query, { variables: {} }
  );

  if (loading)
    return <Spinner/>
  if (error)
    return <div>`Error! ${error.message}`</div>;      

  let options = data.entities.map((x: any) => ({  
        value: x.id, 
        label: toString(x)
      }));

  if (order === "alphabetic") {
    options = options.sort((a: any, b: any) => a.label.localeCompare(b.label));
  }

  return (
    <SelectInput
      value={id || null}
      options={options}
      onChange={ value => onChange(value) }
      isNullable={ isNullable }
    />
  );
}