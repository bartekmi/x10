// @flow

import * as React from "react";

type FormError = string;
type FormContextType = Array<FormError>;

const FormContext: React.Context<FormContextType> = React.createContext([] /* dummy value */);
export const FormProvider = FormContext.Provider

export default FormContext;
