import * as React from "react";

import VerticalStackPanel from "../layout/VerticalStackPanel";

export type FormError = {
  readonly error: string,
  readonly paths: string[], // Paths of fields that this error is for (multiple for cross-validations)
  readonly inListIndex?: number,
};
export type FormContextType = {
  readonly errors: FormError[],
};

export function addError(
  errors: Array<FormError>,
  error: string, 
  paths: string[],
  prefix?: string, 
  inListIndex?: number,
): void {
  errors.push({
    error,
    paths: paths.map(x => prefix == null ? x : `${prefix}.${x}`),
    inListIndex,
  });
}

export function createError(
  error: string, 
  paths: string[],
  prefix?: string, 
  inListIndex?: number,
): FormError {
  return {
    error,
    paths: paths.map(x => prefix == null ? x : `${prefix}.${x}`),
    inListIndex,
  }
}

export const FormContext: React.Context<FormContextType> = React.createContext<FormContextType>({
  errors: [],
});

type Props = {
  readonly children: React.JSX.Element,
  readonly context: any,
};
export default function EditForm(props: Props): React.JSX.Element {
  const { children, context } = props

  return (
    <FormContext.Provider value={context}>
      <VerticalStackPanel gap={20}>
        { children }
      </VerticalStackPanel>
    </FormContext.Provider>    
  );
}
