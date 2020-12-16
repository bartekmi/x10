// @flow

import * as React from "react";

type FormError = string;
type FormContextType = Array<FormError>;

export const FormContext: React.Context<FormContextType> = React.createContext([] /* dummy value */);
const FormProvider = FormContext.Provider

export default FormProvider;
