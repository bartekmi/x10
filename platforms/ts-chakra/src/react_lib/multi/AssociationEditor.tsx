import * as React from "react";

import { Spinner } from '@chakra-ui/react'
// import SearchableSelectInput from "latitude/select/SearchableSelectInput";


type Props = {
  readonly id?: string,  // This should be the unique id of an object
  readonly query: any,
  readonly toString: (item: any) => string,
  readonly onChange: (value: string) => void,
  readonly isNullable: boolean,
  readonly order?: "sameAsDefined" | "alphabetic",
};
export default function AssociationEditor({id, query, toString, onChange, isNullable, order = "alphabetic"}: Props): React.JSX.Element {

//   return (
//     <QueryRenderer
//       environment={environment}
//       query={query}
//       variables={{
//       }}
//       render={({ error, props }) => {
//         if (error) {
//           return <div>{error.message}</div>;
//         } else if (props) {
//           const data = props.entities;
//           let options = data.map(x => ({ 
//                 value: x.id, 
//                 label: toString(x)
//               }));

//           if (order === "alphabetic") {
//             options = options.sort((a, b) => a.label.localeCompare(b.label));
//           }

//           return (
//             // <SearchableSelectInput
//             //   value={id || null}
//             //   options={options}
//             //   onChange={ value => onChange(value) }
//             //   isNullable={ isNullable }
//             // />
//             "TODO"
//           )
//         }
//         return <Spinner/>;
//       }}
//     />
//   );
  return <>TODO</>
}