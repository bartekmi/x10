// @flow

import * as React from "react";

import VerticalStackPanel from "../layout/VerticalStackPanel";

export type FormError = {|
  +error: string,
  +paths: $ReadOnlyArray<string>, // Paths of fields that this error is for (multiple for cross-validations)
  +inListIndex?: number,
|};
export type FormContextType = {|
  +errors: $ReadOnlyArray<FormError>,
|};

export function addError(
  errors: Array<FormError>,
  prefix: ?string, 
  error: string, 
  paths: $ReadOnlyArray<string>,
  inListIndex?: number,
): void {
  errors.push({
    error,
    paths: paths.map(x => prefix == null ? x : `${prefix}.${x}`),
    inListIndex,
  });
}

export const FormContext: React.Context<FormContextType> = React.createContext({
  errors: [],
});
const FormProvider = FormContext.Provider

type Props = {|
  +children: React.Node,
  +value: any,
|};
export default function EditForm(props: Props): React.Node {
  const { children, value } = props

  return (
    <FormContext.Provider value={value}>
      <VerticalStackPanel gap={20}>
        { children }
      </VerticalStackPanel>
    </FormContext.Provider>    
  );
}
