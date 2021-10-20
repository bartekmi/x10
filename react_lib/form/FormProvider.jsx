// @flow

import * as React from "react";

import Group from "latitude/Group";

export type FormError = {|
  +error: string,
  +paths: $ReadOnlyArray<string>, // Paths of fields that this error is for (multiple for cross-validations)
|};
export type FormContextType = {|
  +errors: $ReadOnlyArray<FormError>,
|};

export function addError(
  errors: Array<FormError>,
  prefix: ?string, 
  error: string, 
  paths: $ReadOnlyArray<string>
): void {
  const formContext = React.useContext(FormContext);
  errors.push({
    error,
    paths: paths.map(x => prefix == null ? x : `${prefix}.${x}`),
  });
}

export function errorMessageForPath(path: string): string | null {
  const context = React.useContext(FormContext);
  const errors = context.errors.filter(x => x.paths.includes(path));
  return errors.length == 0 ? null : errors.map(x => x.error).join("\n");
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
      <Group flexDirection="column" gap={20}>
        { children }
      </Group>
    </FormContext.Provider>    
  );
}
